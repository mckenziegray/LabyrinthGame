using Microsoft.VisualStudio.TestTools.UnitTesting;
using Labyrinth;
using System;

namespace LabyrinthTest
{
    [TestClass]
    public class UnitTest
    {
        private Random random = new Random();
        private int maxValue = 10000; // NOTE: Don't use int.MaxValue

        [TestMethod]
        public void ConstructorTest()
        {
            int hp = random.Next();
            int power = random.Next();
            int defense = random.Next();
            Unit unit = new Unit(hp, power, defense);

            Assert.AreEqual(hp, unit.MaxHP);
            Assert.AreEqual(hp, unit.CurrentHP);
            Assert.AreEqual(power, unit.Power);
            Assert.AreEqual(defense, unit.Defense);
        }

        [TestMethod]
        public void DamageHealTest()
        {
            int maxHp = random.Next(3, maxValue - 1);
            int highAmt = random.Next(maxHp + 1, maxValue);
            int lowDmg = random.Next(2, maxHp - 1);
            int lowHeal = random.Next(1, lowDmg - 1);
            Unit unit = new Unit(maxHp, 0, 0);

            unit.Damage(lowDmg);
            Assert.AreEqual(maxHp - lowDmg, unit.CurrentHP);

            unit.Heal(lowHeal);
            Assert.AreEqual(maxHp - lowDmg + lowHeal, unit.CurrentHP);

            unit.Heal(highAmt);
            Assert.AreEqual(maxHp, unit.CurrentHP);

            unit.Damage(highAmt);
            Assert.AreEqual(0, unit.CurrentHP);
        }

        [TestMethod]
        public void AttackTest()
        {
            int defense = random.Next(0, maxValue);
            int power = Math.Clamp(random.Next(defense + 1, maxValue), 1, maxValue);
            int damage = power - defense;
            int remainingHp = random.Next(1, maxValue);
            int highHp = damage + remainingHp;
            int lowHp = random.Next(1, damage);
            
            Unit attackingUnit = new Unit(1, power, 0);
            Unit weakenedUnit = new Unit(highHp, 0, defense);
            Unit defeatedUnit = new Unit(lowHp, 0, defense);

            attackingUnit.Attack(weakenedUnit);
            attackingUnit.Attack(defeatedUnit);

            Assert.AreEqual(remainingHp, weakenedUnit.CurrentHP);
            Assert.AreEqual(0, defeatedUnit.CurrentHP);
        }

        [TestMethod]
        public void MoveTest()
        {
            Location startLoc = new Location();
            Location neighborLoc = new Location();
            int dir = random.Next(Enum.GetValues(typeof(Direction)).Length);
            startLoc.Neighbors[dir] = neighborLoc;

            Unit unit = new Unit();
            unit.Location = startLoc;
            Assert.AreEqual(startLoc, unit.Location);

            unit.Move((Direction)dir);
            Assert.AreEqual(neighborLoc, unit.Location);
        }
    }
}
