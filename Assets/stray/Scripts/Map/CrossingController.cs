using UnityEngine;
using System.Collections.Generic;

public class CrossingController : MonoBehaviour
{
    [Header("Connected Doors")]
    [Tooltip("Assign up to 3 DoorControl objects.")]
    [SerializeField] private List<DoorControl> doors = new List<DoorControl>();

    private int currentDoorIndex = 0;

    // Public method to be called by PlayerControl
    public void Interact()
    {
        if (doors == null || doors.Count == 0) return;

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
