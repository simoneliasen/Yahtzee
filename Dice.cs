using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzy {
    // Roll random dice as default and control the dice value as well as if the dice is 'held'.
    internal class Dice {
        protected Random Rand { get; }
        internal int DiceValue { get; set; }
        internal bool HoldState { get; set; }

        internal Dice()
        {
            HoldState = false;
            Rand = new Random();
        }

        internal virtual int Roll()
        {
            if (!HoldState)
            {
                DiceValue = Rand.Next(1, 7);
            }

            return DiceValue;
        }
    }
}
