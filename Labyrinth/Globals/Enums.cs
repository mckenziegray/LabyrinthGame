namespace Labyrinth
{
    public enum Direction { North, East, South, West }

    public enum ItemType { Weapon, Armor, Shield, Bow, Arrows, Gold, Potions }

    public enum WeaponType { Dagger, Sword, Axe }

    public enum ArmorType { Leather, Chain, Iron }

    public enum ShieldType { Wood, Kite }

    public enum EnemyType { Rat, Spider, Wolf, Ghoul, Goblin, Vagrant, Specter, Ogre, Minotaur, Dragon }

    public enum ChestAction { Open = 'O', Leave = 'D', Examine = 'E' }

    public enum BattleAction { Attack = 'A', Bow = 'B', Potion = 'P', Flee= 'F' }

    public enum MerchantAction { Nothing }

    public enum AttackResult { Miss, Hit, Crit }

    public enum BattleResult { Won, Fled, Lost }

    public class Stats { public const string XP = "XP", Level = "level", MaxHP = "max HP", Power = "power", Defense = "defense"; } // Not an enum, but it sort of acts like one
}
