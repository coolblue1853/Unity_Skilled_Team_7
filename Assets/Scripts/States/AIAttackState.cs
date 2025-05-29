using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAttackState : AIState
{
    private GameObject target;
    private NavMeshAgent agent;
    private float attackCooldown;
    private float lastAttackTime = Mathf.NegativeInfinity; // 마지막 공격 시간
    public GameObject CurrentTarget => target;
    public AIAttackState(GameObject owner) : base(owner)
    {
    }

    public override void Enter()
    {
        SetTarget(target);
        Debug.Log("Attack");
        agent = owner.GetComponent<NavMeshAgent>();
        agent.isStopped = true;

        SO_EnemyAI data = owner.GetComponent<AI_Base>().enemyData;
        attackCooldown = data.attackCoolTime;

  
        lastAttackTime = Time.time - attackCooldown;
    }
    public override void Tick()
    {
        if (target == null || !target.activeInHierarchy)  return; 

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            owner.GetComponent<AI_Base>().Attack(target); // Base 클래스 공격 호출
        }
    }

    public override void Exit()
    {
        agent.isStopped = false;
        target = null;   
    }
    public void SetTarget(GameObject t)
    {
        target = t;
    }



}
