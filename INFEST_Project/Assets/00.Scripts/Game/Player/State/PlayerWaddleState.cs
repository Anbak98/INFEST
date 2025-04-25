using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWaddleState : PlayerSitState
{
    public PlayerWaddleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 4; // �ȴ� �ӵ��� 0.5��
        Debug.Log("Waddle���� ����");
        base.Enter();

        /// blend tree �ִϸ��̼ǿ� ����
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, stateMachine.InputHandler.MoveInput.x);
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, stateMachine.InputHandler.MoveInput.y);
    }
    public override void Exit()
    {
        base.Exit();
        // ���� �ʱ�ȭ
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        Vector2 moveInput = data.direction;

        // ���������� Blend Tree �Ķ���� ������Ʈ
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        // �÷��̾� �̵�
        PlayerWaddle(data);
        //controller.ApplyGravity();  // �߷�

        if (!data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        // isSitting 
        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
        // isSitting && isFiring
        if ((stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.SitAttackState);
            return;
        }
        // isSitting && isReloading
        if ((stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.SitReloadState);
            return;
        }
    }
}
