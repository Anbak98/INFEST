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
    // �÷��̾��� �̵�(������ CameraHandler���� ����)
    public override void HandleMovement()
    {
        Vector3 input = player.Input.MoveInput;

        /*Vector3 move = head.right * input.x + head.forward * input.z;
        _controller.Move(move * _statHandler.MoveSpeed * Time.deltaTime);*/

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 move = right * input.x + forward * input.z;
        move.y = 0f; // ���� ���� ����
        player.characterController.Move(move.normalized * player.statHandler.MoveSpeed * Time.deltaTime);
    }

    public override void ApplyGravity()
    {

    }
    // �ɴ� ������ Local�� Remote�� �ٸ���
}
