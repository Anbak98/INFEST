using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // �ϴ� ���ڴ���. ���߿� PlayStatData.RunSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 6;
        Debug.Log("Run���� ����");
        base.Enter();
        // Run�� Move�� ������� �ؾ��ϴµ�... Move�� ���¸� ������� Run �Ķ���͸� �߰��Է��ؾ��Ѵ�
        // ��¿��?
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void OnUpdate()
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        Vector2 moveInput = stateMachine.InputHandler.MoveInput;

        // ���������� Blend Tree �Ķ���� ������Ʈ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        PlayerRun();
        controller.ApplyGravity();  // �߷�

        // �̵� �Է��� ���� �� Idle�� �ٲٸ� Lshift�� ������ �ִ� ä ������ �ϰ� �ȴ�
        //if (moveInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}
        // run �ٽ� �����ٸ� �ȱ�
        if (!stateMachine.InputHandler.GetIsRunning())
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        // run�϶��� ������ �� ����
        //else if (stateMachine.InputHandler.GetIsJumping())
        //{
        //    stateMachine.ChangeState(stateMachine.JumpState);
        //}
        // run�϶��� ���ݻ��·� ��ȯ�� �� ����
    }

    // Run������ Idle, Move(���� �ۼ����� �ʾҴµ� ���߿� ��������) ���� ������ȯ �����ϴ�
    protected override void AddInputActionsCallbacks()
    {
    }
    protected override void RemoveInputActionsCallbacks()
    {
    }


}
