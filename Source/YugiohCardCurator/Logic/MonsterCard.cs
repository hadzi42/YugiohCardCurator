using System;
using System.Globalization;
using System.IO;
using AsgardCore;

namespace YugiohCardCurator.Logic
{
    internal sealed class MonsterCard : ISerializable
    {
        public const string Header = "Name;PrintTag;Types;Attribute;Level;ATK;DEF;Id;Border;Title;Image;Edition;Storage;Rarity;InitialPrice;CurrentPrice";
        private const string Separator = ";";

        public string Name { get; }
        public string PrintTag { get; }
        public string Types { get; }
        public string Attribute { get; }
        public int Level { get; }
        public string ATK { get; }
        public string DEF { get; }
        public string Id { get; }
        public string Border { get; }
        public string Title { get; }
        public string Image { get; }
        public string Edition { get; }
        public string Storage { get; }
        public string Rarity { get; }
        public float InitialPrice { get; }
        public float CurrentPrice { get; set; }

        public MonsterCard(string name, string printTag, string types, string attribute, int level, string atk, string def, string id, string border, string title, string image, string edition, string storage, string rarity, float initialPrice)
        {
            Name = name;
            PrintTag = printTag;
            Types = types;
            Attribute = attribute;
            Level = level;
            ATK = atk;
            DEF = def;
            Id = id;
            Border = border;
            Title = title;
            Image = image;
            Edition = edition;
            Storage = storage;
            Rarity = rarity;
            InitialPrice = initialPrice;
            CurrentPrice = initialPrice;
        }

        public MonsterCard(BinaryReader br)
        {
            string s = br.ReadString();
            _ = br.ReadString();
            string[] parts = s.Split(Separator);

            Name = parts[0];
            PrintTag = parts[1];
            Types = parts[2];
            Attribute = parts[3];
            Level = Convert.ToInt32(parts[4], CultureInfo.InvariantCulture);
            ATK = parts[5];
            DEF = parts[6];
            Id = parts[7];
            Border = parts[8];
            Title = parts[9];
            Image = parts[10];
            Edition = parts[11];
            Storage = parts[12];
            Rarity = parts[13];
            InitialPrice = Convert.ToSingle(parts[14], CultureInfo.InvariantCulture);
            CurrentPrice = Convert.ToSingle(parts[15], CultureInfo.InvariantCulture);
        }

        public void Serialize(BinaryWriter bw)
        {
            string s = string.Join(Separator, Name, PrintTag, Types, Attribute, Level.ToStringInvariant(), ATK, DEF, Id, Border, Title, Image, Edition, Storage, Rarity, InitialPrice.ToStringInvariant(), CurrentPrice.ToStringInvariant());
            bw.Write(s);
            bw.Write(Environment.NewLine);
        }

        public static MonsterCard Restore(BinaryReader br)
        {
            return new MonsterCard(br);
        }
    }
}
