using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앉아 있는 상태
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerSitState : PlayerGroundState
{
    public PlayerSitState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // collider의 크기를 절반으로 줄인다
        player.characterController.height /= 2;        

        //Debug.Log("Sit상태 진입");
        base.Enter();
        
        // 앉는다
        controller.StartSit();
    }
    public override void Exit()
    {
        // collider의 크기를 2배로 늘린다


        base.Exit();
        // 일어난다
        controller.StartStand();

        player.characterController.height *= 2;
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // 앉아 있는 경우에 Layer를 변경


        if (!data.isSitting)
        {
            // 다시 Layer를 Base로 만든다


            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
