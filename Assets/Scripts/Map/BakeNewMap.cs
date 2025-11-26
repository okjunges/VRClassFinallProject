using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using Unity.AI.Navigation;

public class BakeNewMap : MonoBehaviour
{
    public static BakeNewMap Instance;
    public NavMeshSurface surface;
    [SerializeField] private GameObject currentMapInstance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        surface = GetComponent<NavMeshSurface>();
    }

    public void BakeNow()
    {
        currentMapInstance = GameObject.FindWithTag("Map");
        surface = currentMapInstance.GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}
