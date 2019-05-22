using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    class Game
    {
        private const int MAZE_SIZE = 200;
        private const float CHANCE_FOR_ENEMY_SPAWN = 0.5f;
        private const float CHANCE_TO_FLEE = 0.9f;
        private const float CHANCE_FOR_ENEMY_MOVE = 0.7f;

        public static bool MinotaurIsAlive = true;
        public static bool DragonIsAlive = true;

        private Maze Maze;
        private Player Player;
        private int StartingDifficulty;
        private int Difficulty;
        private int MovesTaken;
        private BattleResult? BattleResult;
        private Dictionary<char, string> DirectionActions = new Dictionary<char, string>
        {
            { 'N', Direction.North.ToString() },
            { 'E', Direction.East.ToString() },
            { 'S', Direction.South.ToString() },
            { 'W', Direction.West.ToString() }
        };
        private Dictionary<char, string> ChestActions = new Dictionary<char, string>
        {
            { (char)ChestAction.Open, "Open the chest" },
            { (char)ChestAction.Leave, "Don't open the chest" },
            { (char)ChestAction.Examine, "Examine the chest" }
        };
        public static Dictionary<char, string> BattleActions = new Dictionary<char, string>
        {
            { (char)BattleAction.Attack, "Normal attack" },
            { (char)BattleAction.Bow, "Bow attack" },
            { (char)BattleAction.Potion, "Use potion" },
            { (char)BattleAction.Flee, "Attempt to flee" }
        };

        /// <summary>
        /// Constructs a <see cref="Game"/> object
        /// </summary>
        /// <param name="startingDifficulty">The starting difficulty of the game</param>
        public Game(int startingDifficulty)
        {
            Maze = new Maze(MAZE_SIZE);
            Player = new Player();
            StartingDifficulty = startingDifficulty;
            Difficulty = startingDifficulty;
            MovesTaken = 0;

            Player.ItemGained += new EventHandler<ItemGainedEventArgs>(OnItemGained);
            Player.StatsIncreased += new EventHandler<StatsIncreasedEventArgs>(OnStatsIncreased);
        }

        /// <summary>
        /// Begins the game
        /// </summary>
        public void Start()
        {
            // Place the player at a random starting location
            Player.Location = Utils.GetRandomFromList<Location>(Maze.Network.SelectMany(l => l).ToList().Where(l => l != null));

            DisplayMessage("You enter the Labyrinth.");

            // Main game loop
            while (true)
            {
                Dictionary<char, string> possibleDirections = DirectionActions
                    .Where(a => Player.Location.Neighbors[(int)Enum.Parse(typeof(Direction), a.Value)] != null)
                    .ToDictionary(a => a.Key, a => a.Value);

                // Player movement
                DisplayPrompt("Which direction?", possibleDirections);
                char dirChar = GetInput(possibleDirections.Keys.ToArray());
                Direction dir = (Direction)Enum.Parse(typeof(Direction), DirectionActions[dirChar]);
                Location newLocation = Player.Move(dir);

                // Spawn an enemy in the player's location
                if (newLocation.Enemy == null && Utils.Roll(CHANCE_FOR_ENEMY_SPAWN))
                {
                    newLocation.Enemy = Enemy.RandomEnemy(Difficulty);
                }

                // Room is trapped
                if (newLocation.IsTrapped)
                {
                    DisplayMessage("You feel a plate sink under your foot.");
                    DisplayMessage("Flames erupt from the floor!");
                    int trapDamage = Player.Location.Trap.Trigger();
                    Player.Damage(trapDamage);
                    DisplayMessage($"You take {trapDamage} points of damage.");
                }

                #region Found an enemy
                if (Player.Location.Enemy != null)
                {
                    BattleResult = Battle(Player, Player.Location.Enemy);

                    switch (BattleResult.Value)
                    {
                        case Labyrinth.BattleResult.Won:
                            DisplayMessage($"You defeat the {Player.Location.Enemy.EnemyType}!");
                            Player.GiveXP(Player.Location.Enemy.XP);

                            if (Player.Location.Enemy.EnemyType == EnemyType.Minotaur)
                                MinotaurIsAlive = false;

                            Player.Location.Enemy = null;
                            break;
                        case Labyrinth.BattleResult.Fled:
                            DisplayMessage("You ran away.");
                            break;
                        case Labyrinth.BattleResult.Lost:
                            GameOver();
                            return;
                    }
                }
                else
                {
                    BattleResult = null;
                }
#endregion

                #region Found a chest
                if (Player.Location.Chest != null)
                {
                    DisplayPrompt("You find a chest.", ChestActions);

                    char action;

                    do
                    {
                        action = GetInput(ChestActions.Keys.ToArray());

                        switch (action)
                        {
                            case (char)ChestAction.Examine:
                                if (Player.Location.Chest.StatusVisible)
                                {
                                    if (Player.Location.Chest.IsTrapped)
                                        DisplayMessage("The chest looks booby-trapped.");
                                    else
                                        DisplayMessage("It appears safe to open");
                                }
                                else
                                {
                                    DisplayMessage("You can't tell if the chest is safe to open.");
                                }
                                break;
                            case (char)ChestAction.Open:
                                if (Player.Location.Chest.IsTrapped)
                                {
                                    DisplayMessage("The chest was booby-trapped!");
                                    int trapDamage = Player.Location.Chest.Trap.Trigger();
                                    Player.Damage(trapDamage);
                                    DisplayMessage($"You take {trapDamage} points of damage.");
                                }

                                DisplayMessage("You open the chest.");
                                Player.GiveLoot(Player.Location.Chest.Items);
                                break;
                            default:
                                break;
                        }
                    }
                    while (action == (char)ChestAction.Examine);
                }
                #endregion

                if (Player.Location.Items.Any())
                {
                    DisplayMessage("There's something on the floor.");
                    Player.GiveLoot(Player.Location.Items);
                }

                MovesTaken++;

                UpdateDifficulty();
                
                // Meve enemies
                if (!(BattleResult != null && BattleResult.Value == Labyrinth.BattleResult.Fled)) // If the player fled, don't move the enemy
                {                                                                                   // TODO: This should probably only happen for the enemy that was battled, rather than all enemies
                    foreach (Enemy enemy in Maze.Network.SelectMany(r => r.Select(l => l.Enemy)).Where(e => e != null))
                    {
                        if (Utils.Roll(CHANCE_FOR_ENEMY_MOVE))
                        {
                            enemy.Move((Direction)Utils.Random.Next(3));
                        }
                    }
                }
            }

        }

        #region Battle Methods
        /// <summary>
        /// Begins a battle between the player and an enemy
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="enemy">The enemy to battle</param>
        /// <returns>The result of the battle</returns>
        public BattleResult Battle(Player player, Enemy enemy)
        {
            while (enemy.CurrentHP > 0 && player.CurrentHP > 0)
            {
                Dictionary<char, string> actions = ValidBattleActions();

                int uiOffset = DisplayBattleUI(player, enemy);
                uiOffset += DisplayPrompt("", actions);

                char action;
                bool validAction = true;
                int tempOffset = 0;
                do
                {
                    action = GetInput(actions.Keys.ToArray());

                    if (!validAction)
                    {
                        //ClearLines(tempOffset);
                        uiOffset -= tempOffset;
                    }

                    if (action == (char)BattleAction.Bow && !player.Items.Contains(ItemType.Arrows))
                    {
                        tempOffset += DisplayMessage("You don't have any arrows!", false);
                        validAction = false;
                    }
                    else if (action == (char)BattleAction.Potion && player.CurrentHP == player.MaxHP)
                    {
                        tempOffset += DisplayMessage("You're already at full HP!", false);
                        validAction = false;
                    }

                    uiOffset += tempOffset;
                } while (!validAction);

                switch (action)
                {
                    case (char)BattleAction.Attack:
                        uiOffset += ProcessPlayerAttack(player, enemy, false);
                        break;
                    case (char)BattleAction.Bow:
                        uiOffset += ProcessPlayerAttack(player, enemy, true);
                        break;
                    case (char)BattleAction.Potion:
                        uiOffset += DisplayMessage("You drink a potion.");
                        int healthRestored = player.UsePotion();
                        uiOffset += DisplayMessage($"Restored {healthRestored} HP.");
                        break;
                    case (char)BattleAction.Flee:
                        if (Utils.Roll(CHANCE_TO_FLEE))
                        {
                            //ClearLines(uiOffset);
                            return Labyrinth.BattleResult.Fled;
                        }
                        else
                        {
                            uiOffset += DisplayMessage("You couldn't get away!");
                        }
                        break;
                    default:
                        throw new Exception("Invalid battle action.");
                }

                if (enemy.CurrentHP > 0)
                {
                    uiOffset += ProcessEnemyAttack(enemy, player);
                }

                //ClearLines(uiOffset);
            }

            if (player.CurrentHP <= 0)
            {
                return Labyrinth.BattleResult.Lost;
            }
            else
            {
                return Labyrinth.BattleResult.Won;
            }
        }

        /// <summary>
        /// Displays the battle interface
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="enemy">The enemy that the player is battling</param>
        /// <param name="displayPositonAdjustment"></param>
        /// <returns>The number of lines written to the console</returns>
        private int DisplayBattleUI(Player player, Enemy enemy, int displayPositonAdjustment = -1)
        {
            //if (displayPositonAdjustment >= 0)
            //    ClearLines(displayPositonAdjustment);

            int linesWritten = 0;

            Console.WriteLine($"You {enemy.EnemyType,26}");
            linesWritten++;
            Console.WriteLine($"HP: {player.CurrentHP,2}/{player.MaxHP,-13} HP: {enemy.CurrentHP,2}/{enemy.MaxHP,-2}");
            linesWritten++;

            return linesWritten;
        }

        /// <summary>
        /// Returns the battle actions available to the player based on the items they have
        /// </summary>
        /// <returns>The valid actions available to the player</returns>
        private Dictionary<char, string> ValidBattleActions()
        {
            Dictionary<char, string> actions = new Dictionary<char, string>();

            foreach (var action in BattleActions)
            {
                switch (action.Key)
                {
                    case (char)BattleAction.Bow:
                        if (Player.Items.Contains(ItemType.Bow))
                            actions.Add(action.Key, action.Value);
                        break;
                    case (char)BattleAction.Potion:
                        if (Player.Items.Contains(ItemType.Potions))
                            actions.Add(action.Key, action.Value);
                        break;
                    default:
                        actions.Add(action.Key, action.Value);
                        break;
                }
            }

            return actions;
        }

        /// <summary>
        /// Processes the player's attack and displays the results
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="enemy">The enemy being attacked</param>
        /// <param name="attackWithBow">Whether the player is attacking with a bow</param>
        /// <returns>The number of lines written to the console</returns>
        public int ProcessPlayerAttack(Player player, Enemy enemy, bool attackWithBow)
        {
            string weaponName = attackWithBow ? "Bow" : player.Items.Contains(ItemType.Weapon) ? player.Items[ItemType.Weapon].ItemType.ToString() : "Fists";
            int offset = DisplayMessage($"You attack the {enemy.EnemyType} with your {weaponName}.");

            Tuple<AttackResult, int> attackResult = attackWithBow ? player.AttackWithBow(enemy) : player.Attack(enemy);
            if (attackResult.Item1 == AttackResult.Miss)
            {
                offset += DisplayMessage($"The {enemy.EnemyType} dodged your attack.");
            }
            else
            {
                if (attackResult.Item1 == AttackResult.Crit)
                    offset += DisplayMessage("Critical hit!");

                offset += DisplayMessage($"Dealt {attackResult.Item2} damage.");
            }

            return offset;
        }

        /// <summary>
        /// Processes the enemy's attack and displays the results
        /// </summary>
        /// <param name="enemy">The enemy attacking the player</param>
        /// <param name="player">The player</param>
        /// <returns>The number of lines writen to the console</returns>
        public int ProcessEnemyAttack(Enemy enemy, Player player)
        {
            int offset = DisplayMessage($"The {enemy.EnemyType} attacks!");
            Tuple<AttackResult, int> attackResult = enemy.Attack(player);
            
            if (attackResult.Item1 == AttackResult.Miss)
            {
                offset += DisplayMessage($"You dodge its attack.");
            }
            else
            {
                if (attackResult.Item1 == AttackResult.Crit)
                    offset += DisplayMessage("Critical hit!");

                offset += DisplayMessage($"You take {attackResult.Item2} damage.");
            }

            return offset;
        }
#endregion

        #region IO Methods
        /// <summary>
        /// Retrieves an input character based on a list of valid inputs
        /// </summary>
        /// <param name="validInputs">Uppercase characters reperesenting valid inputs</param>
        /// <returns>The uppercase character that the player entered</returns>
        public char GetInput(params char[] validInputs)
        {
            char input;

            if (validInputs == null || validInputs.Count() == 0)
            {
                input = Console.ReadKey().KeyChar;
            }
            else
            {
                do
                {
                    input = char.ToUpper(Console.ReadKey(true).KeyChar);
                }
                while (!validInputs.Contains(input));
            }

            return input;
        }

        /// <summary>
        /// Prints a message to the console
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="extraLine">Whether an extra line should be printed after the message</param>
        /// <returns>The number of lines written to the console</returns>
        public static int DisplayMessage(string message, bool extraLine = true)
        {
            Console.WriteLine(message);
            int linesWritten = 1;

            if (extraLine)
            {
                Console.WriteLine();
                linesWritten++;
            }
            return linesWritten;
        }

        /// <summary>
        /// Displays a message along with input options for the player
        /// </summary>
        /// <param name="prompt">The message to display</param>
        /// <param name="actions">The actions that the player can take</param>
        /// <returns>The number of lines written to the console</returns>
        public static int DisplayPrompt(string prompt, Dictionary<char, string> actions)
        {
            int linesWritten = 0;

            if (prompt != null)
            {
                Console.WriteLine(prompt);
                linesWritten++;
            }

            foreach (KeyValuePair<char, string> action in actions)
            {
                Console.WriteLine($"{action.Key}: {action.Value}");
                linesWritten++;
            }

            Console.WriteLine();
            return ++linesWritten;
        }

        //private void ClearLines(int numLines)
        //{
        //    Console.SetCursorPosition(0, Console.CursorTop);
        //    Console.Write(new string(' ', Console.BufferWidth));

        //    for (int i = 0; i < numLines; i++)
        //    {
        //        Console.SetCursorPosition(0, Console.CursorTop - 2);
        //        Console.Write(new string(' ', Console.BufferWidth));
        //    }

        //    Console.SetCursorPosition(0, Console.CursorTop - 1);
        //}
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles the <see cref="Player.ItemGained"/> event
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The arguments sent with the event</param>
        private void OnItemGained(object sender, ItemGainedEventArgs e)
        {
            string message = e.FirstItem ? "Found " : "Also found ";
            message += e.Item.Stackable ? e.Item.Count.ToString() : Utils.AnOrA(e.Item.ItemType.ToString(), false);
            message += " " + e.Item.ItemType.ToString();
            message += e.ItemKept ? '!' : '.';
            DisplayMessage(message);
        }

        /// <summary>
        /// Handles the <see cref="Player.StatsIncreased"/> event
        /// </summary>
        /// <param name="sender">The object that invoked the event</param>
        /// <param name="e">The arguments sent with the event</param>
        private void OnStatsIncreased (object sender, StatsIncreasedEventArgs e)
        {
            string message = "";

            foreach (Tuple<string, int> stat in e.StatsIncreased)
            {
                switch (stat.Item1)
                {
                    case Stats.XP:
                        message += $"Earned {stat.Item2} XP.";
                        break;
                    case Stats.Level:
                        message += $"Leveled up!\nReached level {stat.Item2}.";
                        break;
                    default:
                        message += $"{stat.Item1} increased by {stat.Item2}.";
                        break;
                }

                message += '\n';
            }

            DisplayMessage(message);
        }
        #endregion

        /// <summary>
        /// Updates the game difficulty based on the player's stats and the length of the game
        /// </summary>
        private void UpdateDifficulty()
        {
            Difficulty = StartingDifficulty + ((Player.Power + Player.Defense) / 2) + (MovesTaken / 10);
        }

        /// <summary>
        /// Displays a game over message
        /// </summary>
        private void GameOver()
        {
            DisplayMessage("Game over.");
        }
    }
}
