using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;


/// <summary>
/// 1인칭 프리팹에 붙어서 애니메이션 관리
/// 자신의 이동은 로컬로 계산한다
/// 
/// 자신의 입장에서 3인칭은 비활성화가 되어있을테니까
/// 
/// 플레이어의 이동
/// </summary>
public class LocalPlayerController : PlayerController
{
    public override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    /// <summary>
    /// 1인칭은 나만 가지고 있는거니까 상태 변화하는건 자신의 Update에서 처리한다
    /// </summary>
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // LocalPlayerController만의 로직은 아래에 추가
    }
    // 플레이어의 이동(방향은 CameraHandler에서 설정)
    public override void HandleMovement()
    {
        Vector3 input = player.Input.MoveInput;

        /*Vector3 move = head.right * input.x + head.forward * input.z;
        _controller.Move(move * _statHandler.MoveSpeed * Time.deltaTime);*/

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 move = right * input.x + forward * input.z;
        move.y = 0f; // 수직 방향 제거
        player.characterController.Move(move.normalized * player.statHandler.MoveSpeed * Time.deltaTime);
    }

    public override void ApplyGravity()
    {

    }
    // 앉는 동작은 Local과 Remote가 다르다
}
