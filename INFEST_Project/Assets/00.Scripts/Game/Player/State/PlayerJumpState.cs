using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerController controller, PlayerStateMachine stateMachine, InputManager inputManager) : base(controller, stateMachine, inputManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Jump���� ����");
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // ������ �ѹ���
        PlayerJump();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        // �÷��̾� �̵�
        PlayerMove();
        controller.ApplyGravity();  // �߷�

        // y���� �ӵ��� ������ fallstate�� �ٲ���Ѵ�
        // ���� ���� ������(IsGrounded == true) �߷��� �޴´�        

        if (controller.IsGrounded())
        {            
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // ������ ������ ���� ���·� ���ư����Ѵ�
    }

}
