using System;
using System.Globalization;
using System.IO;
using AsgardCore;

namespace YugiohCardCurator.Logic
{
    internal sealed class MonsterCard : ISerializable
    {
        public const string Header = "Name;PrintTag;Types;Attribute;Level;ATK;DEF;InitialPrice;CurrentPrice";
        private const string Separator = ";";

        public string Name { get; }
        public string PrintTag { get; }
        public string Types { get; }
        public string Attribute { get; }
        public int Level { get; }
        public string ATK { get; }
        public string DEF { get; }
        public float InitialPrice { get; }
        public float CurrentPrice { get; }

        public MonsterCard(string name, string printTag, string types, string attribute, int level, string atk, string def, float initialPrice, float currentPrice)
        {
            Name = name;
            PrintTag = printTag;
            Types = types;
            Attribute = attribute;
            Level = level;
            ATK = atk;
            DEF = def;
            InitialPrice = initialPrice;
            CurrentPrice = currentPrice;
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
            InitialPrice = Convert.ToSingle(parts[7], CultureInfo.InvariantCulture);
            CurrentPrice = Convert.ToSingle(parts[8], CultureInfo.InvariantCulture);
        }

        public void Serialize(BinaryWriter bw)
        {
            string s = string.Join(Separator, Name, PrintTag, Types, Attribute, Level.ToStringInvariant(), ATK, DEF, InitialPrice.ToStringInvariant(), CurrentPrice.ToStringInvariant());
            bw.Write(s);
            bw.Write(Environment.NewLine);
        }

        public static MonsterCard Restore(BinaryReader br)
        {
            return new MonsterCard(br);
        }
    }
}
