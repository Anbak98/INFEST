using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaddleState : PlayerSitState
{
    public PlayerWaddleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 1; // �ȴ� �ӵ��� 0.5��
        Debug.Log("Waddle���� ����");
        base.Enter();


        /// blend tree �ִϸ��̼ǿ� ����
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, stateMachine.InputHandler.MoveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, stateMachine.InputHandler.MoveInput.y);
    }
    public override void Exit()
    {
        base.Exit();
        // ���� �ʱ�ȭ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void Update()
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        Vector2 moveInput = stateMachine.InputHandler.MoveInput;

        // ���������� Blend Tree �Ķ���� ������Ʈ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        // �÷��̾� �̵�
        PlayerWaddle();
        controller.ApplyGravity();  // �߷�


        if (!stateMachine.InputHandler.GetIsSitting())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        //// �̵� �Է��� ������ Sit ���·�
        //if (moveInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
        //// ���⸦ ���� ���¿��� ���ݹ�ư �����ٸ� ���ݻ���
        //else if ((stateMachine.Player.GetWeapon() != null) && stateMachine.InputHandler.GetIsFiring())
        //{

        //}
        //// ���⸦ ���� ���¿��� �Է��� ������ Reload ���·�
        //else if ((stateMachine.Player.GetWeapon() != null) && stateMachine.InputHandler.GetIsReloading())
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
        //// Idle�� ��ȯ
        //else if (!stateMachine.InputHandler.GetIsSitting())
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}

    }

    protected override void AddInputActionsCallbacks()
    {
    }
    protected override void RemoveInputActionsCallbacks()
    {
    }
}
