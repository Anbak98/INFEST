using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }


    public override void Enter()
    {
        // ���鿡�� �������� && y���� �ӵ��� 0���� �۰ų� ������ Fall

        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        // false�� �� ���¿��߸� �Ѵ�(����, ���� �� ����Ű �Է�X)
        player.animationController.isJumping = data.isJumping;

        base.OnUpdate(data);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
