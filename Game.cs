/*Yahtzee 1.0
 * The following yahtzee game is build upon a modular approach, where the nesting of classes, should/would represent a real-world Yahtzee game, like so:
 * A die is given in the context of five other dice, which is held by a player that has an individual score, which exist in the context of other players
 * This is what makes up a Yahtzee game. From this the following classes are created to describe it. 
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
 *     in a yahtzee game seemed over the top.
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
 * Notes:
 *   - It should be noticed that this programs development was influenced by stackoverflow.
 */

// Make on Exception that checks input for validness

using System;
using System.Collections.Generic;
using System.Linq; // Enumerable method

namespace YahtzyNEW {
    public class Game {
        // Set the stage with players, where each individual player has a scoreboard and a list of dies wich contains the individual dies.
        public List<Player> players = new List<Player>();
        public int roundNumber = 0;
        public int AmountOfRolls = 3;

        // Get Amount of players and their names
        public void SetupGame()
        {
            Console.WriteLine("Hey, let's play some Yahtzee! \nType 'help' to see all avaliable commands\nHow many players?");
            int AmountOfPlayers = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < AmountOfPlayers; i++)
            {
                Console.WriteLine("\nWhat is Player " + (i + 1) + "'s name?");
                players.Add(new Player(Console.ReadLine()));
            }
            StartGame();
        }

        // Runs game rounds until conditions of finnished game is fulfilled
        public void StartGame()
        {
            while (!GameEnd())
            {
                NewRound();
                roundNumber++;
            }
        }

        // Manages rounds and player turns, aswell as resetting for next rounds
        public void NewRound()
        {
            foreach (Player player in players)
            {
                Console.WriteLine("\n" + player + "'s turn");
                player.PlayerRolls = AmountOfRolls;
                player.CheckIfUpperSectionDone();
                while (player.PlayerTurn)
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
                    player.OnePair();
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
                player.dieList.ElementAt(holddice -1).HoldState = true;
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
            Console.WriteLine("Change bias of your dice:");
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
            Console.WriteLine("Players now have " + AmountOfRolls + " Rolls pr. turn" );
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
            // would be cool if this was integrated in print
            // print play.scoreboard["aces"] == null && player.Aces >=! print green)
            Console.Write("Theese Are the dies you can save to, write the number of where you want to save your points eg. '1'\n");
            if (player.scoreboard["Aces"] == null && player.UpperSectionscores(1) >= 1)
            {
                Console.WriteLine("1. Aces");
            }
            if (player.scoreboard["Twos"] == null && player.UpperSectionscores(2) >= 1)
            {
                Console.WriteLine("2. Twos");
            }
            if (player.scoreboard["Threes"] == null && player.UpperSectionscores(3) >= 1)
            {
                Console.WriteLine("3. Threes");
            }
            if (player.scoreboard["Fours"] == null && player.UpperSectionscores(4) >= 1)
            {
                Console.WriteLine("4. Fours");
            }
            if (player.scoreboard["Fives"] == null && player.UpperSectionscores(5) >= 1)
            {
                Console.WriteLine("5. Fives");
            }
            if (player.scoreboard["Sixes"] == null && player.UpperSectionscores(6) >= 1)
            {
                Console.WriteLine("6. Sixes");
            }
            if (player.scoreboard["Chance"] == null)
            {
                Console.WriteLine("7. Chance");
            }
            if (player.scoreboard["Yahtzee"] == null && player.Yahtzy() >= 1) 
            {
                Console.WriteLine("8. Yahtzy");
            }
            if (player.scoreboard["One Pair"] == null && player.OnePair() >= 1)
            {
                Console.WriteLine("9. One pair");
            }
            if (player.scoreboard["Two Pairs"] == null && player.TwoPairs() >= 1)
            {
                Console.WriteLine("10. Two Pairs");
            }
            if (player.scoreboard["Three Of A Kind"] == null && player.ThreeOfAKind() >= 1) 
            {
                Console.WriteLine("11. Three Of A Kind");
            }
            if (player.scoreboard["Four Of A Kind"] == null && player.FourOfAKind() >= 1)
            {
                Console.WriteLine("12. Four Of A Kind");
            }
            if (player.scoreboard["Full House"] == null && player.FullHouse() >= 1) 
            {
                Console.WriteLine("13. Full House");
            }
            if (player.scoreboard["Small Straight"] == null && player.SmallStraight() >= 1) // Always shown displays 1
            {
                Console.WriteLine("14. Small Straight");
            }
            if (player.scoreboard["Large Straight"] == null && player.LargeStraight() >= 1)  // Always shown displays 1
            {
                Console.WriteLine("15. Large Straight");
            }
            SaveScore(player);
        }

        // Save the score selected by user
        public void SaveScore(Player player)
        {
            switch (Console.ReadLine())
            {
                // Would be awesome if we could make this a loop, tuple dict? 1 = index, aces == key aces in method could be key aswell?
                // Or put together with above if-statements
                case ("1"):
                    player.scoreboard["Aces"] = player.UpperSectionscores(1);
                    player.PlayerTurn = false;
                    break;
                case ("2"):
                    player.scoreboard["Twos"] = player.UpperSectionscores(2);
                    player.PlayerTurn = false;
                    break;
                case ("3"):
                    player.scoreboard["Threes"] = player.UpperSectionscores(3);
                    player.PlayerTurn = false;
                    break;
                case ("4"):
                    player.scoreboard["Fours"] = player.UpperSectionscores(4);
                    player.PlayerTurn = false;
                    break;
                case ("5"):
                    player.scoreboard["Fives"] = player.UpperSectionscores(5);
                    player.PlayerTurn = false;
                    break;
                case ("6"):
                    player.scoreboard["Sixes"] = player.UpperSectionscores(6);
                    player.PlayerTurn = false;
                    break;
                case ("7"):
                    player.scoreboard["Chance"] = player.Chance();
                    player.PlayerTurn = false;
                    break;
                case ("8"):
                    player.scoreboard["Yahtzee"] = player.Yahtzy();
                    player.PlayerTurn = false;
                    break;

                // Uppersection Doesn't get added for some weird reason
                case ("9"):
                    player.scoreboard["One Pair"] = player.OnePair();
                    player.PlayerTurn = false;
                    break;
                case ("10"):
                    player.scoreboard["Two Pairs"] = player.TwoPairs();
                    player.PlayerTurn = false;
                    break;
                case ("11"):
                    player.scoreboard["Three Of A Kind"] = player.ThreeOfAKind();
                    player.PlayerTurn = false;
                    break;
                case ("12"):
                    player.scoreboard["Four Of A Kind"] = player.FourOfAKind();
                    player.PlayerTurn = false;
                    break;
                case ("13"):
                    player.scoreboard["Full House"] = player.FullHouse();
                    player.PlayerTurn = false;
                    break;
                case ("14"):
                    player.scoreboard["Small Straight"] = player.SmallStraight();
                    player.PlayerTurn = false;
                    break;
                case ("15"):
                    player.scoreboard["Large Straight"] = player.LargeStraight();
                    player.PlayerTurn = false;
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
                var itemKey = item.Key; // can maybe be removed, or renamed for clarity

                // if item == null (if hasn't been assigned)
                if (player.scoreboard[itemKey] == null)
                {
                    // Write index + name
                    Console.WriteLine(i + "." + itemKey);
                    // Append to droppable 
                    droppablevalues.Add(i, itemKey);
                }
            }

            int input = Convert.ToInt32(Console.ReadLine());
            // if dictionary of droppable values contains input
            if (droppablevalues.ContainsKey(input))
            {
                //droppables values key indexet, som der tastes nummer ind på.
                // herfra fås navnet på scoren
                string nameofscore = droppablevalues[input];
                // name of score == key i player.scoreboard
                player.scoreboard[nameofscore] = 0;
                player.PlayerTurn = false;
            }
        }


        // Ends game after 13 rounds and ranks player scores
        public bool GameEnd()
        {
            if (roundNumber == 13) {
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
