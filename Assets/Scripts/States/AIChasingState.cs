using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.UI.Image;

public class AIChasingState : AIState
{
    private NavMeshAgent agent;
    public Transform CurrentTarget { get; set; }
    private EENEMYTYPE type;
    private float wallDetectDistance;
    private bool hasAssignedWallOffset = false;
    private Vector3 targetOffsetPosition;
    public AIChasingState(GameObject owner, Transform target) : base(owner)
    {
        SO_EnemyAI data = owner.GetComponent<AI_Base>().enemyData;
        type = data.type;

        if (type == EENEMYTYPE.RUNNER || type == EENEMYTYPE.SPINE)
        {
            wallDetectDistance = data.wallDetectDistance;
        }

       // this.target = target;
        this.agent = owner.GetComponent<NavMeshAgent>();

     
    }

    public override void Enter()
    {
        hasAssignedWallOffset = false;
        agent.isStopped = false;
        Debug.Log("Chasing");
    }
    public override void Tick()
    {
        if ((type == EENEMYTYPE.RUNNER || type == EENEMYTYPE.SPINE) && IsWallInFront(out Transform wall))
        {
            if (CurrentTarget != wall || !hasAssignedWallOffset)
            {
                CurrentTarget = wall;
                targetOffsetPosition = GetOffsetWallPosition(wall);
                agent.SetDestination(targetOffsetPosition);
                hasAssignedWallOffset = true;
                return;
            }
        }

        if (CurrentTarget == null || IsDead(CurrentTarget))
        {
            CurrentTarget = SelectTarget(type);
            hasAssignedWallOffset = false;

            if (CurrentTarget != null)
            {
                // targetOffsetPosition도 업데이트
                targetOffsetPosition = CurrentTarget.position;
                agent.SetDestination(targetOffsetPosition);
            }
        }

        if (CurrentTarget != null)
        {
            agent.SetDestination(targetOffsetPosition); // 항상 offset된 위치로
        }
    }


    public override void Exit()
    {
        //agent.isStopped = true;
    }
    Transform SelectTarget(EENEMYTYPE type)
    {

        // 플레이어 기본 타겟
        Transform player = TestGameManager.Inst.testPlayer.transform;

        if (type == EENEMYTYPE.RUNNER || type == EENEMYTYPE.SPINE)
        {
            if (IsWallInFront(out Transform wall))
            {
                return wall;
            }
        }
       
        if (type == EENEMYTYPE.STEALER)
        {
            if (DroneManager.HasAliveDrones)
                return DroneManager.ClosestDrone(owner.transform.position);

            Transform building = DroneManager.ClosestDroneBuilding(owner.transform.position);
            if (building != null)
                return building;
        }


        return player;
    }

    bool IsWallInFront(out Transform wall)
    {
        Vector3 origin = owner.transform.position + Vector3.up * 0.5f;
        Vector3 dir = owner.transform.forward;

        int wallLayerMask = LayerMask.GetMask("Wall");
      
        Collider[] hits = Physics.OverlapSphere(origin, wallDetectDistance, wallLayerMask);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Wall"))
            {
                wall = hit.transform;
                return true;
            }
        }
       
        wall = null;
        return false;
    }
    bool IsDead(Transform CurrentTarget)
    {
        return CurrentTarget == null || !CurrentTarget.gameObject.activeInHierarchy;
    }
    Vector3 GetOffsetWallPosition(Transform wall)
    {
        Vector3 wallPosition = wall.position;
        Vector3 right = owner.transform.right;

        // 좌우 랜덤 오프셋
        float offsetRange = 5.0f; // 퍼짐 범위 설정 (원하면 조절 가능)
        float randomOffset = Random.Range(-offsetRange, offsetRange);

        // offset 적용
        Vector3 offsetPosition = wallPosition + right * randomOffset;

        return offsetPosition;
    }

}
