using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRandomRotateTest : MonoBehaviour
{
    public static WallRandomRotateTest Instance;
    public List<GameObject> moveWalls;

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
    }

    public void RandomRotateWalls()
    {
        foreach (GameObject wall in moveWalls)
        {
            float rate = Random.Range(0f, 10f);
            float randomYRotation;
            if (rate > 5f)
            {
                randomYRotation = 90f;
            }
            else
            {
                randomYRotation = 0f;
            }
            wall.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);
        }
    }
}
