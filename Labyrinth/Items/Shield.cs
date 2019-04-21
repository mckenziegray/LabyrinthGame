namespace Labyrinth
{
    class Shield : Item
    {
        public const float CHANCE_FOR_KITE = 0.25f;

        public ShieldType ShieldType { get; private set; }
        public int Defense { get; private set; }

        /// <summary>
        /// Constructs a <see cref="Shield"/> object of a random type
        /// </summary>
        public Shield() : base(ItemType.Shield)
        {
            float p = Utils.GetRandomPercent();

            if (p < CHANCE_FOR_KITE)
            {
                ShieldType = ShieldType.Kite;
            }
            else
            {
                ShieldType = ShieldType.Wood;
            }

            SetDefense();
        }

        /// <summary>
        /// Constructs a <see cref="Shield"/> object of the given type
        /// </summary>
        /// <param name="type">The type of <see cref="Shield"/> to construct</param>
        public Shield(ShieldType type) : base(ItemType.Shield)
        {
            ShieldType = type;
            SetDefense();
        }

        /// <summary>
        /// Sets the <see cref="Defense"/> property based on the <see cref="ShieldType"/>
        /// </summary>
        private void SetDefense()
        {
            switch (ShieldType)
            {
                case ShieldType.Wood:
                    Defense = 4;
                    break;
                case ShieldType.Kite:
                    Defense = 6;
                    break;
            }
        }
    }
}
