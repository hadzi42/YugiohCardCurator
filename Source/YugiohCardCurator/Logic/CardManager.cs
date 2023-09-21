using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using AsgardCore;

namespace YugiohCardCurator.Logic
{
    internal sealed class CardManager
    {
        public List<Card> Monsters = new List<Card>();

        public event Action<Card> MonsterAdded;

        public void Add(Card card)
        {
            Monsters.Add(card);
            MonsterAdded(card);
        }

        public void Save(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (GZipStream gs = new GZipStream(fs, CompressionLevel.Optimal))
                {
                    using (BinaryWriter bw = new BinaryWriter(gs, Encoding.UTF8))
                    {
                        bw.Write(Card.Header);
                        bw.Write(Environment.NewLine);
                        Monsters.Serialize32(bw);
                    }
                }
            }
        }

        public void Load(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (GZipStream gs = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (BinaryReader br = new BinaryReader(gs, Encoding.UTF8))
                    {
                        br.ReadString();
                        br.ReadString();
                        Monsters = Extensions.DeserializeISerializableList32(br, Card.Restore);
                    }
                }
            }
        }
    }
}
