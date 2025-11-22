using UnityEngine;
using System.Collections.Generic;

public class CrossingController : MonoBehaviour
{
    [Header("Connected Doors")]
    [Tooltip("Assign up to 3 DoorControl objects.")]
    [SerializeField] private List<DoorControl> doors = new List<DoorControl>();

    private int currentDoorIndex = 0;

    void Start()
    {
        // Initialize State: Open the first door, close others
        if (doors != null && doors.Count > 0)
        {
            for (int i = 0; i < doors.Count; i++)
            {
                if (doors[i] != null && i == currentDoorIndex)
                {
                    doors[i].Open();
                }
                else if (doors[i] != null)
                {
                    doors[i].Close();
                }
            }
        }
    }

    // Public method to be called by PlayerControl
    public void Interact()
    {
        if (doors == null || doors.Count == 0) return;

        // Case 1: Single Door - Just Toggle
        if (doors.Count == 1)
        {
            if (doors[0] != null)
            {
                doors[0].Toggle();
                Debug.Log("Interacted with Crossing: Toggled Single Door");
            }
            return;
        }

        // Case 2: Multiple Doors - Cycle (Open Next, Close Previous)
        // Calculate previous index (circular)
        int previousDoorIndex = (currentDoorIndex - 1 + doors.Count) % doors.Count;

        // Close previous door
        if (doors[previousDoorIndex] != null)
        {
            doors[previousDoorIndex].Close();
        }

        // Open current door
        if (doors[currentDoorIndex] != null)
        {
            doors[currentDoorIndex].Open();
            Debug.Log($"Interacted with Crossing: Opened Door {currentDoorIndex}, Closed Door {previousDoorIndex}");
        }

        // Move to next door index
        currentDoorIndex = (currentDoorIndex + 1) % doors.Count;
    }
}
