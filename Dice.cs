using System;

namespace YahtzyNEW {
    public class Dice {
        public Random rand = new Random();
        public int DiceValue { get; set; }
        public bool HoldState { get; set; }
        public int Bias = 0;

        /*
                        int BiasPercentage = 100000;

                        List<int> DiceSides = new List<int>() { 1, 2, 3, 4, 5, 6 };

                        int SixesRatio = BiasPercentage;
                        int FivesRatio = Convert.ToInt32(BiasPercentage * 0.70);
                        int FoursRatio = Convert.ToInt32(BiasPercentage * 0.50);

                        DiceSides.AddRange(Enumerable.Repeat(6, SixesRatio));
                        DiceSides.AddRange(Enumerable.Repeat(5, FivesRatio));
                        DiceSides.AddRange(Enumerable.Repeat(4, FoursRatio));

                        this.DiceValue = DiceSides[rand.Next(0, DiceSides.Count)];
         */

        public virtual int Roll()
        {
            switch (HoldState)
            {
                case false:
                    {
                        switch (Bias)
                        {
                            case -1:
                                int[] negativeRandArray = { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 4, 5, 6, };
                                DiceValue = negativeRandArray[rand.Next(0, negativeRandArray.Length)];
                                break;
                            case 0:
                                DiceValue = rand.Next(1, 7);
                                break;
                            case 1:
                                int[] positiveeRandArray = { 1, 2, 3, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6 };
                                DiceValue = positiveeRandArray[rand.Next(0, positiveeRandArray.Length)];
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
