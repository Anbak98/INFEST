using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Stacker : BaseMonster<Monster_Stacker>
{
    public Dictionary<int, CommonSkillTable> commonSkill;

    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsWonderPhase { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsChasePhase { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsPunch))]
    public NetworkBool IsPunch { get; set; } = false;

    private void OnIsWonderPhase() { if (IsWonderPhase) animator.Play("Wonder.Idle"); }
    private void OnIsChasePhase() { if (IsChasePhase) animator.Play("Chase.Run"); }
    private void OnIsPunch() => animator.SetBool("IsPunch", IsPunch);

    public override void Spawned()
    {
        base.Spawned();
        commonSkill = DataManager.Instance.GetDictionary<CommonSkillTable>();
        OnIsWonderPhase();
        OnIsChasePhase();
        OnIsPunch();
    }
    public override void OnWave(Transform target)
    {
        base.OnWave(target);
        TryAddTarget(target);
        SetTarget(target);
        FSM.ChangePhase<Stacker_Phase_Chase>();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        if (IsAttack)
        {
            animator.SetTrigger("IsAttack");
        }
        if (IsDead)
        {
            //FSM.ChangePhase<PJ_HI_II_DeadPhase>();
            animator.SetBool("IsDead", IsDead);
        }

        AIPathing.speed = CurMovementSpeed;
    }
}
