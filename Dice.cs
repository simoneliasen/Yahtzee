using System;
using System.Collections.Generic;
using System.Text;

namespace YahtzyNEW {
    public class Dice {

    public Random rand = new Random();    
    public int DiceValue { get; set; }
    public bool HoldState { get; set; }
    public int Bias = 0;

    // bias variable and one roll method perhaps?
    public int Roll() // maybe roll methods could be one method, if simple enough (set default input at 50 (neutral dice) and make user change)
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

    public class BiasedDice : Dice {
        public int NegativeBiasRoll() 
        {
            if (this.HoldState == false)
            {
                int[] negativeRandArray = { 1, 1, 1, 2, 2, 3, 3, 4, 5, 6, }; 
                this.DiceValue = negativeRandArray[rand.Next(0, negativeRandArray.Length)];
                return this.DiceValue;
            }
            else
            {
                return this.DiceValue;
            }
        }

        public int PositiveBiasRoll()
        {
            if (this.HoldState == false)
            {
                int[] positiveeRandArray = { 1, 2, 3, 4, 4, 5, 5, 6, 6, 6 }; 
                this.DiceValue = positiveeRandArray[rand.Next(0, positiveeRandArray.Length)];
                return this.DiceValue;
            }
            else
            {
                return this.DiceValue;
            }
        }
    }
}
