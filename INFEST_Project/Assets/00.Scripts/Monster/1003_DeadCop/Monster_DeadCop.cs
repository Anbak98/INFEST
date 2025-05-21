using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Monster_DeadCop : BaseMonster<Monster_DeadCop>
{
    public TickTimer animTickTimer;   // �ִϸ��̼��� ����ð����� �ٸ� �������� ���ٲ۴�(Ư�� ����)

    [Networked, OnChangedRender(nameof(OnPhaseIndexChanged))]
    public short PhaseIndex { get; set; } = 0;

    [Networked, OnChangedRender(nameof(OnIsRightPunch))]
    public NetworkBool IsRightPunch { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsHeadButt))]
    public NetworkBool IsHeadButt { get; set; } = false;


    [Networked, OnChangedRender(nameof(OnIsWave))]
    public NetworkBool IsWave { get; set; } = false;

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    private void OnPhaseIndexChanged() => animator.SetInteger("PhaseIndex", PhaseIndex);
    private void OnIsRightPunch() => animator.SetBool("IsRightPunch", IsRightPunch);
    private void OnIsHeadButt() => animator.SetBool("IsHeadButt", IsHeadButt);

    private void OnIsDead() => animator.SetBool("IsDead", IsDead);

    private void OnIsRunning() => animator.Play("Wander.DeadCop_Run");

    // ���̺� ����, �������� 
    private void OnIsWave() => animator.SetBool("IsWave", IsWave);  // ���̺� ������ ���õ� ������ �����ͼ� bool���� �ٲ۴�

    protected override void OnDead()
    {
        base.OnDead();
        if (IsDead)
        {
            FSM.ChangePhase<DeadCop_Phase_Dead>();
        }
    }


    protected override void OnWave()
    {
        base.OnWave();
        //TryAddTarget(target);
        //SetTarget(target);
        animator.Play("Wander.DeadCop_Run");
        FSM.ChangePhase<DeadCop_Phase_Chase>();
    }

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
