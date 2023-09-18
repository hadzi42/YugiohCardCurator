namespace YugiohCardCurator.Logic
{
    internal sealed class Card
    {
        public string Name { get; }
        public string PrintTag { get; }
        public string Types { get; }
        public string Attribute { get; }
        public int Level { get; }
        public int ATK { get; }
        public int DEF { get; }

        public Card(string name, string printTag, string types, string attribute, int level, int atk, int def)
        {
            Name = name;
            PrintTag = printTag;
            Types = types;
            Attribute = attribute;
            Level = level;
            ATK = atk;
            DEF = def;
        }
    }
}
