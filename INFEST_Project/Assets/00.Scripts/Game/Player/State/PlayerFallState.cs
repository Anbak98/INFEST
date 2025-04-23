using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine, InputManager inputManager) : base(controller, stateMachine, inputManager)
    {
    }



    public override void Enter()
    {
        // ���鿡�� �������� && y���� �ӵ��� 0���� �۰ų� ������ Fall


        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        // ���� ���� ������(IsGrounded == true) �߷��� �޴´�

    }

    public override void Exit()
    {
        base.Exit();
    }
}
