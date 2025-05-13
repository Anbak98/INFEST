using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IState
{
    public void Enter();    // 상태 진입
    public void Exit();     // 상태 끝
    public void OnUpdate(NetworkInputData data);   // 상태 업데이트
    public void PhysicsUpdate(NetworkInputData data);    // 물리 업데이트(중력 관련) 
}


public abstract class PlayerBaseState : IState
{
    protected PlayerController controller;    // PlayerController, 다른 npc의 controller
    protected PlayerStateMachine stateMachine;    // PlayerStateMachine
    protected readonly PlayerStatHandler statHandler;   // PlayerStatHandler에 있는 데이터를 읽기만 한다
    protected Player player;
    // 지금은 플레이어의 카메라만 있으니 그냥 하지만
    // 나중에는 다른 카메라(조준에 사용할)를 포함하여 CameraHandler를 통해 카메라에 접근할 수 있음

    public Transform MainCameraTransform { get; set; }


    public PlayerBaseState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
        statHandler = stateMachine.Player.statHandler;
        player = stateMachine.Player;
        //stateMachine.Player.playerAnimator = player.playerAnimator;

        MainCameraTransform = Camera.main.transform;
    }

    public virtual void Enter()
    {
    }
    public virtual void Exit()
    {
    }
    public virtual void OnUpdate(NetworkInputData data)
    {
        if (statHandler.info.CurHealth <= 0)
        {
            stateMachine.ChangeState(stateMachine.DeadState);
        }
    }
    public virtual void PhysicsUpdate(NetworkInputData data)
    {
    }

    #region 애니메이션 교체
    // bool 파라미터
    //protected void StartAnimation(int animatorHash)
    //{
    //    stateMachine.Player.playerAnimator.SetBool(animatorHash, true);
    //}
    //protected void StopAnimation(int animatorHash)
    //{
    //    stateMachine.Player.playerAnimator.SetBool(animatorHash, false);
    //}
    //// Trigger 파리미터
    //protected void SetTriggerAnimation(int animatorHash)
    //{
    //    stateMachine.Player.playerAnimator.SetTrigger(animatorHash);
    //}
    //// MoveX, MoveZ
    //protected void SetAnimationFloat(int animatorHash, float value)
    //{
    //    stateMachine.Player.playerAnimator.SetFloat(animatorHash, value);
    //}

    #endregion

    #region 애니메이션이 있는 것들(이동, 달리기, 사격, 점프, 앉기, 앉아서 이동, 조준)의 실제 동작
    protected void PlayerMove(NetworkInputData data)
    {        
    }

    protected void PlayerFire(NetworkInputData data)
    {
        // 발사로직 PlayerController에 옮길것
        controller.StartFire(data);
    }
    protected void Reload(NetworkInputData data)
    {
        player.Weapons.Reload();
        // 장전
    }

    protected void PlayerRun(NetworkInputData data)
    {

    }
    protected void PlayerJump()
    {
        // Junp 키입력하면 내부에서 1번만 y축 힘받고 그 외는 땅에 닿을 때까지 중력만 받을것이다
        controller.StartJump();
    }
    protected void PlayerSit(NetworkInputData data)
    {
    }

    // 앉아서 걷기
    protected void PlayerWaddle(NetworkInputData data)
    {
    }

    protected void PlayerSitFire(NetworkInputData data)
    {
        Debug.Log("SitFire");
        // 카메라의 회전방향(CameraHandler의 Update에서 실시간으로 업데이트)으로 이동한다
        controller.StartFire(data);
    }
    protected void SitReload(NetworkInputData data)
    {
        // 장전
        Debug.Log("Reload");
        controller.StartReload(data);
    }
    // 조준(애니메이션은 바꾸고, 카메라를 따로 조작)
    protected void PlayerZoom(NetworkInputData data)
    {
        Debug.Log("Zoom");
    }
    #endregion
}
