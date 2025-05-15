using Fusion;
using UnityEngine;

public class Monster_PJ_HI : BaseMonster<Monster_PJ_HI>
{

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
        OnIsWonderPhase();
        OnIsChasePhase();
        OnIsPunch();
    }

    public override void OnWave(Transform target)
    {
        base.OnWave(target);
        TryAddTarget(target);
        SetTarget(target);
        IsChasePhase = true;
        FSM.ChangePhase<PJ_HI_Phase_Chase>();
    }

    public override void OnDead()
    {
        base.OnDead();
        FSM.ChangePhase<PJ_HI_Phase_Dead>();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }
}
