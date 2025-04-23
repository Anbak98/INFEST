using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadState : PlayerGroundState
{
    private bool prevIsReloading = false;
    public PlayerReloadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        Debug.Log("Reload");
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }



    public override void OnUpdate(NetworkInputData data)
    {
        PlayerMove(data);
        if (!prevIsReloading && data.isReloading)
        {
            Reload();
            Reload(data);
        }
        if (!data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        prevIsReloading = data.isReloading;
    }
}
