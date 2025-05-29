using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDeadState : AIState
{
    public AIDeadState(GameObject owner) : base(owner)
    {
    }

    public override void Enter()
    {
        Debug.Log("Dead State");
        owner.GetComponent<NavMeshAgent>().isStopped = true;
    }
    public override void Tick()
    {

    }

    public override void Exit()
    {
       // 풀로 돌아가게
    }

 


}
