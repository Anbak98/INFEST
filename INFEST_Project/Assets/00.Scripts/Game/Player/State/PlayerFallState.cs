using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }



    public override void Enter()
    {
        // ���鿡�� �������� && y���� �ӵ��� 0���� �۰ų� ������ Fall


        base.Enter();
    }
    public override void Update()
    {
        // ���� ���� ������(IsGrounded == true) �߷��� �޴´�

    }

    public override void Exit()
    {
        base.Exit();
    }
}
