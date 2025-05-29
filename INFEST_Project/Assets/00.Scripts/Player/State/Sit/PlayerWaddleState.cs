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
        Debug.Log("Waddle상태 진입");
        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.MoveDirection = data.direction;
        player.animationController.isSitting = data.isSitting;
    }
    public override void Exit()
    {
        base.Exit();
    }

}
