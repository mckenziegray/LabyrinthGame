using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    public class Location
    {
        private const float CHANCE_FOR_LOOT = 1f / 6f;
        private const float CHANCE_FOR_DOUBLE_LOOT = 0.2f;
        private const float CHANCE_FOR_CHEST = 1f / 3f;
        private const float CHANCE_FOR_TRAP = 1f / 10f;
        private const int MIN_TRAP_DAMAGE = 2;
        private const int MAX_TRAP_DAMAGE = 4;

        private Enemy _enemy;
        private Merchant _merchant;

        public bool IsRoom { get; private set; }
        public Location[] Neighbors { get; set; }
        public ItemList Items { get; set; }
        public Chest Chest { get; private set; }
        public Trap Trap { get; private set; }
        public bool IsTrapped => Trap != null && Trap.IsActive;
        public Enemy Enemy
        {
            get => _enemy;
            set
            {
                _enemy = value;

                // Ensure the enemy's location is this
                if (_enemy != null && _enemy.Location != this)
                    _enemy.Location = this;
            }
        }
        public Merchant Merchant
        {
            get => _merchant;
            set
            {
                _merchant = value;

                // Ensure the merchant's location is this
                if (_merchant != null && _merchant.Location != this)
                    _merchant.Location = this;
            }
        }

        public Location()
        {
            Neighbors = new Location[4];
            Items = new ItemList();
            Chest = null;
            Enemy = null;
            IsRoom = false;
            Trap = null;
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

                if (Utils.Roll(CHANCE_FOR_TRAP))
                {
                    Trap = new Trap(MIN_TRAP_DAMAGE, MAX_TRAP_DAMAGE);
                }
            }
        }

        public Location GetNeighbor(Direction dir)
        {
            return Neighbors[(int)dir];
        }

        public IEnumerable<Direction> GetValidDirections()
        {
            return ((int[])Enum.GetValues(typeof(Direction))).Where(d => Neighbors[d] is not null).Cast<Direction>();
        }
    }
}
