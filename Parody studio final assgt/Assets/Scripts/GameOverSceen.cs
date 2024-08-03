using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceen : MonoBehaviour
{
    public bool ScreenVisible = false;

    [Header("Text references")]
    public TextMeshProUGUI Title;

    public TextMeshProUGUI Message;

    // Function to display the Game Over screen with appropriate title and message
    // Parameter gameWon states whether the game was won or lost, and a corresponding message is passed
    public void Show(bool gameWon, string text)
    {
        // Display the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Activate the Game Over screen
        gameObject.SetActive(true);
        ScreenVisible = true;

        // Slow down the time scale so the game scenario can be viewed in the background
        Time.timeScale = 0.1f;

        Message.text = text;
        if (gameWon)
        {
            Title.text = "You Won!";
        }
        else
        {
            Title.text = "Game Over";
        }
    }

    // Function to reload the current screen for player to retry the level
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    // Function to quit the game
    public void Quit()
    {
        Application.Quit();
    }
}
