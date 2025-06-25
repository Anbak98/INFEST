using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class Monster_PJ_HI : BaseMonster<Monster_PJ_HI>
{
    public Dictionary<int, CommonSkillTable> CommonSkillTable;

    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsWonderPhase { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsChasePhase { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsPunch))]
    public NetworkBool IsPunch { get; set; } = false;

    private void OnIsWonderPhase() { if (IsWonderPhase) animator.Play("Wonder.Idle"); }
    private void OnIsChasePhase() { if (IsChasePhase) animator.Play("Chase.Run"); }
    private void OnIsPunch() => animator.SetTrigger("IsPunch");

    public override void Spawned()
    {
        base.Spawned();

        CommonSkillTable = DataManager.Instance.GetDictionary<CommonSkillTable>();

    }

    protected override void OnWave()
    {
        base.OnWave();
        IsChasePhase = true;
        OnIsChasePhase();
        FSM.ChangePhase<PJ_HI_Phase_Chase>();
    }

    protected override void OnDead()
    {
        base.OnDead();
        FSM.ChangePhase<PJ_HI_Phase_Dead>();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }
}
