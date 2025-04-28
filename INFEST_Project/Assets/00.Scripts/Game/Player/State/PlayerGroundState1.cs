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
    // Ŭ���� ��ܿ� ���� �߰�
    private float currentLookDeltaX = 0f;
    private float lerpSpeed = 5f; // ���� �ӵ�(�������� ������ ��ȭ)


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

        //// �÷��̾��� ī�޶� �����Ͽ� ī�޶��� forward�� �÷��̾��� forward�� ���̰��� ���Ͽ� 90���� 1�� �Ǿ���ϴϱ�
        //Vector3 camForward = player.cameraHandler.transform.forward;
        //Vector3 playerForward = player.transform.forward;

        //// �� ���� ���� ����
        //float angleY = Vector3.Angle(camForward, playerForward);  // 0 ~ 180

        //// angle�� 0���̸� ����, 90���̸� ����, 180���� �ݴ� ����
        //// �׷��� ���⿡ ��ȣ�� �ٿ��� �Ѵ�! (������ �̿��Ͽ� ��/�Ʒ� ���� ����)
        //float dotY = Vector3.Dot(camForward, Vector3.up); // y�� ���� ������ ���ϸ� ������ �� 1, �Ʒ����� �� -1

        //// X�� ȸ���� �����ϱ�
        //currentLookDeltaX = Mathf.Lerp(currentLookDeltaX, data.lookDelta.x, lerpSpeed * Time.deltaTime);

        //// currentLookDeltaX�� -1�� 1 ���̷� ����
        //currentLookDeltaX = Mathf.Clamp(currentLookDeltaX, -1f, 1f);

        //// �ִϸ����Ϳ� ������
        player.animationController.lookDir = data.lookDir;
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
