using System;
using System.Collections.Generic;

namespace Labyrinth
{
    public class Unit
    {
        protected const float CHANCE_FOR_CRIT = 0.1f;
        protected const float CHANCE_FOR_MISS = 0.1f;
        protected const int CRIT_MULTIPLIER = 3;

        public int MaxHP { get; protected set; }
        public int CurrentHP { get; protected set; }
        public int Power { get; protected set; }
        public int Defense { get; protected set; }
        public ItemList Items { get; protected set; }
        public int XP { get; protected set; }
        public virtual Location Location { get; set; }

        /// <summary>
        /// Constructs a <see cref="Unit"/> with default stats
        /// </summary>
        public Unit()
        {
            Items = new ItemList();
            MaxHP = 1;
            CurrentHP = MaxHP;
            Power = 0;
            Defense = 0;
            XP = 0;
        }

        /// <summary>
        /// Constructs a <see cref="Unit"/> with the given stats
        /// </summary>
        /// <param name="maxHP">The unit's HP stat</param>
        /// <param name="power">The unit's power stat</param>
        /// <param name="defense">The unit's defense stat</param>
        public Unit(int maxHP, int power, int defense)
        {
            Items = new ItemList();
            MaxHP = maxHP;
            CurrentHP = MaxHP;
            Power = power;
            Defense = defense;
        }

        /// <summary>
        /// Reduces the unit's HP by the given amount
        /// </summary>
        /// <param name="amount">The amount to damage the unit</param>
        /// <returns>The amount of HP the unit lost</returns>
        public int Damage(int amount)
        {
            int startingHP = CurrentHP;
            CurrentHP = Math.Clamp(CurrentHP - amount, 0, MaxHP);
            return startingHP - CurrentHP;
        }

        /// <summary>
        /// Increases the unit's HP by the given amount
        /// </summary>
        /// <param name="amount">The amount to heal the unit</param>
        /// <returns>The amount of HP the unit recovered</returns>
        public int Heal(int amount)
        {
            int startingHP = CurrentHP;
            CurrentHP = Math.Clamp(CurrentHP + amount, 0, MaxHP);
            return CurrentHP - startingHP;
        }

        /// <summary>
        /// Moves the unit in the given direction
        /// </summary>
        /// <param name="dir">The direction to move</param>
        /// <returns>The unit's new location</returns>
        public Location Move(Direction dir)
        {
            this.Location = this.Location.GetNeighbor(dir);
            return this.Location;
        }

        /// <summary>
        /// Randomly determines how much damage an attack will deal
        /// </summary>
        /// <returns>A tuple containing the result of the attack and the amount of damage it does</returns>
        protected Tuple<AttackResult, int> RollDamage()
        {
            Tuple<AttackResult, int> result;

            if (Utils.Roll(CHANCE_FOR_MISS))
            {
                result = new Tuple<AttackResult, int>(AttackResult.Miss, 0);
            }
            else
            {
                if (Utils.Roll(CHANCE_FOR_CRIT))
                {
                    result = new Tuple<AttackResult, int>(AttackResult.Crit, Power * CRIT_MULTIPLIER);
                }
                else
                {
                    result = new Tuple<AttackResult, int>(AttackResult.Hit, Power);
                }
            }

            return result;
        }

        /// <summary>
        /// Causes this unit to attack another unit
        /// </summary>
        /// <param name="other">The unit to attack</param>
        /// <returns>A tuple containing the result of the attack and the amount of damage it does</returns>
        public Tuple<AttackResult, int> Attack(Unit other)
        {
            Tuple<AttackResult, int> result = RollDamage();
            other.Damage(result.Item2 - other.Defense);
            return result;
        }
    }
}
