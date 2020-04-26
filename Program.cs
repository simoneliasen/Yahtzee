/*Yahtzy 1.0
 * The following yahtzy game is build upon a modular approach, where the nesting of classes, should/would represent a real-world Yahtzy game, like so:
 * A die is given in the context of five other dice, which is held by a player that has an individual score, which exist in the context of other players
 * This is what makes up a yahtzy game. From this the following classes are created to describe it. 
 * 
 * Classes
 *  - Program: serves as the starting point that initializes the game.
 * 
 *  - Dice: holds the value of a dice, aswell as if the dice i 'held' from a previous rounds, additionaly this class also controls the bias of the dice,
 *    which can be altered thorughout the game.
 *  
 *   - Player: As this is a multiplayer game, the player has he's/her's own class, to manage each player, a collection of the players dice,
 *     the amount of rolls pr. round (which can be altered) the name of the player, aswell as if it's the players turn, additionally this class also contains
 *     the scoreboard within it aswell and methods for caluclating if score-assignment is possible, and assigning the score.
 *  
 *   - Game: The game class, can be seen as a view for the player, where commands and input is transformed into the 
 *     logic that exists in the other classes, this class also holds global values and methods, which is static in nature
 *     which is either true no matter what the player inputs, or if the value is not changed by the player throughout the game.
 *   
 * Assumptions: 
 *   - In the application the biased dice is made in one class, with three options of biaseness, as making an advance function for distribution
 *     in a yahtzy game seemed over the top.
 *  
 *  - The change of rolls method is applied to all players
 *  
 *  - The release method releases all dies
 *  
 *  - The nesting of scoreboard is not chosen as it's complicated??
 *  
 *  - Bias is applied to all users
 *   
 * Imported Libraries:
 *   - LINQ:
 *   
 *   // Iteration of dictionariy == bad
 *   
 *   
 *   Game rules
 *   https://en.wikipedia.org/wiki/Yatzy
 */

namespace Yahtzy {
    internal class Program {
        private static void Main(string[] args)
        {
            var yahtzy = new Game();
            yahtzy.SetupGame();
        }
    }
}