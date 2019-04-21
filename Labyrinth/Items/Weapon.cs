namespace Labyrinth
{
    class Weapon : Item
    {
        private const float CHANCE_FOR_AXE = 1.0f / 6.0f;
        private const float CHANCE_FOR_SWORD = 1.0f / 3.0f;

        public WeaponType WeaponType { get; private set; }

        public int Damage { get; private set; }

        /// <summary>
        /// Constructs a <see cref="Weapon"/> object of a random type
        /// </summary>
        public Weapon() : base(ItemType.Weapon)
        {
            float p = Utils.GetRandomPercent();

            if (p < CHANCE_FOR_AXE)
            {
                WeaponType = WeaponType.Axe;
            }
            else if (p < CHANCE_FOR_AXE + CHANCE_FOR_SWORD)
            {
                WeaponType = WeaponType.Sword;
            }
            else
            {
                WeaponType = WeaponType.Dagger;
            }

            SetDamage();
        }

        /// <summary>
        /// Constructs a <see cref="Weapon"/> object of the given type
        /// </summary>
        /// <param name="type"></param>
        public Weapon(WeaponType type) : base(ItemType.Weapon)
        {
            WeaponType = type;
            SetDamage();
        }

        /// <summary>
        /// Sets the <see cref="Damage"/> property based on the <see cref="WeaponType"/>
        /// </summary>
        private void SetDamage()
        {
            switch (WeaponType)
            {
                case WeaponType.Dagger:
                    Damage = 5;
                    break;
                case WeaponType.Sword:
                    Damage = 10;
                    break;
                case WeaponType.Axe:
                    Damage = 10;
                    break;
            }
        }
    }
}
