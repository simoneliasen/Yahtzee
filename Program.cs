using System;
using System.Collections.Generic;
using System.Text;

namespace YahtzyNEW {
     class Program {




        // https://stackoverflow.com/questions/28902942/c-sharp-homework-dice-game 
        #region lambda shortcuts
        static Action<object> Warn = m => {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(m);
        };
        static Action<object> Info = m => {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(m);
        };
        static Action<object> WriteQuery = m => {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(m);
        };
        static Action BreakLine = Console.WriteLine;
        #endregion




        static void Main(string[] args)
        {
            Game Yahtzy = new Game();
            Yahtzy.SetupGame();
            }
    }
}