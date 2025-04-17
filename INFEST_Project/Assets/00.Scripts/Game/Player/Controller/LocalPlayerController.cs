using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //public override void PlayFireAnim() => player.firstPersonAnimator?.SetTrigger("Fire");
    public override void PlayFireAnim() => player.playerAnimator?.SetTrigger("Fire");

    // �÷��̾��� �Է�
    public override void HandleMovement()
    {
        base.HandleMovement();

        player = GetComponentInParent<Player>();

        //Vector3 input = _input.MoveInput;
    }



    // �ִϸ��̼� ��ü�Ҷ� ���⵵ ��ü�Ѵ�
    public override void HandleFire(bool started)
    {
        throw new System.NotImplementedException();
    }

    public override void StartJump()
    {
        throw new System.NotImplementedException();
    }
    // �ɴ� ������ Local�� Remote�� �ٸ���
}
