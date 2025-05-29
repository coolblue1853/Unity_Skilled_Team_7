using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Steel : AI_Base
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Setting();
    }
    protected override void Setting()
    {
        var idle = new AIIdleState(gameObject);
        var chase = new AIChasingState(gameObject, TestGameManager.Inst.testPlayer.transform);
        var attack = new AIAttackState(gameObject);
        var dead = new AIDeadState(gameObject);

       // fsm.AddTransition(idle, chase, () => Vector3.Distance(transform.position, player.transform.position) < enemyData.chasingRange);
        fsm.AddTransition(idle, chase, () =>
        {
            if (DroneManager.HasAliveDrones)
                return true;

            return Vector3.Distance(transform.position, TestGameManager.Inst.testPlayer.transform.position) < enemyData.chasingRange;
        });

        fsm.AddAnyTransition(dead, () => GetComponent<Health>().IsDead);

        fsm.SetInitialState(idle);
    }
    public override void Attack(GameObject target)
    {

    }
}
