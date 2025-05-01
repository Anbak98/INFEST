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
        //Debug.Log("Air���� ����");
        base.Enter();

        //controller.SetGrounded(false);
        //StartAnimation(stateMachine.Player.AnimationData.AimParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // ����� layer�� ������
        //StopAnimation(stateMachine.Player.AnimationData.AimParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
