using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ThirdPersonRoot�� Attack �ִϸ��̼��� ����
// FirstPersonRoot�� �ִϸ��̼��� ������ ������ 
// ��� ���¿��� Attack�� �����ϴϱ� DeadState�� ���������� ���� ó���Ѵ�
public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.isFiring = data.isFiring;
    }
}
