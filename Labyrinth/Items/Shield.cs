using System.Data;
using System.Linq;

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

            DataRow entry = ShieldDao.GetTable().Select($"{nameof(ShieldType)} = '{ShieldType}'").FirstOrDefault();
            
            Defense = (int)entry[nameof(Defense)];
            Value = (int)entry[nameof(Value)];
        }

        /// <summary>
        /// Constructs a <see cref="Shield"/> object of the given type
        /// </summary>
        /// <param name="type">The type of <see cref="Shield"/> to construct</param>
        public Shield(ShieldType type) : base(ItemType.Shield)
        {
            DataRow entry = ShieldDao.GetTable().Select($"{nameof(ShieldType)} = '{type}'").FirstOrDefault();

            ShieldType = type;
            Defense = (int)entry[nameof(Defense)];
            Value = (int)entry[nameof(Value)];
        }
    }
}
