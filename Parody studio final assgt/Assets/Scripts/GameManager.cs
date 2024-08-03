using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameOverSceen GameOverSceen;

    public Canvas PlayCanvas;

    // Flag to denote of the Game Over screen has been activated
    private bool gameHasEnded = false;

    // Function to initialize the Game Over screen and hide game HUD
    // Parameter gameWon states whether the game was won or lost, and a corresponding message is passed
    public void EndGame(bool gameWon, string message)
    {
        // Run the following code only once when the game is ended. After which the flag is set to true
        // and the code is not repeated unnecessarily
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            GameOverSceen.Show(gameWon, message);

            // Hide the game HUD
            PlayCanvas.gameObject.SetActive(false);
        }
    }
}
