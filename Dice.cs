using System;
using System.Collections.Generic;
using System.Text;

namespace YahtzyNEW {
    public class Dice {
        public Random rand = new Random();
        public int DiceValue { get; set; }
        public bool HoldState { get; set; }
        public int Bias = 0;

        public virtual int Roll()
        {
            if (this.HoldState == false)
            {
                switch (Bias)
                {
                    case (-1):
                        int[] negativeRandArray = { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 4, 5, 6, };
                        this.DiceValue = negativeRandArray[rand.Next(0, negativeRandArray.Length)];
                        break;

                    case (0):
                        this.DiceValue = rand.Next(1, 7);
                        break;

                    case (1):
                        int[] positiveeRandArray = { 1, 2, 3, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6 };
                        this.DiceValue = positiveeRandArray[rand.Next(0, positiveeRandArray.Length)];
                        break;
                }
                return this.DiceValue;
            }
            else
            {
                return this.DiceValue;
            }
        }
    }
}
