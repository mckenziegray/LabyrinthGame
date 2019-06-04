using System.Data;
using System.Linq;

namespace Labyrinth
{
    public class Weapon : Item
    {
        private const float CHANCE_FOR_AXE = 1.0f / 6.0f;
        private const float CHANCE_FOR_SWORD = 1.0f / 3.0f;

        public WeaponType WeaponType { get; private set; }
        public int Damage { get; private set; }

        public Weapon() : base(ItemType.Weapon) { }

        /// <summary>
        /// Constructs a <see cref="Weapon"/> object of the given type
        /// </summary>
        /// <param name="type"></param>
        public Weapon(WeaponType type) : base(ItemType.Weapon)
        {
            WeaponDataEntry data = WeaponDao.GetData()[type];

            WeaponType = type;
            Damage = data.Damage;
            Value = data.Value;
        }

        /// <summary>
        /// Constructs a <see cref="Weapon"/> object of a random type
        /// </summary>
        public static Weapon RandomWeapon()
        {
            WeaponType type;
            float p = Utils.GetRandomPercent();

            if (p < CHANCE_FOR_AXE)
            {
                type = WeaponType.Axe;
            }
            else if (p < CHANCE_FOR_AXE + CHANCE_FOR_SWORD)
            {
                type = WeaponType.Sword;
            }
            else
            {
                type = WeaponType.Dagger;
            }

            return new Weapon(type);
        }

        public override bool CompareTo(Item other)
        {
            bool result;

            if (other is Weapon)
            {
                result = Damage > (other as Weapon).Damage;
            }
            else
            {
                result = base.CompareTo(other);
            }

            return result;
        }
    }
}
