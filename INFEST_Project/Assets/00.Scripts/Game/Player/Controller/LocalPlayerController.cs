using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;


/// <summary>
/// 1��Ī �����տ� �پ �ִϸ��̼� ����
/// �ڽ��� �̵��� ���÷� ����Ѵ�
/// 
/// �ڽ��� ���忡�� 3��Ī�� ��Ȱ��ȭ�� �Ǿ������״ϱ�
/// 
/// �÷��̾��� �̵�
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
    /// 1��Ī�� ���� ������ �ִ°Ŵϱ� ���� ��ȭ�ϴ°� �ڽ��� Update���� ó���Ѵ�
    /// </summary>
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        // LocalPlayerController���� ������ �Ʒ��� �߰�
    }

    // ���� ���ȳ�
    public override bool IsJumpInput() => player.Input.GetIsJumping();
    public override bool IsSitInput() => player.Input.GetIsSitting();

    // �÷��̾ �� ���� �ִ���?
    public override bool IsGrounded() => player.characterController.isGrounded;
    public override float GetVerticalVelocity() => verticalVelocity;



    // �÷��̾��� �̵�(������ CameraHandler���� ����) ó��
    public override void HandleMovement()
    {
        Vector3 input = player.Input.MoveInput;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 move = right * input.x + forward * input.z;
        move.y = 0f; // ���� ���� ����
        player.characterController.Move(move.normalized * player.statHandler.MoveSpeed * player.statHandler.MoveSpeedModifier * Time.deltaTime);
    }
    public override void ApplyGravity()
    {
        // TODO
        if (IsGrounded() && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        Vector3 gravityMove = new Vector3(0f, verticalVelocity, 0f);
        player.characterController.Move(gravityMove * Time.deltaTime);
    }
    /// <summary>
    /// ���� ���� �� ���� �ӵ� ���
    /// </summary>
    public override void StartJump()
    {
        verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * gravity);
    }

    // �ɴ´�
    public override void StartSit()
    {
        // collider�� ���¿��� ��ȭ��Ű�Ƿ� ���⼭�� transform�� �Ʒ���
        float playerYpos = player.transform.position.y;
        playerYpos /= 2;
        player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    }
    // �Ͼ��
    public override void StartStand()
    {
        // collider�� ���¿��� ��ȭ��Ű�Ƿ� ���⼭�� transform�� �Ʒ���
        float playerYpos = player.transform.position.y;
        playerYpos *= 2;
        player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    }


    public override void HandleFire()
    {
        bool input = player.Input.GetIsFiring();
        if (input)
        {
            // TODO
        }
    }
}
