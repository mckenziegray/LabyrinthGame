using Microsoft.VisualStudio.TestTools.UnitTesting;
using Labyrinth;
using System;

namespace LabyrinthTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            int hp = Utils.Random.Next();
            int power = Utils.Random.Next();
            int defense = Utils.Random.Next();
            Unit unit = new Unit(hp, power, defense);

            Assert.AreEqual(hp, unit.MaxHP);
            Assert.AreEqual(hp, unit.CurrentHP);
            Assert.AreEqual(power, unit.Power);
            Assert.AreEqual(defense, unit.Defense);
        }

        [TestMethod]
        public void DamageHealTest()
        {
            int maxHp = Utils.Random.Next(3, TestUtils.MAX_VALUE - 1);
            int highAmt = Utils.Random.Next(maxHp + 1, TestUtils.MAX_VALUE);
            int lowDmg = Utils.Random.Next(2, maxHp - 1);
            int lowHeal = Utils.Random.Next(1, lowDmg - 1);
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
            int defense = Utils.Random.Next(0, TestUtils.MAX_VALUE);
            int power = Math.Clamp(Utils.Random.Next(defense + 1, TestUtils.MAX_VALUE), 1, TestUtils.MAX_VALUE);
            int damage = power - defense;
            int remainingHp = Utils.Random.Next(1, TestUtils.MAX_VALUE);
            int highHp = damage + remainingHp;
            int lowHp = Utils.Random.Next(1, damage);
            
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
            int dir = Utils.Random.Next(Enum.GetValues(typeof(Direction)).Length);
            startLoc.Neighbors[dir] = neighborLoc;

            Unit unit = new Unit
            {
                Location = startLoc
            };
            Assert.AreEqual(startLoc, unit.Location);

            unit.Move((Direction)dir);
            Assert.AreEqual(neighborLoc, unit.Location);
        }
    }
}
