using System;

namespace Yahtzy
{
    internal static class UtilityClass
    {
        // Red text for displaying errors.
        internal static void RedText(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(input);
            Console.ResetColor();
        }

        // Yellow text for important messages.
        internal static void YellowText(string input)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(input);
            Console.ResetColor();
        }

        // Green text for success messages.
        internal static void GreenText(string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(input);
            Console.ResetColor();
        }
    }
}