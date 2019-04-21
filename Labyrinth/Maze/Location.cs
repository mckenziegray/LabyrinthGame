using System.Collections.Generic;

namespace Labyrinth
{
    class Location
    {
        private float CHANCE_FOR_LOOT = 1f / 6f;
        private float CHANCE_FOR_DOUBLE_LOOT = 0.2f;
        private float CHANCE_FOR_CHEST = 1f / 3f;

        private Enemy enemy;

        public bool IsRoom { get; private set; }
        public Location[] Neighbors;
        public ItemList Items { get; set; }
        public Chest Chest { get; private set; }
        public Enemy Enemy
        {
            get
            {
                return enemy;
            }
            set
            {
                enemy = value;

                if (enemy != null && enemy.Location != this)
                    enemy.Location = this;
            }
        }

        public Location()
        {
            Neighbors = new Location[4];
            Items = new ItemList();
            Chest = null;
            Enemy = null;
            IsRoom = false;
        }

        public Location(bool isRoom)
        {
            Neighbors = new Location[4];
            Items = new ItemList();
            Chest = null;
            Enemy = null;
            IsRoom = isRoom;
            
            // Determine if there is any loot
            if (Utils.Roll(CHANCE_FOR_LOOT))
            {
                Items.Add(Item.RandomItem(Items));

                if (Utils.Roll(CHANCE_FOR_DOUBLE_LOOT))
                {
                    Items.Add(Item.RandomItem(Items));
                }
            }
            
            // If it's a room, decide if there's a chest
            if (IsRoom)
            {
                if (Utils.Roll(CHANCE_FOR_CHEST))
                {
                    Chest = new Chest();
                }
            }
        }

        public Location GetNeighbor(Direction dir)
        {
            return Neighbors[(int)dir];
        }
    }
}
