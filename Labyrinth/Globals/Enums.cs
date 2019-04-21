namespace Labyrinth
{
    enum Direction { North, East, South, West }

    enum ItemType { Weapon, Armor, Shield, Bow, Arrows, Gold, Potions }

    enum WeaponType { Dagger, Sword, Axe }

    enum ArmorType { Leather, Chain, Iron }

    enum ShieldType { Wood, Kite }

    enum EnemyType { Rat, Spider, Wolf, Ghoul, Goblin, Vagrant, Specter, Ogre, Minotaur, Dragon }

    enum ChestAction { Open = 'O', Leave = 'D', Examine = 'E' }

    enum BattleAction { Attack = 'A', Bow = 'B', Potion = 'P', Flee= 'F' }

    enum AttackResult { Miss, Hit, Crit }

    enum BattleResult { Won, Fled, Lost }

    public class Stats { public const string XP = "XP", Level = "level", MaxHP = "max HP", Power = "power", Defense = "defense"; } // Not an enum, but it sort of acts like one
}
