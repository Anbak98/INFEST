using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IState
{
    public void Enter();    // ���� ����
    public void Exit();     // ���� ��
    public void OnUpdate(NetworkInputData data);   // ���� ������Ʈ
    public void PhysicsUpdate(NetworkInputData data);    // ���� ������Ʈ(�߷� ����) 
}


public abstract class PlayerBaseState : IState
{
    protected PlayerController controller;    // PlayerController, �ٸ� npc�� controller
    protected PlayerStateMachine stateMachine;    // PlayerStateMachine
    protected readonly PlayerStatHandler statHandler;   // PlayerStatHandler�� �ִ� �����͸� �б⸸ �Ѵ�
    protected Player player;
    // ������ �÷��̾��� ī�޶� ������ �׳� ������
    // ���߿��� �ٸ� ī�޶�(���ؿ� �����)�� �����Ͽ� CameraHandler�� ���� ī�޶� ������ �� ����

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

    #region �ִϸ��̼� ��ü
    // bool �Ķ����
    //protected void StartAnimation(int animatorHash)
    //{
    //    stateMachine.Player.playerAnimator.SetBool(animatorHash, true);
    //}
    //protected void StopAnimation(int animatorHash)
    //{
    //    stateMachine.Player.playerAnimator.SetBool(animatorHash, false);
    //}
    //// Trigger �ĸ�����
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

    #region �ִϸ��̼��� �ִ� �͵�(�̵�, �޸���, ���, ����, �ɱ�, �ɾƼ� �̵�, ����)�� ���� ����
    protected void PlayerMove(NetworkInputData data)
    {        
    }

    protected void PlayerFire(NetworkInputData data)
    {
        // �߻���� PlayerController�� �ű��
        controller.StartFire(data);
    }
    protected void Reload(NetworkInputData data)
    {
        player.Weapons.Reload();
        // ����
    }

    protected void PlayerRun(NetworkInputData data)
    {

    }
    protected void PlayerJump()
    {
        // Junp Ű�Է��ϸ� ���ο��� 1���� y�� ���ް� �� �ܴ� ���� ���� ������ �߷¸� �������̴�
        controller.StartJump();
    }
    protected void PlayerSit(NetworkInputData data)
    {
    }

    // �ɾƼ� �ȱ�
    protected void PlayerWaddle(NetworkInputData data)
    {
    }

    protected void PlayerSitFire(NetworkInputData data)
    {
        Debug.Log("SitFire");
        // ī�޶��� ȸ������(CameraHandler�� Update���� �ǽð����� ������Ʈ)���� �̵��Ѵ�
        controller.StartFire(data);
    }
    protected void SitReload(NetworkInputData data)
    {
        // ����
        Debug.Log("Reload");
        controller.StartReload(data);
    }
    // ����(�ִϸ��̼��� �ٲٰ�, ī�޶� ���� ����)
    protected void PlayerZoom(NetworkInputData data)
    {
        Debug.Log("Zoom");
    }
    #endregion
}
