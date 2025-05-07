using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    // ���� ���� ����
    public override void Enter()
    {
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();   // Ground ����
        //Debug.Log("Idle���� ����");

        // Ground && Idle
        //StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
        base.PhysicsUpdate(data);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);


        if ((controller.IsGrounded()) && data.isJumping)
        {
            player.animationController.isJumping = data.isJumping;
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        // ������ ���¿��� �¿�ȸ��
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        // �ϴ� ����(isShotgunOnFiring)�� ���ۼ�
        if (stateMachine.Player.GetWeapons() != null && data.isFiring)
        {
            // ���� frame�� ������ state�� �ٲ� ���� frame�� ������ �ٸ��� ������
            // ���� frame���� _isShotgunOnFiring true�� ���� frame���� false�� �Ǳ� �����̴�
            // ���� �̹� �����ӿ��� 1�� ��� �Ѿ���Ѵ�
            // rifle�� ��쿡 ���������ӿ����� ��� �߻��ϸ�
            // pistol�� ��쿡�� ���� �����ӿ� �߻����� �ʴ´�
            player.animationController.isFiring = data.isFiring;
            PlayerFire(data);

            stateMachine.ChangeState(stateMachine.AttackState);
        }
        if (controller.IsGrounded() && data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }


        // �ɴ� ���´� ���
        //if (controller.IsGrounded() && data.isSitting)
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
    }
}
