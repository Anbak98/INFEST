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
        Debug.Log("Jump���� ����");
        base.Enter();
        //StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
        // ������ �ѹ���
        PlayerJump();

        // OnUpdate���� controller.IsGrounded()�� ���� ������ ���������� true�� �����ϴ� ������ �־ �Ű��
        //stateMachine.ChangeState(stateMachine.IdleState);
        // PlayerJump���ڸ��� ���� ������� IdleState�� �����ϰ�
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        // �÷��̾� �̵�
        PlayerMove(data);
        controller.ApplyGravity();  // �߷�

        // y���� �ӵ��� ������ fallstate�� �ٲپ���Ѵ�(�̰� �ϴ� ���߿�

        // ���� �������µ� controller.IsGrounded()�� true�� �����ϴ� ���� ������
        // �ٸ� ������ �߰��Ͽ� ������ ���� �������� �� false�� �Ǿ���Ѵ�
        // ���� ���� �������� false�� �Ǵ°� �ֱ׷�����???
        //Debug.LogFormat($"NetworkCharacterController.IsGrounded: {controller.IsGrounded()}");
        //if (controller.IsGrounded())
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}
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
