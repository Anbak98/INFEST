using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 1인칭 프리팹에 붙어서 애니메이션 관리
/// 1인칭 프리팹의 애니메이션, 회전을 관리한다
/// 
/// 마우스 커서 이동
/// </summary>
public class LocalPlayerController : PlayerController
{
    // 1인칭에 적용되는 무기의 animator는 3종류이며 무기가 바뀌면 animator를 교체해야한다
    public Animator animator;

    protected override void Awake()
    {
        // animator이 있는 곳에 추가했다(1인칭, 3인칭 각각)
        animator = GetComponent<Animator>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        Debug.Log("LocalPlayerController FixedUpdateNetwork 진입");
        if (GetInput(out NetworkInputData input))
        {
            // 테스트용으로 플레이어를 이동시킨다

            //// 입력을 현재 상태에 전달
            //stateMachine.currentState.HandleInput(input);

            //// 상태 업데이트
            //stateMachine.currentState.UpdateLogic();
        }
    }


    public override void PlayFireAnim() => animator?.SetTrigger("Fire");



    // 애니메이션 교체할때 무기도 교체한다
    public override void HandleFire(bool started)
    {
        throw new System.NotImplementedException();
    }

    public override void StartJump()
    {
        throw new System.NotImplementedException();
    }
}
