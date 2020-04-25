using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; // Enumerable method
using System.Collections.Specialized;

namespace YahtzyNEW {
    //Stores all data on the user, their scoreboard, score possibilities and manage their list of dies.
    public class Player {
        public string Name { get; set; }
        public int PlayerRolls = 3;
        public bool PlayerTurn = true;
        public List<Dice> dieList = Enumerable.Range(1, 5).Select(i => new Dice()).ToList();
        public Dictionary<int, int> OccurenceOfEachDice;
        public Dictionary<string, int?> scoreboard;

        public Player(string name)
        {
            Name = name;
            scoreboard = new Dictionary<string, int?>
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

        // Return name by default
        public override string ToString()
        {
            return Name;
        }

        //Roll Players dice
        public void Roll()
        {
            if (PlayerRolls != 0)
            {
                foreach (Dice dice in dieList)
                {
                    dice.Roll();
                    if (dice.HoldState == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(dice.DiceValue + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(dice.DiceValue + " ");
                    }
                }
                this.PlayerRolls--;
            }
            else if (PlayerRolls == 0)
            {
                foreach (Dice dice in dieList)
                {
                    Console.Write(dice.DiceValue + " ");
                }
            }
            Console.WriteLine("\n---------------------------");
            Console.WriteLine(this.PlayerRolls + " rolls left");
            Console.WriteLine("---------------------------");
        }

        // Release lower section, when upper section is done, null == released
        public void CheckIfUpperSectionDone()
        {
            if (scoreboard["Ones"] != null &&
            scoreboard["Twos"] != null &&
            scoreboard["Threes"] != null &&
            scoreboard["Fours"] != null &&
            scoreboard["Fives"] != null &&
            scoreboard["Sixes"] != null)
            {
                // One pair is used as a check-value, if one pair exsists, all exists
                if (!scoreboard.ContainsKey("One Pair"))
                {
                    this.scoreboard.Add("One Pair", null);
                    this.scoreboard.Add("Two Pairs", null);
                    this.scoreboard.Add("Three Of A Kind", null);
                    this.scoreboard.Add("Four Of A Kind", null);
                    this.scoreboard.Add("Full House", null);
                    this.scoreboard.Add("Small Straight", null);
                    this.scoreboard.Add("Large Straight", null);
                    this.scoreboard.Add("Yahtzy", null);
                    this.scoreboard.Add("Chance", null);
                    Bonus();
                }
            }
        }

        // Check if uppersection scores are avaliable or assign score
        /*
         public int UpperSectionscores(int score, bool assign = false)
         {

             int points = this.dieList.Where(x => x.DiceValue.Equals(score)).Count() * score;
             return points;
         }
         */

        public int Ones(bool assign = false)
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(1)).Count();

            if (assign == true)
            {
                scoreboard["Ones"] = points * 1;
                PlayerTurn = false;
            }
            return points;
        }

        public int Twos(bool assign = false)
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(2)).Count();

            if (assign == true)
            {
                scoreboard["Twos"] = points * 2;
                PlayerTurn = false;
            }
            return points;
        }

        public int Threes(bool assign = false)
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(3)).Count();
            if (assign == true)
            {
                scoreboard["Threes"] = points * 3;
                PlayerTurn = false;
            }
            return points;
        }

        public int Fours(bool assign = false)
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(4)).Count();
            if (assign == true)
            {
                scoreboard["Fours"] = points * 4;
                PlayerTurn = false;
            }
            return points;
        }

        public int Fives(bool assign = false)
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(5)).Count();
            if (assign == true)
            {
                scoreboard["Fives"] = points * 5;
                PlayerTurn = false;
            }
            return points;
        }

        public int Sixes(bool assign = false)
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(6)).Count();
            if (assign == true)
            {
                scoreboard["Sixes"] = points * 6;
                PlayerTurn = false;
            }
            return points;
        }

        // Calculate occurences of each dice, to calculate scores for lower section.
        public void OccurencesOfDice() // could maybe be void?
        {
            List<int> diceroll = dieList.Select(dice => dice.DiceValue).ToList();

            for (int i = 1; i < 7; i++) // For each dice 0-5
            {
                int occurence = 0;
                foreach (int y in diceroll.FindAll(dice => dice == i))
                {
                    occurence++;
                }
                OccurenceOfEachDice[i] = occurence;
            }

            // Helper list for troubleshooting
            OccurenceOfEachDice.Select(o => $"{o.Key}: {o.Value}").ToList().ForEach(Console.WriteLine);
        }

        // Check if possible or assign depending on bool input
        public int Chance(bool assign = false)
        {
            int points = this.dieList.Sum(x => x.DiceValue);

            if (assign == true)
            {
                scoreboard["Chance"] = points;
                PlayerTurn = false;
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int OnePair(bool assign = false)
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value >= 2)
                {
                    points = 2 * item.Key;

                    if (assign == true)
                    {
                        scoreboard["One Pair"] = points;
                        PlayerTurn = false;
                    }
                }
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int TwoPairs(bool assign = false)
        {
            int points = 0;
            int temppoints = 0;
            int amountofpairs = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value >= 2)
                {
                    amountofpairs++;
                    temppoints += 2 * item.Key;
                }
            }
            if (amountofpairs >= 2)
            {
                points = temppoints;
                if (assign == true)
                {
                    scoreboard["Two Pairs"] = points;
                    PlayerTurn = false;
                }
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int ThreeOfAKind(bool assign = false)
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value >= 3)
                {
                    points = 3 * item.Key;

                    if (assign == true)
                    {
                        scoreboard["Three Of A Kind"] = points;
                        PlayerTurn = false;
                    }
                }
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int FourOfAKind(bool assign = false)
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value >= 4)
                {
                    points = 4 * item.Key;

                    if (assign == true)
                    {
                        scoreboard["Four Of A Kind"] = points;
                        PlayerTurn = false;
                    }
                }
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int SmallStraight(bool assign = false)
        {
            int points = 0;
            int[] occurencearray = OccurenceOfEachDice.Values.ToArray();
            int[] smallstraightoccurence = { 1, 1, 1, 1, 1, 0 };
            bool matchcheck = Enumerable.SequenceEqual(occurencearray, smallstraightoccurence);
            if (matchcheck)
            {
                points = 15;
                if (assign == true)
                {
                    scoreboard["Small Straight"] = points;
                    PlayerTurn = false;
                }
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int LargeStraight(bool assign = false)
        {
            int points = 0;
            int[] occurencearray = OccurenceOfEachDice.Values.ToArray();
            int[] largestraightoccurence = { 0, 1, 1, 1, 1, 1 };
            bool matchcheck = Enumerable.SequenceEqual(occurencearray, largestraightoccurence);
            if (matchcheck)
            {
                points = 20;
                if (assign == true)
                {
                    scoreboard["Large Straight"] = points;
                    PlayerTurn = false;
                }
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int FullHouse(bool assign = false)
        {
            int points = 0;
            int temppoints = 0;
            bool pair = false;
            bool threeofakind = false;

            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value == 2)
                {
                    temppoints += 2* item.Key;
                    pair = true;
                }
                if (item.Value == 3)
                {
                    temppoints += 3 * item.Key;
                    threeofakind = true;
                }
            }
            if (pair && threeofakind)
            {
                points = temppoints;
                if (assign == true)
                {
                    scoreboard["Full House"] = points;
                    PlayerTurn = false;
                }
            }
            return points;
        }

        // Check if possible or assign depending on bool input
        public int Yahtzy(bool assign = false)
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value == 5)
                {
                    points = 50;
                    if (assign == true)
                    {
                        scoreboard["Yahtzy"] = points;
                        PlayerTurn = false;
                    }
                }
            }
            return points;
        }

        // Check if viable for bonuspoints
        private void Bonus()
        {
            if (TotalSum() >= 63)
            {
                this.scoreboard.Add("Bonus", 50);
                Console.WriteLine("You got a bonus of 50 points, because you got over 63 points in the upper section");
            }
        }

        // Check if possible or assign depending on bool input
        public int? TotalSum()
        {
            return scoreboard.Sum(x => x.Value);
        }
    }
}


