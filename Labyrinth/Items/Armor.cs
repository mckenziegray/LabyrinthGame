using System.Data;
using System.Linq;

namespace Labyrinth
{
    public class Armor : Item
    {
        private const float CHANCE_FOR_IRON = 1.0f / 6.0f;
        private const float CHANCE_FOR_CHAIN = 1.0f / 3.0f;

        public ArmorType ArmorType { get; private set; }

        public int Defense { get; private set; }

        public Armor() : base(ItemType.Armor) { }

        /// <summary>
        /// Creates an <see cref="Armor"/> object of a particular type
        /// </summary>
        /// <param name="type">The type of armor to create</param>
        public Armor(ArmorType type) : base(ItemType.Armor)
        {
            ArmorDataEntry data = ArmorDao.GetData()[type];

            ArmorType = type;
            Defense = data.Defense;
            Value = data.Value;
        }

        /// <summary>
        /// Creates an <see cref="Armor"/> object of a random type
        /// </summary>
        public static Armor RandomArmor()
        {
            ArmorType type;
            float p = Utils.GetRandomPercent();

            if (p < CHANCE_FOR_IRON)
            {
                type = ArmorType.Iron;
            }
            else if (p < CHANCE_FOR_IRON + CHANCE_FOR_CHAIN)
            {
                type = ArmorType.Chain;
            }
            else
            {
                type = ArmorType.Leather;
            }

            return new Armor(type);
        }
        
        public override bool CompareTo(Item other)
        {
            bool result;

            if (other is Armor)
            {
                result = Defense > (other as Armor).Defense;
            }
            else
            {
                result = base.CompareTo(other);
            }

            return result;
        }
    }
}
