using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCube : MonoBehaviour
{
    // Function to define behaviour of Cubes when player enters its collider
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        // If the collided object is the player
        if(playerInventory != null )
        {
            playerInventory.CollectCube();
            gameObject.SetActive(false); // Deactivate this cube
        }
    }
}
