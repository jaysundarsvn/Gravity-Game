using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float TimeLeft;

    public bool TimerOn = false;

    [Space(10)]
    public GameManager GameManager;

    // Variable to store reference to self as a TextMeshProUGUI object
    private TextMeshProUGUI TimerText;

    // Function to start the timer
    public void StartTimer()
    {
        TimerOn = true;
    }

    private void Start()
    {
        StartTimer();
        TimerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (TimerOn)
        {
            // If time is left
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft); // Update the text to new time
            }
            else // If time is exhausted
            {
                TimeLeft = 0;
                TimerOn = false;
                GameManager.EndGame(false, "You ran out of time");
            }
        }
    }

    // Function to update the timer text to a new time
    private void updateTimer(float currentTime)
    {
        currentTime += 1;

        // Format the time left into mm : ss format
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
