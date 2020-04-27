
/* Yahtzy 1.0

The following yahtzy games design is built upon the real-world application of it.
A dice is given in the context of five other dice, which is held by a player, which plays with other players. Each individual player has a score, which they keep track of.
Based on this we can describe the following classes that seeks to mimic and support this.

Classes

- The Program Class: serves as the starting point that initializes the game.

- The Game Class: Can be seen as a view for the player, where generic commands are managed based on the users input, that controls logic in other nested classes.

- The Dice Class: holds the value of a dice, as well as if the dice is 'held' from a previous rounds, additionally this class also controls the bias of the dice, which can be set by the player.

- The BiasedDice Class: This class is inherited from the Dice class and then additionally controls the bias amd the weigth of the bias set by the player.

- The Player class: Contains player information, such as a scoreboard, and the calculations for checking scores are available, and can be assigned.

- The Utility Class: a static class that provides the program with methods for printing output that enhances the user experience of the game.

Assumptions
-	A user wants to release all his dice
-	Inheritance biaseddice?
-   Amount of rolls is applied to all players
ASsumptions = amount of rolls for both, highest pairs if pairs

Game rules
https://en.wikipedia.org/wiki/Yatzy
*/

namespace Yahtzy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var yahtzy = new Game();
            yahtzy.SetupGame();
        }
    }
}