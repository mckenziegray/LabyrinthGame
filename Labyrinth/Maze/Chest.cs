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
        public Trap Trap { get; private set; }
        public bool StatusVisible { get; private set; }
        public bool IsTrapped
        {
            get
            {
                return Trap != null && Trap.IsActive;
            }
        }

        public Chest()
        {
            Items = new ItemList();
            Items.Add(Item.RandomItem(Items));
            if (Utils.Roll(CHANCE_FOR_DOUBLE_LOOT))
            {
                Items.Add(Item.RandomItem(Items));
            }

            Trap = Utils.Roll(CHANCE_FOR_TRAP) ? new Trap(MIN_TRAP_DAMAGE, MAX_TRAP_DAMAGE) : null;
            StatusVisible = Utils.Roll(CHANCE_FOR_VISIBLE_STATUS);
        }
    }
}
