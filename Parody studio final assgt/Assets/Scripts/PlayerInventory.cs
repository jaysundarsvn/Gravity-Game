using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    // Variable to track number of cubes collected
    public int NumberOfCubes { get; private set; }

    public int TotalNumberOfCubes;


    public GameManager GameManager;

    [Tooltip("Event when player collects a cube")]
    public UnityEvent<PlayerInventory> OnCubeCollected;

    // Function to define game behaviour to collect a cube
    public void CollectCube()
    {
        NumberOfCubes++;
        OnCubeCollected.Invoke(this); // Invoke the event

        // If all the cubes have been collected
        if (NumberOfCubes >= TotalNumberOfCubes)
        {
            GameManager.EndGame(true, "You collected all the cubes"); // The game is won
        }
    }
}
