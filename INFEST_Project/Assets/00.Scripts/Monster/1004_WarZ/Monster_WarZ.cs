using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_WarZ : BaseMonster<Monster_WarZ>
{
    public TickTimer animTickTimer;   // 애니메이션의 재생시간동안 다른 동작으로 못바꾼다(특히 공격)
    public Dictionary<int, CommonSkillTable> CommonSkillTable;

    [Networked, OnChangedRender(nameof(OnPhaseIndexChanged))]
    public short PhaseIndex { get; set; } = 0;

    [Networked, OnChangedRender(nameof(OnIsRightPunch))]
    public NetworkBool IsRightPunch { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsDropKick))]
    public NetworkBool IsDropKick{ get; set; } = false;



    [Networked, OnChangedRender(nameof(OnIsWave))]
    public NetworkBool IsWave { get; set; } = false;

    public void Start()
    {
        CommonSkillTable = DataManager.Instance.GetDictionary<CommonSkillTable>();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    private void OnPhaseIndexChanged()
    {
        switch(PhaseIndex)
        {
            case 0:
                animator.Play("Wonder.WarZ_Idle"); break;
            case 1:
                animator.Play("Chase.WarZ_Run"); break;
        }
    }
    private void OnIsRightPunch() => animator.SetBool("IsRightPunch", IsRightPunch);
    private void OnIsDropKick() => animator.SetBool("IsDropKick", IsDropKick);

    private void OnIsDead() => animator.SetBool("IsDead", IsDead);


    private void OnIsRunning() => animator.Play("Wander.WarZ_Run");

    // 웨이브 시작, 종료조건 
    private void OnIsWave() => animator.SetBool("IsWave", IsWave);  // 웨이브 생성과 관련된 곳에서 가져와서 bool값을 바꾼다

    protected override void OnDead()
    {
        base.OnDead();
        if (IsDead)
        {
            FSM.ChangePhase<WarZ_Phase_Dead>();
        }
    }

    protected override void OnWave()
    {
        base.OnWave();
        //TryAddTarget(target);
        //SetTarget(target);
        animator.Play("Wander.WarZ_Run");
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
