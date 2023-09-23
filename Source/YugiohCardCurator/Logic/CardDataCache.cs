using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AsgardCore;
using YugiohCardCurator.DTOs;

namespace YugiohCardCurator.Logic
{
    internal sealed class CardDataCache
    {
        private Dictionary<string, CacheEntry> _Data;
        private Task _LoadTask;

        public CardDataCache()
        {
            _LoadTask = Task.Run(Load);
        }

        public void Add(string name, CardData data)
        {
            WaitForLoad();

            CacheEntry entry = new CacheEntry(data);
            if (_Data.Count > 63)
            {
                // Find and remove oldest entry.
                int max = -1;
                CacheEntry oldest = null;
                foreach (CacheEntry e in _Data.Values)
                {
                    e.Age++;
                    if (e.Age > max)
                    {
                        max = e.Age;
                        oldest = e;
                    }
                }

                _Data.Remove(oldest.Card.Name);
            }

            _Data.Add(name, entry);
            Task.Run(Save);
        }

        public bool TryGet(string name, out CardData data)
        {
            WaitForLoad();

            bool contains = _Data.TryGetValue(name, out CacheEntry entry);
            if (contains)
            {
                // Update ages and make the current element the youngest.
                foreach (CacheEntry e in _Data.Values)
                    e.Age++;
                entry.Age = 0;
            }

            data = entry?.Card;
            return contains;
        }

        private void Load()
        {
            if (File.Exists("CardDataCache.gz"))
            {
                using (FileStream fs = new FileStream("CardDataCache.gz", FileMode.Open))
                {
                    using (GZipStream gs = new GZipStream(fs, CompressionMode.Decompress))
                    {
                        using (BinaryReader br = new BinaryReader(gs, Encoding.UTF8))
                        {
                            _Data = Extensions.DeserializeStringISerializableDictionary32(br, CacheEntry.Restore);
                        }
                    }
                }

                return;
            }

            _Data = new Dictionary<string, CacheEntry>();
        }

        private void Save()
        {
            using (FileStream fs = new FileStream("CardDataCache.gz", FileMode.Create))
            {
                using (GZipStream gs = new GZipStream(fs, CompressionLevel.Optimal))
                {
                    using (BinaryWriter bw = new BinaryWriter(gs, Encoding.UTF8))
                    {
                        _Data.Serialize32(bw);
                    }
                }
            }
        }

        private void WaitForLoad()
        {
            if (_LoadTask != null)
            {
                _LoadTask.Wait();
                _LoadTask = null;
            }
        }

        private sealed class CacheEntry : ISerializable
        {
            public readonly CardData Card;
            public int Age;

            public CacheEntry(CardData card)
            {
                Card = card;
            }

            public CacheEntry(BinaryReader br)
            {
                string json = br.ReadString();
                Card = JsonSerializer.Deserialize<CardData>(json);
                Age = br.ReadInt32();
            }

            public static CacheEntry Restore(BinaryReader br)
            {
                return new CacheEntry(br);
            }

            public void Serialize(BinaryWriter bw)
            {
                string json = JsonSerializer.Serialize(Card);
                bw.Write(json);
                bw.Write(Age);
            }
        }
    }
}
