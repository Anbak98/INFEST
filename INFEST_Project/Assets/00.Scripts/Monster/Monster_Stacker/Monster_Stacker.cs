using Fusion;
using UnityEngine;

public class Monster_Stacker : BaseMonster<Monster_Stacker>
{
    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsIdle { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsWalk { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsRun { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsPunch))]
    public NetworkBool IsPunch { get; set; } = false;

    private int _wonderParameterHash { get; set; }

    private int _chaseParameterHash { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        _wonderParameterHash = Animator.StringToHash("Wonder.Idle");
        _chaseParameterHash = Animator.StringToHash("Chase.Run");
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    public override void OnDead()
    {
        base.OnDead();
        if (IsDead)
        {
            FSM.ChangePhase<Stacker_Phase_Dead>();
        }
    }

    public override void OnWave(Transform target)
    {
        base.OnWave(target);
        TryAddTarget(target);
        SetTarget(target);
        FSM.ChangePhase<Stacker_Phase_Chase>();
    }

    private void OnIsWonderPhase() { if (IsIdle) animator.Play(_wonderParameterHash); }
    private void OnIsChasePhase() { if (IsRun) animator.Play(_chaseParameterHash); }

    private void OnIsPunch() => animator.SetBool("IsPunch", IsPunch);
}
