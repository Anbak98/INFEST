using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

// 땅에 붙어있는 모든 상태 공통 적용
// Idle, Move, Run, Attack, Reload
public class PlayerGroundState : PlayerBaseState
{
    // 가만히 있는 플레이어의 forward를 저장
    Vector3 prevForward;

    public PlayerGroundState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.lookDelta = data.lookDelta;
        //player.animationController.look= data.look;

        // 플레이어의 카메라에 접근하여 카메라의 forward와 플레이어의 forward의 사이각을 비교하여 90도면 1이 되어야하니까
        Vector3 camForward = player.cameraHandler.transform.forward;
        Vector3 playerForward = player.transform.forward;

        // 두 벡터 간의 각도
        float angleY = Vector3.Angle(camForward, playerForward);  // 0 ~ 180

        // angle이 0도이면 정면, 90도이면 수직, 180도면 반대 방향
        // 그런데 여기에 부호를 붙여야 한다! (내적을 이용하여 위/아래 방향 구분)
        float dotY = Vector3.Dot(camForward, Vector3.up); // y축 방향 내적을 구하면 위쪽일 때 1, 아래쪽일 때 -1


        if (data.lookDelta.x != 0f)
        {
            // player의 forward를 저장한다
            prevForward = player.transform.forward;
        }
        // 그 forward.x와 나중의 player의 forward.x를 내적해야한다
        //float dotX = Vector3.Dot(Vector3.forward, playerForward); 
        
        // 애니메이터에 값전달
        player.animationController.lookDelta = new Vector2(data.lookDelta.x, dotY);
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
