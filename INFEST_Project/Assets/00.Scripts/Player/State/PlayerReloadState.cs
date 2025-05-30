using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadState : PlayerBaseState
{
    //private bool prevIsReloading = false;

    public PlayerReloadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {

    }
    public override void Enter()
    {
        Debug.Log("Reload상태 진입");
        base.Enter();

        //StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
    public override void OnUpdate(NetworkInputData data)
    {
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }


}
