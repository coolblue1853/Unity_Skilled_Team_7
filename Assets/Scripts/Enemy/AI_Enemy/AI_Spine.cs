using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Spine : AI_Base
{
    protected override void Start()
    {
        base.Start();
        Setting();
    }



    public override void Attack(GameObject target)
    {

    }

    protected override void Setting()
    {
        var idle = new AIIdleState(gameObject);
        var chase = new AIChasingState(gameObject, TestGameManager.Inst.testPlayer.transform);
        var attack = new AIAttackState(gameObject);
        var overWall = new AIOverWallState(gameObject, TestGameManager.Inst.testPlayer.transform);
        var dead = new AIDeadState(gameObject);

        fsm.SetInitialState(idle);

        fsm.AddTransition(idle, chase, () => Vector3.Distance(transform.position, TestGameManager.Inst.testPlayer.transform.position) < enemyData.chasingRange);
        //fsm.AddTransition(idle, chase, () => Vector3.Distance(transform.position, player.transform.position) < enemyData.attackRange);

        fsm.AddTransition(chase, idle, () => Vector3.Distance(transform.position, TestGameManager.Inst.testPlayer.transform.position) > enemyData.chasingRange);
        fsm.AddTransition(chase, overWall, () => Vector3.Distance(transform.position, chase.CurrentTarget.position) < enemyData.attackRange);

        fsm.AddTransition(overWall, idle, () => overWall.IsClimbComplete);
        //fsm.AddTransition(attack, chase, () => Vector3.Distance(transform.position, player.transform.position) > attackRange);
        fsm.AddAnyTransition(dead, () => GetComponent<Health>().IsDead);

    }
}
