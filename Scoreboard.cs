using System;
using System.Collections.Generic;
using System.Text;

namespace YahtzyNEW {
    /*
    class Scoreboard {

        public Dictionary<string, int?> scoreboard;


        // work in progress -- Add UI + set things in correct classes
        public Scoreboard() // Maybe player?
        {
            scoreboard = new Dictionary<string, int?>
            {
                ["Aces"] = null,
                ["Twos"] = null,
                ["Threes"] = null,
                ["Threes"] = null,
                ["Fours"] = null,
                ["Fives"] = null,
                ["Sixes"] = null,

                ["One Pair"] = null,
                ["Two Pairs"] = null,
                ["Three Of A Kind"] = null,
                ["Four Of A Kind"] = null,
                ["Full House"] = null,
                ["Small Straight"] = null,
                ["Large Straight"] = null,

                ["Yahtzee"] = null,
                ["Chance"] = null
            };

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

            public void ScorePossibilities(Player player)
            {
                // would be cool if this was integrated in print
                // print play.scoreboard["aces"] == null && player.Aces >=! print green)
                Console.Write("Theese Are the dies you can save to, write the number of where you want to save your points eg. '1'\n");
                if (player.scoreboard["Aces"] == null && player.Aces() >= 1)
                {
                    Console.WriteLine("1. Aces");
                }
                if (player.scoreboard["Twos"] == null && player.Twos() >= 1)
                {
                    Console.WriteLine("2. Twos");
                }
                if (player.scoreboard["Threes"] == null && player.Threes() >= 1)
                {
                    Console.WriteLine("3. Threes");
                }
                if (player.scoreboard["Fours"] == null && player.Fours() >= 1)
                {
                    Console.WriteLine("4. Fours");
                }
                if (player.scoreboard["Fives"] == null && player.Fives() >= 1)
                {
                    Console.WriteLine("5. Fives");
                }
                if (player.scoreboard["Sixes"] == null && player.Sixes() >= 1)
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
                if (player.scoreboard["Small Straight"] == null && player.SmallStraight() >= 1)
                {
                    Console.WriteLine("14. Small Straight");
                }
                if (player.scoreboard["Large Straight"] == null && player.LargeStraight() >= 1)
                {
                    Console.WriteLine("15. Large Straight");
                }
                SaveScore(player);
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

            public int? TotalSum()
            {
                return scoreboard.Sum(x => x.Value);
            }
        }
    }
    */
}
