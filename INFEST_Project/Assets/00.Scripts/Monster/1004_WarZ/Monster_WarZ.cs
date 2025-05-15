using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_WarZ : BaseMonster<Monster_WarZ>
{
    public TickTimer animTickTimer;   // �ִϸ��̼��� ����ð����� �ٸ� �������� ���ٲ۴�(Ư�� ����)

    //[Networked, OnChangedRender(nameof(OnIsHeadButt))]
    //public NetworkBool IsHeadButt { get; set; } = false;


    [Networked, OnChangedRender(nameof(OnIsRunning))]
    public NetworkBool IsRunning { get; set; } = false;



    [Networked, OnChangedRender(nameof(OnIsWave))]
    public NetworkBool IsWave { get; set; } = false;

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        animator.SetFloat("Distance", AIPathing.remainingDistance);

        if (IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            animator.SetBool("IsDead", IsDead);
        }
        AIPathing.speed = CurMovementSpeed;
    }
    //private void OnIsHeadButt() => animator.SetBool("IsHeadButt", IsHeadButt);


    // ó�� ���� �ִϸ��̼� ���
    //private void OnIsIdle() => animator.Play("Wander.DeadCop_Idle");
    private void OnIsRunning() => animator.Play("Wander.WarZ_Run");


    // ���̺� ����, �������� 
    private void OnIsWave() => animator.SetBool("IsWave", IsWave);  // ���̺� ������ ���õ� ������ �����ͼ� bool���� �ٲ۴�




    public float GetCurrentAnimLength()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            return clipInfos[0].clip.length;
        }
        return 0f;
    }
}
