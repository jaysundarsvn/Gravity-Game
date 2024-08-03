using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    // Variable to store reference to self as a TextMeshProUGUI object
    private TextMeshProUGUI cubeText;
    
    void Start()
    {
        cubeText = GetComponent<TextMeshProUGUI>();
    }

    // Function to update the text with the count of cubes collected
    // This function is a subscriber to the OnCubeCollected event of the PlayerInventory script
    public void updateCubeText(PlayerInventory playerInventory)
    {
        cubeText.text = playerInventory.NumberOfCubes.ToString();
    }
}
