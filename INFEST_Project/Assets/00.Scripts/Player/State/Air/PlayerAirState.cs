using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���鿡 ���� ������ ����
// Jump, Fall
public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public PlayerLockState LockState = PlayerLockState.ZoomLock;

    public override void Enter()
    {
        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
    }
    public override void Exit()
    {
        base.Exit();    // ����� layer�� ������
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
