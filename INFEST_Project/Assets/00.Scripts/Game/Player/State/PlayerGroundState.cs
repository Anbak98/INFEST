using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ���� �پ��ִ� ��� ���� ���� ����
// Idle, Move, Run, Attack, Reload
public class PlayerGroundState : PlayerBaseState
{
    float lookThreshold = 0.3f; // �󸶳� �¿�� �������� �Ǵ��� ���� �� (���� ����)

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
        player.animationController.lookDelta = data.lookDelta;
        if (data.lookDelta.y != 0f)
        {
            // �� �Ǵ� �Ʒ��� ��������
            player.animationController.playerAnimator.GetLayerIndex("Look");    // Layer�� �����Ѵ�
        }
        // lookDelta.x�� ������ ����
        // ȸ������ -30������ +30���϶��� ��ü(UpperBody)�� ȸ������
        // �� ���� ���������� 
        // �� ��ü(Base)�� ȸ������ Layer�� �����Ѵ�
        if (Mathf.Abs(data.lookDelta.x) < lookThreshold)
        {
            // ��ü�� ȸ���Ѵ�
            player.animationController.playerAnimator.GetLayerIndex()
        }

    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
