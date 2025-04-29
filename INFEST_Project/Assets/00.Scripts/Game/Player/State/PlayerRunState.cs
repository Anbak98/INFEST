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
        stateMachine.StatHandler.MoveSpeedModifier = 8;
        Debug.Log("Run���� ����");
        base.Enter();
        // Run�� Move�� ������� �ؾ��ϴµ�... Move�� ���¸� ������� Run �Ķ���͸� �߰��Է��ؾ��Ѵ�
        // ��¿��?
        //StartAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        //Vector3 moveInput = data.direction;

        // ���������� Blend Tree �Ķ���� ������Ʈ
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        //SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.z);


        PlayerRun(data);
        //controller.ApplyGravity();  // �߷�

        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }
        // run �ٽ� �����ٸ� �ȱ�
        if (!data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }
    }
}
