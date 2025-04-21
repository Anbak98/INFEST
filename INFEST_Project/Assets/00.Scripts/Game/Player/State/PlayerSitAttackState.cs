using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitAttackState : PlayerSitState
{
    public PlayerSitAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("SitAttack상태 진입");
        base.Enter();

    }
    public override void Exit()
    {
        base.Exit();    // 상단의 layer로 나간다

    }
    public override void Update()
    {
    }
    public override void PhysicsUpdate()
    {
    }
}
