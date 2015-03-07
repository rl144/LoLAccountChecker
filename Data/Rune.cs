namespace LoLAccountChecker.Data
{
    public class Rune
    {
        public enum Types
        {
            Unknown,
            Mark,
            Seal,
            Glyph,
            Quintessence
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Tier { get; set; }
        public Types Type { get; set; }
    }
}