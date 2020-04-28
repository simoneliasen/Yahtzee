
/* Yahtzy 1.0

The following application is built upon the real-world application of a yahtzy game.
A dice is given in the context of five other dice, which is held by a player, which plays with other players. 
Each individual player has a score, which they keep track of.

Based on this we can describe the following classes that seeks to mimic and support this.

- The Program Class: 
Serves as the starting point that initializes the game.

- The Game Class: 
Can be seen as a view for the player, where generic commands are managed based on the users input, that controls logic in other areas of the application.

- The Dice Class: 
Holds the value of a dice, as well as if the dice is 'held' from a previous rounds.

- The Biased Dice Class: 
This class is inherited from the dice class and additionally controls the bias and the weight of the bias set by the player.

- The Player Class: 
Contains player information, such as a scoreboard, and the calculations checking if scores are available, and can be assigned.

- The Utility Class: 
A static class that provides the program with methods for printing output that enhances the user experience of the game.

Assumptions:
- A user wants to release all dice from being 'held', not only one.
- A change in amount of rolls will affect all players.
- The player will always want to assign the largest pair to themself.

The rules of the games can be found here:
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