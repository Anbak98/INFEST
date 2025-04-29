using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

// ���� �پ��ִ� ��� ���� ���� ����
// Idle, Move, Run, Attack, Reload
public class PlayerGroundState : PlayerBaseState
{
    // ������ �ִ� �÷��̾��� forward�� ����
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

        // �÷��̾��� ī�޶� �����Ͽ� ī�޶��� forward�� �÷��̾��� forward�� ���̰��� ���Ͽ� 90���� 1�� �Ǿ���ϴϱ�
        Vector3 camForward = player.cameraHandler.transform.forward;
        Vector3 playerForward = player.transform.forward;

        // �� ���� ���� ����
        float angleY = Vector3.Angle(camForward, playerForward);  // 0 ~ 180

        // angle�� 0���̸� ����, 90���̸� ����, 180���� �ݴ� ����
        // �׷��� ���⿡ ��ȣ�� �ٿ��� �Ѵ�! (������ �̿��Ͽ� ��/�Ʒ� ���� ����)
        float dotY = Vector3.Dot(camForward, Vector3.up); // y�� ���� ������ ���ϸ� ������ �� 1, �Ʒ����� �� -1


        if (data.lookDelta.x != 0f)
        {
            // player�� forward�� �����Ѵ�
            prevForward = player.transform.forward;
        }
        // �� forward.x�� ������ player�� forward.x�� �����ؾ��Ѵ�
        //float dotX = Vector3.Dot(Vector3.forward, playerForward); 
        
        // �ִϸ����Ϳ� ������
        player.animationController.lookDelta = new Vector2(data.lookDelta.x, dotY);
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
