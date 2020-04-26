
// BiasInheritance
// IntroTekst
// Bedre tekst + bedre navne + intronavne
// OP methods for pairs ++
// UI
// -- Center
// -- Spaces
// -- Clearing console
// methods ++ ?


using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzy {
    public class Game {
        private int RoundNumber { get; set; } = 0;
        private int AmountOfRolls { get; set; } = 3;
        private List<Player> Players { get; } = new List<Player>();

        // Get Amount of players and their names
        internal void SetupGame()
        {
            UtilityClass.YellowText("Hey, let's play some Yahtzy! \nType 'help' to see all available commands\nHow many players?\n");
            try
            {
                var amountOfPlayers = Convert.ToInt32(Console.ReadLine());
                for (var i = 0; i < amountOfPlayers; i++)
                {
                    UtilityClass.GreenText($"\nWhat is Player { i + 1}'s name?\n");
                    Players.Add(new Player(Console.ReadLine()));
                }
                StartGame();
            }
            catch (Exception e)
            {
                UtilityClass.RedText(e.Message);
            }
        }

        // Run game rounds until conditions of finished game is fulfilled
        private void StartGame()
        {
            while (!GameEnd())
            {
                NewRound();
                RoundNumber++;
            }
        }

        // Manages rounds and player turns, as well as resetting for next round
        private void NewRound()
        {
            foreach (var player in Players) // A Round
            {
                UtilityClass.YellowText($"\n{player}'s turn\n");
                player.PlayerRolls = AmountOfRolls;
                player.CheckIfUpperSectionDone();
                Release(player);
                while (player.PlayerTurn)  // A Turn
                {
                    ReadLine(player);
                }
                player.PlayerTurn = true;
            }
        }

        // Collection of player commands
        private void ReadLine(Player player = null)
        {
            switch (Console.ReadLine())
            {
                case "roll":
                    Console.Clear();
                    Score(player);
                    player.Roll();
                    player.OccurrencesOfDice();
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
                    UtilityClass.YellowText("These Are the dies you can save to, write the number of where you want to save your points eg. '1'\n");
                    if (player.UpperSection)
                    {
                        UpperSectionScores(player);
                    }
                    else
                    {
                        LowerSectionScores(player);
                    }
                    break;
                case "release":
                    Release(player);
                    break;
                case "drop":
                    DropScore(player);
                    break;
                case "bias":
                    Bias(player);
                    break;
                case "more":
                    ChangeAmountOfRolls(player);
                    break;
                default:
                    UtilityClass.RedText("Unavailable command type 'help' to get a list of the available commands\n");
                    break;
            }
        }

        // Pick dies to hold
        private static void Hold(Player player)
        {
            try
            {
                UtilityClass.YellowText("Type in the dies you want to hold in the format '1,2,3' where 1 marks the most left dice in the list\n");
                var holdList = Console.ReadLine().Split(',').Select(int.Parse).ToList();
                foreach (var holdDice in holdList)
                {
                    player.DieList.ElementAt(holdDice - 1).HoldState = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Set hold state for all dies to false (Possibility of releasing single dice?)
        private static void Release(Player player)
        {
            foreach (var dice in player.DieList)
            {
                dice.HoldState = false;
            }
        }

        // Change bias of dice
        private static void Bias(Player player)
        {
            UtilityClass.YellowText("Change the bias of your dice: \n1: Lucky dice.\n0: Fair dice.\n-1: Unfair dice.\n");
            var bias = Convert.ToInt32(Console.ReadLine());
            var weight = 1;
            if (bias != 0)
            {
                UtilityClass.YellowText("Great! How biased do you want to be? It gets pretty exciting at +10\n");
                weight = Convert.ToInt32(Console.ReadLine());
            }
            foreach (var dice in player.DieList)
            {
                dice.Bias = bias;
                dice.BiasWeight = weight;
            }
        }

        // Change amount of rolls for player
        private void ChangeAmountOfRolls(Player player)
        {
            UtilityClass.YellowText("Enter how many rolls you want pr. turn:\n");
            AmountOfRolls = Convert.ToInt32(Console.ReadLine());
            player.PlayerRolls = AmountOfRolls;
            UtilityClass.GreenText($"Players now have {AmountOfRolls} Rolls pr. turn\n");
        }

        // Display scoreboard
        private static void Score(Player player)
        {
            Console.WriteLine("---------------------------");
            foreach (var (key, value) in player.Scoreboard)
            {
                Console.WriteLine($"{key}, {value}");
            }
            Console.WriteLine("---------------------------");
            Console.WriteLine($"Total score: {player.TotalSum()}");
            Console.WriteLine("---------------------------");
        }

        // Display command options
        private static void Help()
        {
            UtilityClass.YellowText("\nHey, looks like you're perhaps having some trouble " +
                "\nHere's a series of commands that can be used at all times during the game " +
                "\n 'help' to get this message shown " +
                "\n 'score' to display the scoreboard the current score " +
                "\n 'options' to view the current possibilities for adding points " +
                "\n 'add' to add points from your current rolls " +
                "\n 'roll' to roll all dies which hasn't been marked with as 'hold' " +
                "\n 'hold' to select dies you want to keep in the next roll " +
                "\n 'exit' to exit the game completely\n ");
        }

        // Check if Upper section scores are possible, and assign by userInput
        private static void UpperSectionScores(Player player)
        {
            for (var i = 0; i < player.Scoreboard.Count; i++) // Check if possible
            {
                var (itemKey, itemValue) = player.Scoreboard.ElementAt(i);
                if (player.Scoreboard[itemKey] == null && (player.DieList.Any(x => x.DiceValue.Equals(i + 1))))
                {
                    UtilityClass.GreenText($"{i + 1}. {itemKey}{itemValue}\n");
                }
            }
            try
            {
                var input = Convert.ToInt32(Console.ReadLine()); // Assign based on input
                for (var i = 0; i < player.Scoreboard.Count; i++)
                {
                    var (itemKey, itemValue) = player.Scoreboard.ElementAt(i);
                    if (input != (i + 1) || (!player.DieList.Any(x => x.DiceValue.Equals(i + 1)))) continue;
                    {
                        player.Scoreboard[itemKey] = player.DieList.Count(x => x.DiceValue.Equals(i + 1)) * (i + 1);
                        player.PlayerTurn = false;
                        Console.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                UtilityClass.RedText(e.Message);
            }
        }

        // Check if scores are possible, and assign by userInput
        private static void LowerSectionScores(Player player)
        {
            if (player.Scoreboard.ContainsKey("One Pair") &&
                player.Scoreboard["One Pair"] == null &&
                player.OnePair() >= 1)
            {
                UtilityClass.GreenText("1. One pair\n");
            }
            if (player.Scoreboard.ContainsKey("Two Pairs") &&
              player.Scoreboard["Two Pairs"] == null &&
              player.TwoPairs() >= 1)
            {
                UtilityClass.GreenText("2. Two Pairs\n");
            }
            if (player.Scoreboard.ContainsKey("Three Of A Kind") &&
              player.Scoreboard["Three Of A Kind"] == null &&
              player.ThreeOfAKind() >= 1)
            {
                UtilityClass.GreenText("3. Three Of A Kind\n");
            }
            if (player.Scoreboard.ContainsKey("Four Of A Kind") &&
              player.Scoreboard["Four Of A Kind"] == null &&
              player.FourOfAKind() >= 1)
            {
                UtilityClass.GreenText("4. Four Of A Kind\n");
            }
            if (player.Scoreboard.ContainsKey("Full House") &&
              player.Scoreboard["Full House"] == null &&
              player.FullHouse() >= 1)
            {
                UtilityClass.GreenText("5. Full House\n");
            }
            if (player.Scoreboard.ContainsKey("Small Straight") &&
              player.Scoreboard["Small Straight"] == null &&
              player.SmallStraight() >= 1)
            {
                UtilityClass.GreenText("6. Small Straight\n");
            }
            if (player.Scoreboard.ContainsKey("Large Straight") &&
            player.Scoreboard["Large Straight"] == null &&
            player.LargeStraight() >= 1)
            {
                UtilityClass.GreenText("7. Large Straight\n");
            }
            if (player.Scoreboard.ContainsKey("Chance") &&
            player.Scoreboard["Chance"] == null &&
            player.Chance() >= 1)
            {
                UtilityClass.GreenText("8. Chance\n");
            }
            if (player.Scoreboard.ContainsKey("Yahtzy") &&
            player.Scoreboard["Yahtzy"] == null &&
            player.Yahtzy() >= 1)
            {
                UtilityClass.GreenText("9. Yahtzy\n");
            }

            switch (Console.ReadLine())
            {
                case "1":
                    player.OnePair(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "2":
                    player.TwoPairs(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "3":
                    player.ThreeOfAKind(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "4":
                    player.FourOfAKind(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "5":
                    player.FullHouse(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "6":
                    player.SmallStraight(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "7":
                    player.LargeStraight(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "8":
                    player.Chance(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                case "9":
                    player.Yahtzy(true);
                    player.PlayerTurn = false;
                    Console.Clear();
                    break;
                default:
                    UtilityClass.RedText("Doesn't look like you have the right dies, either 'release' 'roll' or 'drop' to select a score to skip\n"); // redirect user so he can either release or cancel die (se it to 0)
                    break;
            }
        }

        // Show scores that the player can drop, after entering "drop"
        private static void DropScore(Player player)
        {
            UtilityClass.YellowText("What Column do you want to drop? \ntype the number you want to drop\n");
            var droppables = new Dictionary<int, string>();
            for (var i = player.Scoreboard.Count - 1; i >= 0; i--)
            {
                var item = player.Scoreboard.ElementAt(i);
                var scoreName = item.Key;
                if (player.Scoreboard[scoreName] != null) continue;
                UtilityClass.GreenText($"{i}. {scoreName}\n");
                droppables.Add(i, scoreName);
            }
            try
            {
                var input = Convert.ToInt32(Console.ReadLine());
                if (!droppables.ContainsKey(input)) return;
                var nameOfScore = droppables[input];
                player.Scoreboard[nameOfScore] = 0;
                player.PlayerTurn = false;
            }
            catch (Exception e)
            {
                UtilityClass.RedText(e.Message);
            }
        }

        // Manages when the game ends and prints a ranked scoreboard
        private bool GameEnd()
        {
            if (RoundNumber == 15)
            {
                var rankedScores = Players.OrderByDescending(player => player.TotalSum()).ToList();
                var ranking = 1;
                foreach (var player in rankedScores)
                {
                    UtilityClass.GreenText($"{ranking}. {player}: {player.TotalSum()} points\n");
                    ranking++;
                }
                return true;
            };
            return false;
        }

        // Exit game
        private static void Exit() => Environment.Exit(0);
    }
}
