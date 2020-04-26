using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzy
{
    public class Player
    {
        private string Name { get; }
        internal bool UpperSection { get; set; } = true;
        internal int PlayerRolls { get; set; } = 3;
        internal bool PlayerTurn { get; set; } = true;
        private Dictionary<int, int> OccurenceOfEachDice { get; }
        internal List<Dice> DieList { get; set; } = Enumerable.Range(1, 5).Select(i => new Dice()).ToList();
        internal Dictionary<string, int?> Scoreboard { get; set; }

        internal Player(string name)
        {
            Name = name;
            Scoreboard = new Dictionary<string, int?>
            {
                ["Ones"] = null,
                ["Twos"] = null,
                ["Threes"] = null,
                ["Fours"] = null,
                ["Fives"] = null,
                ["Sixes"] = null
            };
            OccurenceOfEachDice = new Dictionary<int, int>
            {
                [1] = 0,
                [2] = 0,
                [3] = 0,
                [4] = 0,
                [5] = 0,
                [6] = 0,
            };
        }

        // Return name. 
        public override string ToString() => Name;

        //Roll players dice.
        internal void Roll()
        {
            if (PlayerRolls != 0)
            {
                foreach (var dice in DieList)
                {
                    dice.Roll();
                    switch (dice.HoldState)
                    {
                        case true:
                            UtilityClass.RedText($"{dice.DiceValue} ");
                            break;
                        default:
                            Console.Write($"{dice.DiceValue} ");
                            break;
                    }
                }

                PlayerRolls--;
            }
            else if (PlayerRolls == 0)
            {
                foreach (var dice in DieList)
                {
                    Console.Write($"{dice.DiceValue} ");
                }
            }

            Console.WriteLine("\n---------------------------");
            Console.Write($"{PlayerRolls} rolls left\n");
            Console.WriteLine("---------------------------");
        }

        // Release lower section, when upper section is done. (null == available)
        internal void CheckIfUpperSectionDone()
        {
            if (Scoreboard["Ones"] == null || Scoreboard["Twos"] == null || Scoreboard["Threes"] == null ||
                Scoreboard["Fours"] == null || Scoreboard["Fives"] == null || Scoreboard["Sixes"] == null) return;

            if (Scoreboard.ContainsKey("One Pair")) return;
            Scoreboard.Add("One Pair", null);
            Scoreboard.Add("Two Pairs", null);
            Scoreboard.Add("Three Of A Kind", null);
            Scoreboard.Add("Four Of A Kind", null);
            Scoreboard.Add("Full House", null);
            Scoreboard.Add("Small Straight", null);
            Scoreboard.Add("Large Straight", null);
            Scoreboard.Add("Yahtzy", null);
            Scoreboard.Add("Chance", null);
            Bonus();
            UpperSection = false;
        }

        // Calculate how many times each dice occur, to calculate more complex scores in the lower section.
        internal void OccurrencesOfDice()
        {
            var diceList = DieList.Select(dice => dice.DiceValue).ToList();
            for (var i = 1; i < 7; i++)
            {
                var occurence = (diceList.FindAll(dice => dice == i).Select(y => y)).Count();
                OccurenceOfEachDice[i] = occurence;
            }
        }

        // Check if score is available based on occurence of each dice, and assigns to scoreboard if bool input is true.
        // The same is the case for the renaming score calculations in the lower section.
        internal int OnePair(bool assign = false)
        {
            var points = 0;
            foreach (var item in OccurenceOfEachDice.Where(item => item.Value >= 2).Select(item => item))
            {
                points = 2 * item.Key;
                if (assign)
                {
                    Scoreboard["One Pair"] = points;
                }
            }

            return points;
        }

        internal int TwoPairs(bool assign = false)
        {
            var points = 0;
            var tempPoints = 0;
            var amountOfPairs = 0;
            foreach (var item in OccurenceOfEachDice.Where(item => item.Value >= 2).Select(item => item))
            {
                amountOfPairs++;
                tempPoints += 2 * item.Key;
            }

            if (amountOfPairs < 2) return points;
            points = tempPoints;
            if (assign)
            {
                Scoreboard["Two Pairs"] = points;
            }

            return points;
        }

        internal int ThreeOfAKind(bool assign = false)
        {
            var points = 0;
            foreach (var item in OccurenceOfEachDice.Where(item => item.Value >= 3).Select(item => item))
            {
                points = 3 * item.Key;
                if (assign)
                {
                    Scoreboard["Three Of A Kind"] = points;
                }
            }

            return points;
        }

  
        internal int FourOfAKind(bool assign = false)
        {
            var points = 0;
            foreach (var item in OccurenceOfEachDice.Where(item => item.Value >= 4).Select(item => item))
            {
                points = 4 * item.Key;
                if (assign)
                {
                    Scoreboard["Four Of A Kind"] = points;
                }
            }

            return points;
        }

        internal int SmallStraight(bool assign = false)
        {
            var points = 0;
            var occurenceArray = OccurenceOfEachDice.Values.ToArray();
            int[] smallStraightOccurence = {1, 1, 1, 1, 1, 0};
            var matchCheck = occurenceArray.SequenceEqual(smallStraightOccurence);
            if (!matchCheck) return points;
            points = 15;
            if (assign)
            {
                Scoreboard["Small Straight"] = points;
            }

            return points;
        }

        internal int LargeStraight(bool assign = false)
        {
            var points = 0;
            var occurenceArray = OccurenceOfEachDice.Values.ToArray();
            int[] largeStraightOccurence = {0, 1, 1, 1, 1, 1};
            var matchCheck = occurenceArray.SequenceEqual(largeStraightOccurence);
            if (!matchCheck) return points;
            points = 20;
            if (assign)
            {
                Scoreboard["Large Straight"] = points;
            }

            return points;
        }

        internal int FullHouse(bool assign = false)
        {
            var points = 0;
            var tempPoints = 0;
            var pair = false;
            var threeOfAKind = false;
            foreach (var (key, value) in OccurenceOfEachDice)
            {
                switch (value)
                {
                    case 2:
                        tempPoints += 2 * key;
                        pair = true;
                        break;
                    case 3:
                        tempPoints += 3 * key;
                        threeOfAKind = true;
                        break;
                }
            }

            if (!pair || !threeOfAKind) return points;
            points = tempPoints;
            if (assign)
            {
                Scoreboard["Full House"] = points;
            }

            return points;
        }

        internal int Yahtzy(bool assign = false)
        {
            var points = 0;
            foreach (var item in OccurenceOfEachDice.Where(item => item.Value == 5))
            {
                points = 50;
                if (assign)
                {
                    Scoreboard["Yahtzy"] = points;
                }
            }

            return points;
        }

        internal int Chance(bool assign = false)
        {
            if (assign)
            {
                Scoreboard["Chance"] = DieList.Sum(x => x.DiceValue);
            }

            return DieList.Sum(x => x.DiceValue);
        }

        // Assigns bonus points to dictionary, if total sum is above 63 in upper section.
        private void Bonus()
        {
            if (!(TotalSum() >= 63)) return;
            Scoreboard.Add("Bonus", 50);
            UtilityClass.GreenText("You got 50 bonus points, because you got over 63 points in the upper section");
        }

        // Get players total sum.
        internal int? TotalSum() => Scoreboard.Sum(x => x.Value);
    }
}