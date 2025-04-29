using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimAttackState : PlayerGroundState
{
    public PlayerAimAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {

    }


}
