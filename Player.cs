using System;
using System.Collections.Generic;
using System.Text;
using System.Linq; // Enumerable method

namespace YahtzyNEW {
    //Stores all data on the user, their scoreboard, score possibilities and manage their list of dies.
    public class Player {
        public string Name { get; set; }
        public int PlayerRolls = 3;
        public bool PlayerTurn = true;
        public List<Dice> dieList = Enumerable.Range(1, 5).Select(i => new Dice()).ToList();
        public Dictionary<int, int> OccurenceOfEachDice;
        public Dictionary<string, int?> scoreboard;
        // public Dictionary<Tuple<int, bool>, string> updatedscoreboard; // int = index, bool = possible, string = Name
        // Sorted dictionary also idea, or using enums

        // Player Constructors
        public Player(string name)
        {
            Name = name;
            scoreboard = new Dictionary<string, int?>
            {
                ["Aces"] = null,
                ["Twos"] = null,
                ["Threes"] = null,
                ["Threes"] = null,
                ["Fours"] = null,
                ["Fives"] = null,
                ["Sixes"] = null,

                ["One Pair"] = 0,
                ["Two Pairs"] = 0,
                ["Three Of A Kind"] = 0,
                ["Four Of A Kind"] = 0,
                ["Full House"] = 0,
                ["Small Straight"] = 0,
                ["Large Straight"] = 0,

                ["Yahtzee"] = null,
                ["Chance"] = null
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

        //Roll Dice
        public void Roll()
        {
            if (PlayerRolls != 0)
            {

                foreach (Dice dice in dieList)
                {
                    dice.Roll();
                    Console.Write(dice.DiceValue + " ");
                }
                Console.WriteLine("\n---------------------------");
                this.PlayerRolls--;
            }
            else if (PlayerRolls == 0)
            {
                Console.WriteLine("Out of rolls");
            }
            Console.WriteLine("\n");
            Console.WriteLine(this.PlayerRolls + " rolls left");
        }

        // Assign points
        public int Aces() // int as input perhaps
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(1)).Count();
            return points;
        }
        public int Twos()
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(2)).Count() * 2;
            return points;
        }
        public int Threes()
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(3)).Count() * 3;
            return points;
        }
        public int Fours()
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(4)).Count() * 4;
            return points;
        }
        public int Fives()
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(5)).Count() * 5;
            return points;
        }
        public int Sixes()
        {
            int points = this.dieList.Where(x => x.DiceValue.Equals(6)).Count() * 6;
            return points;
        }
        public int Chance()
        {
            int points = this.dieList.Sum(x => x.DiceValue);
            return points;
        }
        public int Yahtzy()
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value == 5)
                {
                    points = 50;
                }
            }
            return points;
        }

        // Release lower section, when upper section is done, null == released
        public void CheckIfUpperSectionDone()
        {
            if (scoreboard["Aces"] != null &&
                scoreboard["Twos"] != null &&
                scoreboard["Threes"] != null &&
                scoreboard["Fours"] != null &&
                scoreboard["Fives"] != null &&
                scoreboard["Sixes"] != null)
            {
                scoreboard["One Pair"] = null;
                scoreboard["Two Pairs"] = null;
                scoreboard["Three Of A Kind"] = null;
                scoreboard["Four Of A Kind"] = null;
                scoreboard["Full House"] = null;
                scoreboard["Small Straight"] = null;
                scoreboard["Large Straight"] = null;
            }
            Bonus();
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

        // Calculate occurences of each dice, to calculate more complex scores, such as pairs, of a kind, full house etc.
        public Dictionary<int, int> OccurencesOfDice()
        {
            List<int> diceroll = dieList.Select(dice => dice.DiceValue).ToList();

            for (int i = 1; i < 7; i++) // For each dice 0-5
            {
                int occurence = 0;
                foreach (int y in diceroll.FindAll(dice => dice == i))
                {
                    occurence++;
                }
                this.OccurenceOfEachDice[i] = occurence;
            }
            
            this.OccurenceOfEachDice.Select(o => $"{o.Key}: {o.Value}").ToList().ForEach(Console.WriteLine);

            return this.OccurenceOfEachDice;
        }

        // works (could possible add possibility of choosing which pair if more than one exsist)
        public int OnePair()
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value >= 2)
                {
                    points = 2 * item.Key;
                }
            }
                return points;
        }

        // works
        public int TwoPairs() 
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

            }
            return points; 
        }

        // works
        public int ThreeOfAKind()
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value >= 3)
                {
                    points = 3 * item.Key;
                }
            }
            return points;
        }

        // works
        public int FourOfAKind()
        {
            int points = 0;
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value >= 4)
                {
                    points = 4 * item.Key; 
                }
            }
            return points;
        }

        // fucky. perhaps sorted array is easier?
        public int SmallStraight() 
        {
            int points = 0;
            int[] scoreboardvaluelist = OccurenceOfEachDice.Values.ToArray();

            int[] firstscenario = { 1, 1, 1, 1, 0, 0 };
            int[] secondscenario = { 0, 1, 1, 1, 1, 0 };
            int[] thirdscenario = { 0, 0, 1, 1, 1, 1 };

            if (scoreboardvaluelist.Equals(firstscenario) || scoreboardvaluelist.Equals(secondscenario) || scoreboardvaluelist.Equals(thirdscenario))
            {
                points = 30;
            }
            return points;
        }

        // fucky, much easier with sorted array
        public int LargeStraight() 
        {
/*
            Array.Sort(i);

            if (((i[0] == 1) &&
                 (i[1] == 2) &&
                 (i[2] == 3) &&
                 (i[3] == 4) &&
                 (i[4] == 5)) ||
                ((i[0] == 2) &&
                 (i[1] == 3) &&
                 (i[2] == 4) &&
                 (i[3] == 5) &&
                 (i[4] == 6)))
            {
                Sum = 40;
            }
            */
            int points = 0;
            int[] scoreboardvaluelist = OccurenceOfEachDice.Values.ToArray();

            int[] firstscenario = { 1, 1, 1, 1, 1, 0 };
            int[] secondscenario = { 0, 1, 1, 1, 1, 1 };

            if (scoreboardvaluelist.Equals(firstscenario) || scoreboardvaluelist.Equals(secondscenario))
            {
                points = 40;
            }
            return points;
        }

        // works
        public int FullHouse()
        {
            int points = 0;
            bool pair = false;
            bool threeofakind = false;
 
            foreach (var item in OccurenceOfEachDice)
            {
                if (item.Value == 2)
                {
                    pair = true;
                    break;
                }
                if (item.Value == 3) 
                {
                    threeofakind = true;
                }
            }
            if (pair && threeofakind)
            {
                points = 25;
            }
            return points;
        }

        // Get Total sum of player
        public int? TotalSum()
        {
            return scoreboard.Sum(x => x.Value);
        }
    }
}
    

