using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 1��Ī �����տ� �پ �ִϸ��̼� ����
/// 1��Ī �������� �ִϸ��̼�, ȸ���� �����Ѵ�
/// 
/// ���콺 Ŀ�� �̵�
/// </summary>
public class LocalPlayerController : PlayerController
{
    // 1��Ī�� ����Ǵ� ������ animator�� 3�����̸� ���Ⱑ �ٲ�� animator�� ��ü�ؾ��Ѵ�
    public Animator animator;

    public override void Awake()
    {
        // animator�� �ִ� ���� �߰��ߴ�(1��Ī, 3��Ī ����)
        animator = GetComponent<Animator>();

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
        
    }

}
