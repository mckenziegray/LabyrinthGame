using System.Data;
using System.Linq;

namespace Labyrinth
{
    public class Shield : Item
    {
        public const float CHANCE_FOR_KITE = 0.25f;

        public ShieldType ShieldType { get; private set; }
        public int Defense { get; private set; }

        public Shield() : base(ItemType.Shield) { }

        /// <summary>
        /// Constructs a <see cref="Shield"/> object of the given type
        /// </summary>
        /// <param name="type">The type of <see cref="Shield"/> to construct</param>
        public Shield(ShieldType type) : base(ItemType.Shield)
        {
            ShieldDataEntry data = ShieldDao.GetData()[type];

            ShieldType = type;
            Defense = data.Defense;
            Value = data.Value;
        }

        /// <summary>
        /// Constructs a <see cref="Shield"/> object of a random type
        /// </summary
        public static Shield RandomShield()
        {
            ShieldType type;
            float p = Utils.GetRandomPercent();

            if (p < CHANCE_FOR_KITE)
            {
                type = ShieldType.Kite;
            }
            else
            {
                type = ShieldType.Wood;
            }

            return new Shield(type);
        }
    }
}
