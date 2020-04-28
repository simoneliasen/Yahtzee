using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzy
{
    // If a negative/positive bias is selected by a player a bias percentage is assigned by the user,
    // which changes distribution of randomness in the selected biases favor.
    internal class BiasedDice : Dice
    {
        internal int Bias { get; set; }
        internal int BiasWeight { get; set; }

        internal BiasedDice(int bias, int biasWeight)
        {
            Bias = bias;
            BiasWeight = biasWeight;
        }

        // Overrides roll from the dice class.
        // The bias is calculated by duplicating elements in a List depending on the players input.
        internal override int Roll()
        {
            var chanceDistribution = new List<int>() { 1, 2, 3, 4, 5, 6 };
            switch (HoldState)
            {
                case false:
                {
                    switch (Bias)
                    {
                        case -1:
                            int twosRatio = Convert.ToInt32(BiasWeight * 0.75);
                            int threesRatio = Convert.ToInt32(BiasWeight * 0.50);
                            chanceDistribution.AddRange(Enumerable.Repeat(1, BiasWeight));
                            chanceDistribution.AddRange(Enumerable.Repeat(2, twosRatio));
                            chanceDistribution.AddRange(Enumerable.Repeat(3, threesRatio));
                            DiceValue = chanceDistribution[Rand.Next(0, chanceDistribution.Count)];
                            break;
                        case 0:
                            DiceValue = Rand.Next(1, 7);
                            break;
                        case 1:
                            int fivesRatio = Convert.ToInt32(BiasWeight * 0.75);
                            int foursRatio = Convert.ToInt32(BiasWeight * 0.50);
                            chanceDistribution.AddRange(Enumerable.Repeat(6, BiasWeight));
                            chanceDistribution.AddRange(Enumerable.Repeat(5, fivesRatio));
                            chanceDistribution.AddRange(Enumerable.Repeat(4, foursRatio));
                            DiceValue = chanceDistribution[Rand.Next(0, chanceDistribution.Count)];
                            break;
                    }

                    return DiceValue;
                }
                default: return DiceValue;
            }
        }
    }
}