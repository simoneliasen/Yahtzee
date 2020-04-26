using System;
using System.Collections.Generic;
using System.Linq;

namespace YahtzyNEW {
    public class Player {
        public string Name { get; set; }
        public int PlayerRolls = 3;
        public bool PlayerTurn = true;
        public List<Dice> dieList = Enumerable.Range(1, 5).Select(i => new Dice()).ToList();
        public Dictionary<int, int> OccurenceOfEachDice;
        public Dictionary<string, int?> scoreboard;
        public bool UpperSection = true;

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
        public override string ToString() => Name;

        //Roll Players dice
        public void Roll()
        {
            if (PlayerRolls != 0)
            {
                for (int i = 0; i < dieList.Count; i++)
                {
                    Dice dice = dieList[i];
                    dice.Roll();
                    switch (dice.HoldState)
                    {
                        case true:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(dice.DiceValue + " ");
                            Console.ResetColor();
                            break;
                        default:
                            Console.Write(dice.DiceValue + " ");
                            break;
                    }
                }
                PlayerRolls--;
            }
            else if (PlayerRolls == 0)
            {
                foreach (Dice dice in dieList)
                {
                    Console.Write(dice.DiceValue + " ");
                }
            }
            Console.WriteLine("\n---------------------------");
            Console.WriteLine(PlayerRolls + " rolls left");
            Console.WriteLine("---------------------------");
        }

        // Release lower section, when upper section is done, (null == avaliable)
        public void CheckIfUpperSectionDone()
        {
            if (scoreboard["Ones"] != null &&
            scoreboard["Twos"] != null &&
            scoreboard["Threes"] != null &&
            scoreboard["Fours"] != null &&
            scoreboard["Fives"] != null &&
            scoreboard["Sixes"] != null)
            {
                // One pair is used as a check-value, if "one pair" exists, all should exists
                if (!scoreboard.ContainsKey("One Pair"))
                {
                    scoreboard.Add("One Pair", null);
                    scoreboard.Add("Two Pairs", null);
                    scoreboard.Add("Three Of A Kind", null);
                    scoreboard.Add("Four Of A Kind", null);
                    scoreboard.Add("Full House", null);
                    scoreboard.Add("Small Straight", null);
                    scoreboard.Add("Large Straight", null);
                    scoreboard.Add("Yahtzy", null);
                    scoreboard.Add("Chance", null);
                    Bonus();
                    UpperSection = false;
                }
            }
        }

        // Check if scores are possible, and assign by userInput
        public void UpperSectionScores()
        {
            for (int i = 0; i < scoreboard.Count; i++)
            {
                var item = scoreboard.ElementAt(i);
                var itemKey = item.Key;
                var itemValue = item.Value;

                // i+1 to adjust to Ones == 1 (eventhough it's at index 0, for useroutput
                if (scoreboard[itemKey] == null && (dieList.Where(x => x.DiceValue.Equals((i+1))).Count() >= 1))  // && player.Ones() >= 1
                {
                    Console.WriteLine((i+1) + ". " + itemKey + itemValue);
                }
            }
            try
            {   // Assign score based on userInput (Still assign if not in dicelist)
                int input = Convert.ToInt32(Console.ReadLine());
                for (int i = 0; i < scoreboard.Count; i++)
                {
                    var item = scoreboard.ElementAt(i);
                    var itemKey = item.Key;
                    var itemValue = item.Value;
                    if (input == (i + 1) && (dieList.Where(x => x.DiceValue.Equals((i + 1))).Count() >= 1)) // is input for One and so on
                    {
                        scoreboard[itemKey] = dieList.Where(x => x.DiceValue.Equals(i + 1)).Count() * (i + 1);
                        PlayerTurn = false;
                       // break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
            if (assign == true)
            {
                scoreboard["Chance"] = dieList.Sum(x => x.DiceValue);
            }
            return dieList.Sum(x => x.DiceValue);
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
                    temppoints += 2 * item.Key;
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
                scoreboard.Add("Bonus", 50);
                Console.WriteLine("You got a bonus of 50 points, because you got over 63 points in the upper section");
            }
        }

        // Check if possible or assign depending on bool input
        public int? TotalSum() => scoreboard.Sum(x => x.Value);
    }
}


