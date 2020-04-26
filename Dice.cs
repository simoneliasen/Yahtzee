using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzy {
    public class Dice {
        internal int DiceValue { get; set; }
        internal bool HoldState { get; set; }
        private Random Rand { get; } = new Random();
        internal int Bias { get; set; } = 0;
        internal int BiasWeight { get; set; } = 1;

        protected internal int Roll()
        {
            var chanceDistribution = new List<int>() { 1, 2, 3, 4, 5, 6 };
            switch (HoldState)
            {
                case false:
                    {
                        switch (Bias)
                        {
                            case -1:
                                var twosRatio = Convert.ToInt32(BiasWeight * 0.75);
                                var threesRatio = Convert.ToInt32(BiasWeight * 0.50);
                                chanceDistribution.AddRange(Enumerable.Repeat(1, BiasWeight));
                                chanceDistribution.AddRange(Enumerable.Repeat(2, twosRatio));
                                chanceDistribution.AddRange(Enumerable.Repeat(3, threesRatio));
                                DiceValue = chanceDistribution[Rand.Next(0, chanceDistribution.Count)];
                                break;
                            case 0:
                                DiceValue = Rand.Next(1, 7);
                                break;
                            case 1:
                                var fivesRatio = Convert.ToInt32(BiasWeight * 0.75);
                                var foursRatio = Convert.ToInt32(BiasWeight * 0.50);
                                chanceDistribution.AddRange(Enumerable.Repeat(6, BiasWeight));
                                chanceDistribution.AddRange(Enumerable.Repeat(5, fivesRatio));
                                chanceDistribution.AddRange(Enumerable.Repeat(4, foursRatio));
                                DiceValue = chanceDistribution[Rand.Next(0, chanceDistribution.Count)];
                                break;
                        }
                        return DiceValue;
                    }
                default:
                    return DiceValue;
            }
        }
    }
}
