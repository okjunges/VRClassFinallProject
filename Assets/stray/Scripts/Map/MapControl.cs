using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public static MapControl Instance { get; private set; }

    [SerializeField] private List<CrossingController> crossings = new List<CrossingController>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        InitializeMap();
    }

    public void InitializeMap()
    {
        // Find all CrossingControllers in the scene
        crossings.Clear();
        CrossingController[] foundCrossings = FindObjectsOfType<CrossingController>();
        crossings.AddRange(foundCrossings);
        
        Debug.Log($"MapControl Initialized: Found {crossings.Count} crossings.");
    }

    public void InteractWith(GameObject triggerObject)
    {
        if (triggerObject == null) return;

        CrossingController controller = triggerObject.GetComponent<CrossingController>();
        if (controller != null && crossings.Contains(controller))
        {
            // Perform interaction via MapControl
            controller.Interact();
        }
        else
        {
            Debug.LogWarning($"Interaction failed: {triggerObject.name} is not a registered crossing.");
        }
    }
}
