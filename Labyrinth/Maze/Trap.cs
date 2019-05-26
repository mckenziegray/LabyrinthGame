using System;
using System.Collections.Generic;
using System.Text;

namespace Labyrinth
{
    public class Trap
    {
        private int MinDamage;
        private int MaxDamage;

        public bool IsActive { get; private set; }
        public bool StatusVisible { get; private set; }

        public Trap(int minDamage, int maxDamage)
        {
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            IsActive = false;
        }

        public int Trigger()
        {
            int damage = IsActive ? Utils.Random.Next(MinDamage, MaxDamage) : 0;
            IsActive = false;

            return damage;
        }
    }
}
