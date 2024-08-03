using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public GameObject HoloPrefab;

    [Tooltip("The main camera")]
    public GameObject Camera;

    public bool HoloVisible = false;

    public float HoloRotationTime = 0.5f;

    public float SwitchingTime = 0.2f;

    [Header("Other Script references")]
    public ThirdPersonController thirdPersonController;

    public GameOverSceen gameOverSceen;

    // Gameobject to instantiate and store the Hologram prefab
    private GameObject holo;

    // Utility function to get the multiple of 90 closest to x. Useful in restricting rotations to principle axes ony
    private float ClosestMultipleOf90(float x)
    {
        int temp = (int) x / 45;
        return (temp + (temp & 1)) * 45f; // For even value of temp, we return temp*45. And for odd values (temp+1)*45
    }

    // Custom Coroutine to gradually rotate hologram to a fixed Quaternion rotation in given time (not instantly)
    private IEnumerator RotateMe(GameObject holo, Quaternion rotation, float inTime)
    {
        // Iterate over small time intervals untill "inTime" amount of time has passed
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            // Interpolate hologram's rotation towards required value for a small amount respective of time interval t
            if (holo != null) holo.transform.rotation = Quaternion.Slerp(holo.transform.rotation, rotation, t);
            yield return null;
        }

        // Ensure that the final rotation of the hologram is set to the required value
        if (holo != null) holo.transform.rotation = rotation;
    }

    // Custom Coroutine to gradually rotate around an object across a given axis through a pivot by 90 degrees (not instantly)
    private IEnumerator RotateAround(GameObject obj, Transform pivot, Vector3 axis, float inTime)
    {
        float angle = 0; // Variable to keep track of angle rotated by the object

        // Iterate over small time intervals untill "inTime" amount of time has passed
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            float delta_angle = 90f * Time.deltaTime / inTime; // Calculate the small angle to be rotated in time t
            obj.transform.RotateAround(pivot.position, axis, delta_angle); // Rotate object by small angle
            angle += delta_angle; // Increase the variable by this small angle
            yield return null;
        }

        // Rotate the object by the appropriate angle if it undershoots or overshoots 90 degrees
        obj.transform.RotateAround(pivot.position, axis, 90f - angle);

        // Ensure that all 3 axis rotations on the object are multiples of 90 to keep it alligned with the principle axes
        obj.transform.eulerAngles = new Vector3(
            ClosestMultipleOf90(obj.transform.rotation.eulerAngles.x), 
            ClosestMultipleOf90(obj.transform.rotation.eulerAngles.y), 
            ClosestMultipleOf90(obj.transform.rotation.eulerAngles.z)
            );
    }

    // Update is called once per frame
    void Update()
    {
        if (!HoloVisible) // When the hologram is not visible
        {
            // Only allow hologram preview when the player is grounded and the Gave Over screen is not active
            if (thirdPersonController.Grounded && !gameOverSceen.isActiveAndEnabled)
            {
                // If any arrow key is pressed, create the hologram at the appropriate place
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    holo = Instantiate(HoloPrefab);
                    holo.transform.position = transform.position;
                    holo.transform.rotation = transform.rotation;
                    HoloVisible = true;
                }
            }
        } else // When the hologram is visible
        {
            var direction = Quaternion.identity; // Variable to store the direction towards which the hologram should be rotated
            
            // When an Arrow Key is pressed, rotate the hologram towards the appropriate direction
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Quaternion.Euler(90, ClosestMultipleOf90(Camera.transform.rotation.eulerAngles.y), 180);
                StartCoroutine(RotateMe(holo, direction, HoloRotationTime)); // Start the custom coroutine for smooth rotation
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Quaternion.Euler(270, ClosestMultipleOf90(Camera.transform.rotation.eulerAngles.y), 180);
                StartCoroutine(RotateMe(holo, direction, HoloRotationTime));// Start the custom coroutine for smooth rotation
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Quaternion.Euler(180, ClosestMultipleOf90(Camera.transform.rotation.eulerAngles.y), 270);
                StartCoroutine(RotateMe(holo, direction, HoloRotationTime));// Start the custom coroutine for smooth rotation
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Quaternion.Euler(180, ClosestMultipleOf90(Camera.transform.rotation.eulerAngles.y), 90);
                StartCoroutine(RotateMe(holo, direction, HoloRotationTime));// Start the custom coroutine for smooth rotation
            }

            // When the Enter key is pressed, select the hologram direction and rotate the world around the player by
            // 90 degrees in the opposite direction. This creates the illusion of changing gravity and lets us keep the
            // code for Movement, jumping, camera controls and hologram direction calculations simplified.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Set the player gravity to zero momentarily
                thirdPersonController.Gravity = 0f;

                // Set the SwitchingGravity animation state
                thirdPersonController.SetSwitchingGravity();

                // Iterate over all objects that are part of the world
                GameObject[] world_objects = GameObject.FindGameObjectsWithTag("World");
                foreach (GameObject obj in world_objects)
                {
                    // Start the custom coroutine for smooth rotation
                    StartCoroutine(RotateAround(obj, transform, Vector3.Cross(holo.transform.up, transform.up).normalized, SwitchingTime));
                }

                // Stop the SwitchingGravity animation state
                thirdPersonController.Invoke("UnsetSwitchingGravity", SwitchingTime);

                // Destroy the hologram
                Destroy(holo); 
                holo = null;
                HoloVisible = false;

                // Reset the player gravity after the world has rotated
                thirdPersonController.Invoke("ResetGravity", SwitchingTime);
            }

            // Cancel the change of gravity if Backspace is pressed
            if(Input.GetKeyDown(KeyCode.Backspace))
            {
                Destroy(holo);
                holo = null;
                HoloVisible = false;
            }
        }
    }
}
