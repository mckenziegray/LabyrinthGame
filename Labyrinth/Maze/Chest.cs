using System.Collections.Generic;

namespace Labyrinth
{
    class Chest
    {
        private const float CHANCE_FOR_DOUBLE_LOOT = 0.2f;
        private const float CHANCE_FOR_TRAP = 0.5f;
        private const float CHANCE_FOR_VISIBLE_STATUS = 0.5f;
        private const int MIN_TRAP_DAMAGE = 1;
        private const int MAX_TRAP_DAMAGE = 3;

        public ItemList Items { get; private set; }
        public bool IsTrapped { get; private set; }
        public bool StatusVisible { get; private set; }

        public Chest()
        {
            Items = new ItemList();
            Items.Add(Item.RandomItem(Items));
            if (Utils.Roll(CHANCE_FOR_DOUBLE_LOOT))
            {
                Items.Add(Item.RandomItem(Items));
            }

            IsTrapped = Utils.Roll(CHANCE_FOR_TRAP);
            StatusVisible = Utils.Roll(CHANCE_FOR_VISIBLE_STATUS);
        }

        public int TriggerTrap()
        {
            int damage = IsTrapped ? Utils.Random.Next(MIN_TRAP_DAMAGE, MAX_TRAP_DAMAGE) : 0;

            IsTrapped = false;

            return damage;
        }
    }
}
