namespace Labyrinth
{
    class Armor : Item
    {
        private const float CHANCE_FOR_IRON = 1.0f / 6.0f;
        private const float CHANCE_FOR_CHAIN = 1.0f / 3.0f;

        public ArmorType ArmorType { get; private set; }

        public int Defense { get; private set; }

        /// <summary>
        /// Creates an <see cref="Armor"/> object of a random type
        /// </summary>
        public Armor() : base(ItemType.Armor)
        {
            float p = Utils.GetRandomPercent();

            if (p < CHANCE_FOR_IRON)
            {
                ArmorType = ArmorType.Iron;
            }
            else if (p < CHANCE_FOR_IRON + CHANCE_FOR_CHAIN)
            {
                ArmorType = ArmorType.Chain;
            }
            else
            {
                ArmorType = ArmorType.Leather;
            }

            SetDefense();
        }

        /// <summary>
        /// Creates an <see cref="Armor"/> object of a particular type
        /// </summary>
        /// <param name="type">The type of armor to create</param>
        public Armor(ArmorType type) : base(ItemType.Armor)
        {
            ArmorType = type;
            SetDefense();
        }

        /// <summary>
        /// Sets the <see cref="Defense"/> property according to the <see cref="ArmorType"/>
        /// </summary>
        private void SetDefense()
        {
            switch (ArmorType)
            {
                case ArmorType.Leather:
                    Defense = 3;
                    break;
                case ArmorType.Chain:
                    Defense = 6;
                    break;
                case ArmorType.Iron:
                    Defense = 9;
                    break;
            }
        }
    }
}
