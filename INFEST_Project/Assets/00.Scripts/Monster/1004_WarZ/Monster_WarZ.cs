using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_WarZ : BaseMonster<Monster_WarZ>
{
    public TickTimer animTickTimer;   // �ִϸ��̼��� ����ð����� �ٸ� �������� ���ٲ۴�(Ư�� ����)

    [Networked, OnChangedRender(nameof(OnPhaseIndexChanged))]
    public short PhaseIndex { get; set; } = 0;

    [Networked, OnChangedRender(nameof(OnIsRightPunch))]
    public NetworkBool IsRightPunch { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsDropKick))]
    public NetworkBool IsDropKick{ get; set; } = false;



    [Networked, OnChangedRender(nameof(OnIsWave))]
    public NetworkBool IsWave { get; set; } = false;

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    private void OnPhaseIndexChanged() => animator.SetInteger("PhaseIndex", PhaseIndex);
    private void OnIsRightPunch() => animator.SetBool("IsRightPunch", IsRightPunch);
    private void OnIsDropKick() => animator.SetBool("IsDropKick", IsDropKick);

    private void OnIsDead() => animator.SetBool("IsDead", IsDead);


    private void OnIsRunning() => animator.Play("Wander.WarZ_Run");

    // ���̺� ����, �������� 
    private void OnIsWave() => animator.SetBool("IsWave", IsWave);  // ���̺� ������ ���õ� ������ �����ͼ� bool���� �ٲ۴�


    public override void OnWave(Transform target)
    {
        base.OnWave(target);
        TryAddTarget(target);
        SetTarget(target);
        FSM.ChangePhase<WarZ_Phase_Chase>();
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
