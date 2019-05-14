using System.Data;
using System.Linq;

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

            DataRow entry = WeaponDao.GetTable().Select($"{nameof(WeaponType)} = '{WeaponType}'").FirstOrDefault();
            
            Damage = (int)entry[nameof(Damage)];
            Value = (int)entry[nameof(Value)];
        }

        /// <summary>
        /// Constructs a <see cref="Weapon"/> object of the given type
        /// </summary>
        /// <param name="type"></param>
        public Weapon(WeaponType type) : base(ItemType.Weapon)
        {
            DataRow entry = WeaponDao.GetTable().Select($"{nameof(WeaponType)} = '{type}'").FirstOrDefault();

            WeaponType = type;
            Damage = (int)entry[nameof(Damage)];
            Value = (int)entry[nameof(Value)];
        }
    }
}
