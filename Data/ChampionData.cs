#region

using System;

#endregion

namespace LoLAccountChecker.Data
{
    public class ChampionData
    {
        public string Name { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Champion Champion;
    }
}