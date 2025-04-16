using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 1��Ī �����տ� �پ �ִϸ��̼� ����
/// 1��Ī �������� �ִϸ��̼�, ȸ���� �����Ѵ�
/// </summary>
public class LocalPlayerController : PlayerController
{
    // 1��Ī�� ����Ǵ� ������ animator�� 3�����̸� ���Ⱑ �ٲ�� animator�� ��ü�ؾ��Ѵ�
    public Animator animator;

    protected override void Awake()
    {
        // animator�� �ִ� ���� �߰��ߴ�(1��Ī, 3��Ī ����)
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

    public override void PlayFireAnim() => animator?.SetTrigger("Fire");


    // �ִϸ��̼� ��ü�Ҷ� ���⵵ ��ü�Ѵ�
    public override void HandleFire(bool started)
    {
        throw new System.NotImplementedException();
    }

    public override void StartJump()
    {
        throw new System.NotImplementedException();
    }
}
