namespace LoLAccountChecker.Data
{
    public class Champion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Skin[] Skins { get; set; }
    }

    public class Skin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }
}