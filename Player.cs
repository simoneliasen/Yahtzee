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
            int points = this.dieList.Sum(x => x.DiceValue);
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

        // Checks occurences of each dice
        public Dictionary<int, int> OccurencesOfDice()
        {
            List<int> diceroll = dieList.Select(x => x.DiceValue).ToList();

            for (int i = 1; i < 7; i++) // For each dice 0-5
            {
                int count = 0;
                foreach (int y in diceroll.FindAll(x => x == i))
                {
                    count++;
                }
                this.OccurenceOfEachDice[i] = count;
            }
            this.OccurenceOfEachDice.Select(o => $"{o.Key}: {o.Value}").ToList().ForEach(Console.WriteLine);
            if (OccurenceOfEachDice.ContainsValue(2))
            {
                Console.WriteLine("Contains a pair");
            }
            return this.OccurenceOfEachDice;
        }

        public int OnePair()
        {
            int points = 0;
            int pairs = 0;
            for (int i = 0; i < OccurenceOfEachDice.Values.Count(); i++)
            {
                if (OccurenceOfEachDice[i] <= 2) // If equals two or larger (3 is still a pair)
                {
                    pairs++;
                    points += i * 2;
                }
            }
            // Is possible?
            // assign
            Console.WriteLine("You have a pair");

            // If 2 or more is in list of occurences, display as option
            // if more one pair occur, give option to select what pair to use
            return 1;
        }
        public int TwoPairs()
        {
            // If Onepair * 2
            return 1;
        }
        public int ThreeOfAKind()
        {
            // If three or more exist in array (steal from one pair)
            return 1;
        }
        public int FourOfAKind()
        {
            // If four or more of a kind exist
            return 1;
        }
        public int SmallStraight()
        {
            // Sorted array method? -1 start and -1 end check if they are one larger than eachother?
           
            return 1;
        }
        public int LargeStraight()
        {
            // sorted array if it's equal to 1,2,3,4,5
            // if dielist == 1,2,3,4,5
            return 1;
        }
        public int FullHouse()
        {
            //bool ThreeOfAKind = false;// add methods in future
            //bool OnePair = false; // add methods in future
            int points = 0;
            for (int i = 0; i < OccurenceOfEachDice.Values.Count(); i++)
            {
                if (OccurenceOfEachDice[i] == 3)
                {
                    points += i * 3;
                    //ThreeOfAKind = true;
                }
                if (OccurenceOfEachDice[i] == 2)
                {
                    points += i * 2;
                    //OnePair = true;

                }
            }
            return points;

            //if (ThreeOfAKind && OnePair)
            //{
            //    return points;
            //}
        }

        // Get Total sum of player
        public int? TotalSum()
        {
            return scoreboard.Sum(x => x.Value);
        }
    }
}
    

