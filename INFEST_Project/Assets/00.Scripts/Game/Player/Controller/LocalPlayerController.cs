using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //public override void PlayFireAnim() => player.firstPersonAnimator?.SetTrigger("Fire");
    public override void PlayFireAnim() => player.playerAnimator?.SetTrigger("Fire");

    // 플레이어의 입력
    public override void HandleMovement()
    {
        base.HandleMovement();

        player = GetComponentInParent<Player>();

        //Vector3 input = _input.MoveInput;
    }



    // 애니메이션 교체할때 무기도 교체한다
    public override void HandleFire(bool started)
    {
        throw new System.NotImplementedException();
    }

    public override void StartJump()
    {
        throw new System.NotImplementedException();
    }
    // 앉는 동작은 Local과 Remote가 다르다
}
