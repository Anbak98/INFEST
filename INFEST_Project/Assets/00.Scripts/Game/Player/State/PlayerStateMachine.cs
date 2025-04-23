using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput���� ���� ������ �������� ���¸� ������Ʈ(���� Ȥ�� ��ȭ)
/// </summary>
public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    // player�� �Է������� �������⸸ �Ѵ�
    public PlayerInputHandler InputHandler { get; }

    // ���߿� Dictionary�� ���¸� �����ϴ� ������� ������ ����
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerFireState FireState { get; private set; }
    public PlayerReloadState ReloadState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }

    // ī�޶��� ȸ������ ���� ���⿡��...? ���� �� ����غ��� 
    public Transform MainCameraTransform { get; set; }  // Rotation�� �� ī�޶� ���� ȸ���ؾ��Ѵ�

    public PlayerStateMachine(Player player, PlayerController controller)
    {
        this.Player = player;

        MainCameraTransform = Camera.main.transform;

        // StateMachine���� BaseControllerŸ���� ����ϹǷ�, �̸� ���޹޴� ������ ����Ϸ��� ����ȯ �ʿ�
        // ���׸����� �����ϰ� ĳ����
        var playerController = GetController<PlayerController>();

        // ��������
        IdleState = new PlayerIdleState(playerController, this);
        WalkState = new PlayerWalkState(playerController, this);
        RunState = new PlayerRunState(playerController, this);
        JumpState = new PlayerJumpState(playerController, this);
        FallState = new PlayerFallState(playerController, this);

        // ���⿡���� ������ �����ϴµ�, PlayerInputHandler�� �÷��̾��� �Է°��� �����ϰ� �����Ƿ� �Ʒ��� ������ ������Ѵ�
        //MovementSpeed = player.Data.GroundData.BaseSpeed;
        //RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }
    // ChangeState, Update ���� �޼���� �θ��� StateMachine�� ������ ���� ����Ѵ�
    protected override void UpdateStateTransition()
    {
        // �Է°��� ���� ���� ����


    }
    // ���� ���� �Լ���

}

