using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using Unity.AI.Navigation;

public class BakeNewMap : MonoBehaviour
{
    public static BakeNewMap Instance;
    public NavMeshSurface surface;

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
        surface.BuildNavMesh();
    }
}
