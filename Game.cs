// Better dice class? (inheritance + percentage)
// Acess modifiers
// Try/catch
// Code conventions: Refactor: Reshaper// Intellisense
// Intro tekst + metodenavne mm.
// Better texts, in game
// UI
// -- Center
// -- Clear on new round
// -- Possible scores == green?
// Clean up in properties / constructors
// method for try/catch?? <-- Pass in custom input if failed?
// writeline appropiate?
// smart cw, w. variable at last (newer string formating)
// tenary possibilites?
// arrays/list 
// members/lists
// abstract classes/methods
// Interface
// Enum
// namespaces

using System;
using System.Collections.Generic;
using System.Linq;

namespace YahtzyNEW {
    public class Game {
        public List<Player> players = new List<Player>();
        public int roundNumber = 0;
        public int AmountOfRolls = 3; // player klassen?

        // Get Amount of players and their names
        public void SetupGame()
        {
            Console.WriteLine("Hey, let's play some Yahtzy! \nType 'help' to see all avaliable commands\nHow many players?");
            try
            {
                int AmountOfPlayers = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < AmountOfPlayers; i++)
                {
                    Console.WriteLine("\nWhat is Player " + (i + 1) + "'s name?");
                    players.Add(new Player(Console.ReadLine()));
                }
                StartGame();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
                case "roll":
                    Score(player);
                    player.Roll();
                    player.OccurencesOfDice();
                    break;
                case "help":
                    Help();
                    break;
                case "score":
                    Score(player);
                    break;
                case "hold":
                    Hold(player);
                    break;
                case "exit":
                    Exit();
                    break;
                case "save":
                    Console.Write("Theese Are the dies you can save to, write the number of where you want to save your points eg. '1'\n");
                    if (player.UpperSection == true)
                    {
                        player.UpperSectionScores();
                    }
                    if (player.UpperSection == false)
                    {
                        LowerSectionScores(player);
                    }
                    break;
                case "release":
                    Release(player);
                    break;
                case "drop":
                    Drop(player);
                    break;
                case "bias":
                    Bias(player);
                    break;
                case "changerolls":
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
            try
            {
                Console.WriteLine("Type in the dies you want to hold in the format '1,2,3' where 1 marks the most left dice in the list");
                List<int> holdlist = Console.ReadLine().Split(',').Select(s => int.Parse(s)).ToList();
                foreach (var holddice in holdlist)
                {
                    player.dieList.ElementAt(holddice - 1).HoldState = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            Console.WriteLine("Change the bias of your dice: \n1: Lucky dice.\n0: Fair dice.\n-1: Unfair dice. ");
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
            AmountOfRolls = Convert.ToInt32(Console.ReadLine());
            player.PlayerRolls = AmountOfRolls; // also add rolls for current round
            Console.WriteLine("Players now have " + AmountOfRolls + " Rolls pr. turn");
        }

        // Display scoreboard (text box)
        public void Score(Player player)
        {
            Console.WriteLine("---------------------------");
            foreach (KeyValuePair<string, int?> kvp in player.scoreboard)
            {
                var results = $"{kvp.Key}, {kvp.Value}";
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

        // Check if scores are possible, and assign by userInput
        public void LowerSectionScores(Player player)
        {
            if (player.scoreboard.ContainsKey("One Pair") &&
                player.scoreboard["One Pair"] == null &&
                player.OnePair() >= 1)
            {
                Console.WriteLine("1. One pair");
            }
            if (player.scoreboard.ContainsKey("Two Pairs") &&
              player.scoreboard["Two Pairs"] == null &&
              player.TwoPairs() >= 1)
            {
                Console.WriteLine("2. Two Pairs");
            }
            if (player.scoreboard.ContainsKey("Three Of A Kind") &&
              player.scoreboard["Three Of A Kind"] == null &&
              player.ThreeOfAKind() >= 1)
            {
                Console.WriteLine("3. Three Of A Kind");
            }
            if (player.scoreboard.ContainsKey("Four Of A Kind") &&
              player.scoreboard["Four Of A Kind"] == null &&
              player.FourOfAKind() >= 1)
            {
                Console.WriteLine("4. Four Of A Kind");
            }
            if (player.scoreboard.ContainsKey("Full House") &&
              player.scoreboard["Full House"] == null &&
              player.FullHouse() >= 1)
            {
                Console.WriteLine("5. Full House");
            }
            if (player.scoreboard.ContainsKey("Small Straight") &&
              player.scoreboard["Small Straight"] == null &&
              player.SmallStraight() >= 1)
            {
                Console.WriteLine("6. Small Straight");
            }
            if (player.scoreboard.ContainsKey("Large Straight") &&
            player.scoreboard["Large Straight"] == null &&
            player.LargeStraight() >= 1)
            {
                Console.WriteLine("7. Large Straight");
            }
            if (player.scoreboard.ContainsKey("Chance") &&
            player.scoreboard["Chance"] == null &&
            player.Chance() >= 1)
            {
                Console.WriteLine("8. Chance");
            }
            if (player.scoreboard.ContainsKey("Yahtzy") &&
            player.scoreboard["Yahtzy"] == null &&
            player.Yahtzy() >= 1)
            {
                Console.WriteLine("9. Yahtzy");
            }
            switch (Console.ReadLine())
            {
                case "1":
                    player.OnePair(true);
                    player.PlayerTurn = false;
                    break;
                case "2":
                    player.TwoPairs(true);
                    player.PlayerTurn = false;
                    break;
                case "3":
                    player.ThreeOfAKind(true);
                    player.PlayerTurn = false;
                    break;
                case "4":
                    player.FourOfAKind(true);
                    player.PlayerTurn = false;
                    break;
                case "5":
                    player.FullHouse(true);
                    player.PlayerTurn = false;
                    break;
                case "6":
                    player.SmallStraight(true);
                    player.PlayerTurn = false;
                    break;
                case "7":
                    player.LargeStraight(true);
                    player.PlayerTurn = false;
                    break;
                case "8":
                    player.Chance(true);
                    player.PlayerTurn = false;
                    break;
                case "9":
                    player.Yahtzy(true);
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
                string scorename = item.Key;
                // if item == null (if hasn't been assigned)
                if (player.scoreboard[scorename] == null)
                {
                    // Write index + name 
                    Console.WriteLine(i + "." + scorename);
                    // Append to droppable 
                    droppablevalues.Add(i, scorename);
                }
            }
            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Manages when the game ends and prints a ranked scoreboard
        public bool GameEnd()
        {
            if (roundNumber == 15)
            {
                List<Player> RankedScores = players.OrderByDescending(player => player.TotalSum()).ToList();
                int Ranking = 1;
                foreach (Player player in RankedScores)
                {
                    Console.WriteLine(Ranking + "." + player + ": " + player.TotalSum() + "points"); // sort by sum
                    Ranking++;
                }
                return true;
            };
            return false;
        }

        // Exit game
        public static void Exit() => System.Environment.Exit(0);
    }
}
