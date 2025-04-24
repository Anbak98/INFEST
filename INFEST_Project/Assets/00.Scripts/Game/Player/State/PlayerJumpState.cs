using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        //Debug.Log("Jump���� ����");
        base.Enter();
        //StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // ������ �ѹ���
        PlayerJump();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        // �÷��̾� �̵�
        PlayerMove(data);
        controller.ApplyGravity();  // �߷�

        // �ӵ��� �ش��ϴ� float ������ �����ؼ� ���� ó���ؾ���
        if (controller.GetVerticalVelocity() <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // ������ ������ ���� ���·� ���ư����Ѵ�
    }

}
