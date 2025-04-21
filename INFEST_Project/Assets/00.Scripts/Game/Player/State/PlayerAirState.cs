using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���鿡 ���� ������ ����
// Jump, Fall
public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Air���� ����");
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.AimParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // ����� layer�� ������
        StopAnimation(stateMachine.Player.AnimationData.AimParameterHash);
    }

    public override void Update()
    {
    }
    public override void PhysicsUpdate()
    {
    }
}
