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
        Debug.Log("SitAttack���� ����");
        base.Enter();

    }
    public override void Exit()
    {
        base.Exit();    // ����� layer�� ������

    }
    public override void Update()
    {
    }
    public override void PhysicsUpdate()
    {
    }
}
