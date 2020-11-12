namespace Labyrinth
{
    public struct StatIncrease
    {
        public string Stat { get; set; }
        public int IncreaseAmount { get; set; }
        public int NewAmount { get; set; }

        public StatIncrease(string statName, int increaseAmount, int newAmount)
        {
            Stat = statName;
            IncreaseAmount = increaseAmount;
            NewAmount = newAmount;
        }
    }
}
