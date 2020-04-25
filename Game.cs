/*Yahtzy 1.0
 * The following yahtzy game is build upon a modular approach, where the nesting of classes, should/would represent a real-world Yahtzy game, like so:
 * A die is given in the context of five other dice, which is held by a player that has an individual score, which exist in the context of other players
 * This is what makes up a yahtzy game. From this the following classes are created to describe it. 
 * 
 * Classes
 *  - Program: serves as the starting point that initializes the game.
 * 
 *  - Dice: holds the value of a dice, aswell as if the dice i 'held' from a previous rounds, additionaly this class also controls the bias of the dice,
 *    which can be altered thorughout the game.
 *  
 *   - Player: As this is a multiplayer game, the player has he's/her's own class, to manage each player, a collection of the players dice,
 *     the amount of rolls pr. round (which can be altered) the name of the player, aswell as if it's the players turn, additionally this class also contains
 *     the scoreboard within it aswell and methods for caluclating if score-assignment is possible, and assigning the score.
 *  
 *   - Game: The game class, can be seen as a view for the player, where commands and input is transformed into the 
 *     logic that exists in the other classes, this class also holds global values and methods, which is static in nature
 *     which is either true no matter what the player inputs, or if the value is not changed by the player throughout the game.
 *   
 * Assumptions: 
 *   - In the application the biased dice is made in one class, with three options of biaseness, as making an advance function for distribution
 *     in a yahtzy game seemed over the top.
 *  
 *  - The change of rolls method is applied to all players
 *  
 *  - The release method releases all dies
 *  
 *  - The nesting of scoreboard is not chosen as it's complicated??
 *  
 *  - Bias is applied to all users
 *   
 * Imported Libraries:
 *   - LINQ:
 *   
 *   // Iteration of dictionariy == bad
 *   
 *   
 *   Game rules
 *   https://en.wikipedia.org/wiki/Yatzy
 */


// Fix scores for good
// Make scoreboard class
// Optimize Possible + Add method for adding scores
// Try/catch implementation
// More clever dice
// access modifiers
// code conventions

// Ekstra
// Rebalancering af placering af metoder
// UI
// -- Center
// -- Clear on new round
// -- Possible scores == green
// -- Held dice == red
// straight metoder kan sammenskrives
// par metoder kan måske sammenskrives
// UI

using System;
using System.Collections.Generic;
using System.Linq;

namespace YahtzyNEW {
    public class Game {
        public List<Player> players = new List<Player>();
        public int roundNumber = 0;
        public int AmountOfRolls = 3;

        // Get Amount of players and their names
        public void SetupGame()
        {
            Console.WriteLine("Hey, let's play some Yahtzy! \nType 'help' to see all avaliable commands\nHow many players?");
            int AmountOfPlayers = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < AmountOfPlayers; i++)
            {
                Console.WriteLine("\nWhat is Player " + (i + 1) + "'s name?");
                players.Add(new Player(Console.ReadLine()));
            }
            StartGame();
        }

        // Run game rounds until conditions of finished game is fulfilled
        public void StartGame()
        {
            while (!GameEnd())
            {
                NewRound();
                roundNumber++;
            }
        }

        // Manages rounds and player turns, aswell as resetting for next round
        public void NewRound()
        {
            foreach (Player player in players) // A Round
            {
                Console.WriteLine("\n" + player + "'s turn");
                player.PlayerRolls = AmountOfRolls;
                player.CheckIfUpperSectionDone();
                Release(player);
                while (player.PlayerTurn)  // A Turn
                {
                    ReadLine(player);
                    Console.WriteLine("\n\n\n");
                }
                player.PlayerTurn = true;
            }
        }

        // Collection of player commands
        public void ReadLine(Player player = null)
        {
            switch (Console.ReadLine())
            {
                case ("roll"):
                    Score(player);
                    player.Roll();
                    player.OccurencesOfDice();
                    break;
                case ("help"):
                    Help();
                    break;
                case ("score"):
                    Score(player); // Could be in player class
                    break;
                case ("hold"):
                    Hold(player);
                    break;
                case ("exit"):
                    Exit();
                    break;
                case ("save"):
                    ScorePossibilities(player);  //should this be in player class?
                    break;
                case ("release"):
                    Release(player);
                    break;
                case ("drop"):
                    Drop(player);
                    break;
                case ("bias"):
                    Bias(player);
                    break;
                case ("changerolls"):
                    Amountofrolls(player);
                    break;
                default:
                    Console.WriteLine("Unavaliable command type 'help' to get a list of the avaliable commands");
                    break;
            }
        }

        // Pick dies to hold
        public void Hold(Player player)
        {
            Console.WriteLine("Type in the dies you want to hold in the format '1,2,3' where 1 marks the most left dice in the list");
            List<int> holdlist = Console.ReadLine().Split(',').Select(s => int.Parse(s)).ToList();
            foreach (var holddice in holdlist)
            {
                player.dieList.ElementAt(holddice - 1).HoldState = true;
            }
        }

        // Set holdsate for all dies to false (Possibility of releasing single dice?)
        public void Release(Player player)
        {
            foreach (Dice dice in player.dieList)
            {
                dice.HoldState = false;
            }
        }

        // Change bias of dice
        public void Bias(Player player)
        {
            Console.WriteLine("Change the bias of your dice:");
            Console.WriteLine("1: Lucky dice.");
            Console.WriteLine("0: Fair dice.");
            Console.WriteLine("-1: Unfair dice.");

            int newbias = Convert.ToInt32(Console.ReadLine());

            foreach (Dice dice in player.dieList)
            {
                dice.Bias = newbias;
            }
        }

        // Change amount of rolls for player
        public void Amountofrolls(Player player)
        {
            Console.WriteLine("Enter how many rolls you want pr. turn:");
            this.AmountOfRolls = Convert.ToInt32(Console.ReadLine());
            player.PlayerRolls = AmountOfRolls; // also add rolls for current round
            Console.WriteLine("Players now have " + AmountOfRolls + " Rolls pr. turn");
        }

        // Display scoreboard (text box)
        public void Score(Player player)
        {
            Console.WriteLine("---------------------------");
            foreach (KeyValuePair<string, int?> kvp in player.scoreboard)
            {
                var results = string.Format("{0}, {1}", kvp.Key, kvp.Value);
                Console.WriteLine(results);
            }
            Console.WriteLine("---------------------------");
            Console.WriteLine("Total score " + player.TotalSum());
            Console.WriteLine("---------------------------");
        }

        // Display command options
        public void Help()
        {
            Console.WriteLine("\n Hey, looks like you're perhaps having some trouble Here's a series of commands that can be used at all times during the gameplay \n  'help' to get this message shown \n 'score' to display the scoreboard the current score \n 'options' to view the current possibilities for adding points \n 'add' to add points from your current rolls \n 'roll' to roll all dies which hasn't been marked with as 'hold' \n 'hold' to select dies you want to keep in the next roll \n 'exit' to exit the game completely\n ");
        }

        // Checks if no previos score exist, and if the dicehand fulfills criteria for score possibility
        public void ScorePossibilities(Player player)
        {
            Console.Write("Theese Are the dies you can save to, write the number of where you want to save your points eg. '1'\n");

            if (player.scoreboard["Ones"] == null && player.Ones(false) >= 1)
            {
                Console.WriteLine("1. Ones");
            }
            if (player.scoreboard["Twos"] == null && player.Twos(false) >= 1)
            {
                Console.WriteLine("2. Twos");
            }
            if (player.scoreboard["Threes"] == null && player.Threes(false) >= 1)
            {
                Console.WriteLine("3. Threes");
            }
            if (player.scoreboard["Fours"] == null && player.Fours(false) >= 1)
            {
                Console.WriteLine("4. Fours");
            }
            if (player.scoreboard["Fives"] == null && player.Fives(false) >= 1)
            {
                Console.WriteLine("5. Fives");
            }
            if (player.scoreboard["Sixes"] == null && player.Sixes(false) >= 1)
            {
                Console.WriteLine("6. Sixes");
            }
            if (player.scoreboard.ContainsKey("One Pair") &&
                player.scoreboard["One Pair"] == null &&
                player.OnePair(false) >= 1)
            {
                Console.WriteLine("7. One pair");
            }
            if (player.scoreboard.ContainsKey("Two Pairs") &&
              player.scoreboard["Two Pairs"] == null &&
              player.TwoPairs(false) >= 1)
            {
                Console.WriteLine("8. Two Pairs");
            }
            if (player.scoreboard.ContainsKey("Three Of A Kind") &&
              player.scoreboard["Three Of A Kind"] == null &&
              player.ThreeOfAKind(false) >= 1)
            {
                Console.WriteLine("9. Three Of A Kind");
            }
            if (player.scoreboard.ContainsKey("Four Of A Kind") &&
              player.scoreboard["Four Of A Kind"] == null &&
              player.FourOfAKind(false) >= 1)
            {
                Console.WriteLine("10. Four Of A Kind");
            }
            if (player.scoreboard.ContainsKey("Full House") &&
              player.scoreboard["Full House"] == null &&
              player.FullHouse(false) >= 1)
            {
                Console.WriteLine("11. Full House");
            }
            if (player.scoreboard.ContainsKey("Small Straight") &&
              player.scoreboard["Small Straight"] == null &&
              player.SmallStraight(false) >= 1)
            {
                Console.WriteLine("12. Small Straight");
            }
            if (player.scoreboard.ContainsKey("Large Straight") &&
            player.scoreboard["Large Straight"] == null &&
            player.LargeStraight(false) >= 1)
            {
                Console.WriteLine("13. Large Straight");
            }
            if (player.scoreboard.ContainsKey("Chance") &&
            player.scoreboard["Chance"] == null &&
            player.Chance(false) >= 1)
            {
                Console.WriteLine("14. Chance");
            }
            if (player.scoreboard.ContainsKey("Yahtzy") &&
            player.scoreboard["Yahtzy"] == null &&
            player.Yahtzy(false) >= 1)
            {
                Console.WriteLine("15. Yahtzy");
            }
            SaveScore(player);
        }

        // Save the score selected by user
        public void SaveScore(Player player) // could pass if possible from other method
        {
            switch (Console.ReadLine())
            {
                case ("1"):
                    player.Ones(true);
                    break;
                case ("2"):
                    player.Twos(true);
                    break;
                case ("3"):
                    player.Threes(true);
                    break;
                case ("4"):
                    player.Fours(true);
                    break;
                case ("5"):
                    player.Fives(true);
                    break;
                case ("6"):
                    player.Sixes(true);
                    break;
                case ("7"):
                    player.OnePair(true);
                    break;
                case ("8"):
                    player.TwoPairs(true);
                    break;
                case ("9"):
                    player.ThreeOfAKind(true);
                    break;
                case ("10"):
                    player.FourOfAKind(true);
                    break;
                case ("11"):
                    player.FullHouse(true);
                    break;
                case ("12"):
                    player.SmallStraight(true);
                    break;
                case ("13"):
                    player.LargeStraight(true);
                    break;
                case ("14"):
                    player.Chance(true);
                    break;
                case ("15"):
                    player.Yahtzy(true);
                    break;
                default:
                    Console.WriteLine("Doesn't look like you have the right dies, either 'release' 'roll' or 'drop' to select a score to skip"); // redirect user so he can either release or cancel die (se it to 0)
                    break;
            }
        }

        // Show scores that the player can drop, after entering "drop"
        public void Drop(Player player)
        {
            Console.WriteLine("What Column do you want to drop? \ntype the number you want to drop");

            // List that will contain values that can be dropped, based on below criteria
            Dictionary<int, string> droppablevalues = new Dictionary<int, string>();

            // Print all values that can be dropped
            for (int i = player.scoreboard.Count - 1; i >= 0; i--)
            {
                var item = player.scoreboard.ElementAt(i);
                var scorename = item.Key;

                // if item == null (if hasn't been assigned)
                if (player.scoreboard[scorename] == null)
                {
                    // Write index + name 
                    Console.WriteLine(i + "." + scorename);
                    // Append to droppable 
                    droppablevalues.Add(i, scorename);
                }
            }

            int input = Convert.ToInt32(Console.ReadLine());

            // if input matches index of list
            if (droppablevalues.ContainsKey(input))
            {
                // ise input to get value, which corresponds to the score name of input
                string nameofscore = droppablevalues[input];
                // set scoreboard to 0, from the key to the scoreboard which we just got
                player.scoreboard[nameofscore] = 0;
                player.PlayerTurn = false;
            }
        }

        // Ends game after 13 rounds and ranks player scores
        public bool GameEnd()
        {
            if (roundNumber == 15)
            {
                List<Player> RankedScores = players.OrderByDescending(player => player.TotalSum()).ToList();
                int Ranking = 1;
                foreach (Player player in RankedScores)
                {
                    Console.WriteLine((Ranking) + "." + player + ": " + player.TotalSum() + "points"); // sort by sum
                    Ranking++;
                }
                return true;
            };
            return false;
        }

        // Exit game
        public static void Exit()
        {
            System.Environment.Exit(0);
        }
    }
}
