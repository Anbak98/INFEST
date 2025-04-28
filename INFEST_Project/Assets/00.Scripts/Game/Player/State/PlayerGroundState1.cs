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
    // 클래스 상단에 변수 추가
    private float currentLookDeltaX = 0f;
    private float lerpSpeed = 5f; // 보간 속도(높을수록 빠르게 변화)


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
        //player.animationController.lookDelta = data.lookDelta;
        ////Vector2 tmpLookDelta = data.lookDelta;

        //// 플레이어의 카메라에 접근하여 카메라의 forward와 플레이어의 forward의 사이각을 비교하여 90도면 1이 되어야하니까
        //Vector3 camForward = player.cameraHandler.transform.forward;
        //Vector3 playerForward = player.transform.forward;

        //// 두 벡터 간의 각도
        //float angleY = Vector3.Angle(camForward, playerForward);  // 0 ~ 180

        //// angle이 0도이면 정면, 90도이면 수직, 180도면 반대 방향
        //// 그런데 여기에 부호를 붙여야 한다! (내적을 이용하여 위/아래 방향 구분)
        //float dotY = Vector3.Dot(camForward, Vector3.up); // y축 방향 내적을 구하면 위쪽일 때 1, 아래쪽일 때 -1

        //// X축 회전값 보간하기
        //currentLookDeltaX = Mathf.Lerp(currentLookDeltaX, data.lookDelta.x, lerpSpeed * Time.deltaTime);

        //// currentLookDeltaX를 -1과 1 사이로 제한
        //currentLookDeltaX = Mathf.Clamp(currentLookDeltaX, -1f, 1f);

        //// 애니메이터에 값전달
        player.animationController.lookDir = data.lookDir;
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
