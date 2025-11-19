using System.Collections;
using System.Collections.Generic;
using DavidJalbert.LowPolyPeople;
using UnityEditor.VisionOS;
using UnityEngine;
using UnityEngine.AI;

public class MonsterLogic : MonoBehaviour
{
    public Animator monsterAnim;
    public NavMeshAgent monsterAgent;
    public MonsterVision monsterVision;
    public GameObject head;
    public Transform mapCenter;         // 랜덤 이동 중심 기준점

    [Header("Monster Move Settings")]
    public float moveSpeed;
    public float turnDuration;
    public float roamRadius;
    public float spendTime { get; private set; }
    [Header("Monster Look Settings")]
    public float headLookAngleX = 70f;
    public float headLookAngleY = 40f;
    public float headLookSpeed;       // 초당 회전 속도(도)
    public float minGlanceInterval;   // 최소 힐끗거림 간격
    public float maxGlanceInterval;   // 최대 힐끗거림 간격
    [Header("Monster Stuck Settings")]
    public float stuckSpeedEps = 0.05f; // 멈춰있는 것으로 간주하는 속도 임계값
    public float stuckTime = 0.5f;     // 멈춰있는 것으로 간주하는 시간
    // get은 public으로 어디서나 값을 읽을 수 있지만, set은 private으로 이 클래스 내에서만 값을 변경할 수 있음
    public bool IsMonsterTurn { get; private set; }
    public bool FoundPlayer { get; private set; }
    public bool TurnFinished { get; private set; }
    Quaternion headBaseLocalRot;
    Quaternion headTargetLocalRot;
    Coroutine turnRoutine;

    void Awake()
    {
        monsterAgent = GetComponent<NavMeshAgent>();
        monsterVision = GetComponent<MonsterVision>();
        monsterAnim = GetComponent<Animator>();
        // 머리의 초기 로컬 회전값 저장 (Quaternion.identity = 기본 회전값으로 (0,0,0)을 의미)
        headBaseLocalRot = head != null ? head.transform.localRotation : Quaternion.identity;
        headTargetLocalRot = headBaseLocalRot;
    }

    void LateUpdate()
    {
        // Animator가 뼈를 다 움직인 *다음에* 우리가 원하는 각도로 덮어쓰기
        if (head != null)
        {
            head.transform.localRotation = headTargetLocalRot;
        }
    }

    public void StartMonsterTurn(float duration)
    {
        if (IsMonsterTurn) return; // 이미 몬스터 턴이면 무시
        turnDuration = duration;
        FoundPlayer = false;
        TurnFinished = false;
        IsMonsterTurn = true;

        monsterAgent.speed = moveSpeed;
        monsterAgent.isStopped = false;

        turnRoutine = StartCoroutine(MonsterTurnRoutine());
    }
    
    public void ForceStopTurn()
    { // 몬스터 턴 강제 종료
        if (turnRoutine != null) StopCoroutine(turnRoutine);
        monsterAgent.isStopped = true;
        IsMonsterTurn = false;
    }

    IEnumerator MonsterTurnRoutine()
    {
        spendTime = 0f;
        float nextGlanceTime = RandomGlanceTime(Time.time);
        float stuckTimer = 0f;
        Vector3 lastPosition = transform.position;
        // 랜덤 목적지 설정
        SetRandomDestination();
        monsterAnim.SetBool("Walk", true);
        while (spendTime < turnDuration)
        {
            Debug.Log(spendTime);
            if (FoundPlayer) break;
            // 플레이어 발견 여부 갱신
            if (!FoundPlayer && monsterVision != null && monsterVision.CanSeePlayer())
            {
                // 플레이어 발견 시 즉시 이동 중지
                FoundPlayer = true;
                monsterAgent.isStopped = true;
                break;
            }
            // 막혀있는지 확인
            float moved = Vector3.Distance(transform.position, lastPosition);
            // monsterAgent.velocity.magnitude = 현재 속도 크기
            if (moved < 0.01f && monsterAgent.velocity.magnitude < stuckSpeedEps)
            {
                stuckTimer += Time.deltaTime;
            }
            else
            {
                stuckTimer = 0f;
                lastPosition = transform.position;
            }
            if (stuckTimer >= stuckTime)
            {
                // 멈춰있으면 새로운 목적지 설정
                SetRandomDestination();
                stuckTimer = 0f;
            }

            // 목적지에 도달했으면 새로운 목적지 설정
            if (!monsterAgent.pathPending && monsterAgent.remainingDistance <= monsterAgent.stoppingDistance + 0.1f)
            {
                SetRandomDestination();
            }

            // 머리 힐끗거림 처리
            if (Time.time >= nextGlanceTime)
            {
                yield return StartCoroutine(LookLeftOrRightRoutine());
                nextGlanceTime = RandomGlanceTime(Time.time);
            }
            spendTime += Time.deltaTime;
            yield return null;
        }
        monsterAnim.SetBool("Walk", false);
        
        // 턴 종료시 플레이어를 발견하지 못했을 경우
        if (!FoundPlayer)
        {
            monsterAgent.isStopped = true;
            yield return StartCoroutine(FinalScanRoutine());
        }
        // 턴 종료
        TurnFinished = true;
        IsMonsterTurn = false;
        Debug.Log("몬스터 턴 종료");
    }

    void SetRandomDestination()
    {
        Vector3 center = mapCenter != null ? mapCenter.position : transform.position;
        // 10번 시도하여 유효한 목적지 설정
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomDirection = Random.insideUnitSphere * roamRadius;
            Vector3 direction = center + new Vector3(randomDirection.x, 0, randomDirection.y);
            if (NavMesh.SamplePosition(direction, out NavMeshHit hit, 1.5f, NavMesh.AllAreas))
            {
                if (monsterAgent.SetDestination(hit.position)) return;
            }
        }
        monsterAgent.SetDestination(center); // 실패 시 중심으로 이동
    }

    float RandomGlanceTime(float from)
    {
        return from + Random.Range(minGlanceInterval, maxGlanceInterval);
    }

    IEnumerator LookLeftOrRightRoutine()
    {
        if (head == null) yield break;

        // 왼쪽 또는 오른쪽으로 힐끗거림
        float targetAngleX = Random.value > 0.5f ? headLookAngleX : -headLookAngleX;
        float targetAngleY;
        if (targetAngleX > 0) targetAngleY = -headLookAngleY;
        else targetAngleY = headLookAngleY;

        Quaternion start = head.transform.localRotation;
        Quaternion target = headBaseLocalRot * Quaternion.Euler(targetAngleX, targetAngleY, -40f);

        // 머리 회전 코루틴 실행
        yield return RotateHeadRoutine(start, target);
        if (FoundPlayer) yield break;
        // 잠시 대기
        yield return new WaitForSeconds(0.2f);
        // 머리 원위치 회전 코루틴 실행
        yield return RotateHeadRoutine(head.transform.localRotation, headBaseLocalRot);
        if (FoundPlayer) yield break;
    }

    IEnumerator RotateHeadRoutine(Quaternion from, Quaternion to)
    {
        // 각도 차이 / 속도 = 대략 걸리는 시간
        float angleDiff = Quaternion.Angle(from, to);
        float duration = angleDiff / headLookSpeed;     // 초당 도 단위 속도로 계산
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // 목표하는 회전 값 갱신
            headTargetLocalRot = Quaternion.Slerp(from, to, t);

            if (!FoundPlayer && monsterVision != null && monsterVision.CanSeePlayer())
            {
                // 플레이어 발견 시 즉시 이동 중지
                FoundPlayer = true;
                monsterAgent.isStopped = true;
                yield break;
            }
            yield return null;
        }
        head.transform.localRotation = to;
    }
    
    IEnumerator FinalScanRoutine()
    {
        if (head == null) yield break;

        Quaternion center = headBaseLocalRot;
        Quaternion left = headBaseLocalRot * Quaternion.Euler(headLookAngleX, -headLookAngleY, -30f);
        Quaternion right = headBaseLocalRot * Quaternion.Euler(-headLookAngleX, headLookAngleY, -30f);

        // 왼쪽으로 힐끗
        yield return RotateHeadRoutine(center, left);
        if (FoundPlayer) yield break;

        // 오른쪽으로 힐끗
        yield return RotateHeadRoutine(left, right);
        if (FoundPlayer) yield break;

        // 중앙으로 복귀
        yield return RotateHeadRoutine(right, center);
    }
}