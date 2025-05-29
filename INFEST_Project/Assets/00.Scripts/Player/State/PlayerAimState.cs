using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ThirdPersonRoot�� Aim �ִϸ��̼��� ����
// FirstPersonRoot�� ������ ������, Sit�� Stand�� ������ ����
public class PlayerAimState : PlayerGroundState
{
    public PlayerAimState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // �����߿��� �޸���, ���� �Ұ�
        controller.LockState = PlayerLockState.RunLock | PlayerLockState.JumpLock;

        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
    }
}
