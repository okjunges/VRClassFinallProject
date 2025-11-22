using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterVision : MonoBehaviour
{
    [Header("Seeker Vision Settings")]
    public Camera monsterCamera;
    public Transform playerTransform;
    public LayerMask wallLayerMask;
    public float visionRange;

    void Start()
    {
        monsterCamera = GameObject.FindWithTag("MonsterCamera").GetComponent<Camera>();
        visionRange = monsterCamera.farClipPlane;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public bool CanSeePlayer()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (monsterCamera == null || playerTransform == null) return false;

        Vector3 target = playerTransform.position + Vector3.up * 1.5f;

        // 카메라 view frustrum 내에 있는지 확인
        Vector3 vp = monsterCamera.WorldToViewportPoint(target);

        bool inFront = vp.z > 0f;
        bool inViewport = vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f;
        if (!inFront || !inViewport) return false;
        // 시야 거리 내에 있는지 확인
        if (vp.z > visionRange) return false;

        // 벽에 가려져 있는지 확인
        Vector3 directionToPlayer = (target - monsterCamera.transform.position).normalized;
        if (Physics.Raycast(monsterCamera.transform.position, directionToPlayer, out RaycastHit hitInfo, visionRange))
        {
            if (((1 << hitInfo.collider.gameObject.layer) & wallLayerMask) != 0) return false; // 벽에 가려져 있음
            if (hitInfo.transform.CompareTag("Player") || hitInfo.transform == playerTransform) {
                Debug.Log("플레이어를 찾았습니다!!");
                return true; // 플레이어가 보임
            }
        }
        return false;
    }
}