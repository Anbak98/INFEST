using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Monster_GoreHaul : BaseMonster<Monster_GoreHaul>
{
    public Dictionary<int, GoreHaulSkillTable> skills;

    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsIdle { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsWalk { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsRun { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsPunch))]
    public NetworkBool IsPunch { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsLowAttack))]
    public NetworkBool IsLowAttack { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsJumpAttack))]
    public NetworkBool IsJumpAttack { get; set; } = false;

    // Wonder Phase
    private int _wonderParameterHash { get; set; }
    private int _walkHash { get; set; }

    // Chase Phase
    private int _chaseParameterHash { get; set; }
    private int _patternOneHash { get; set; }
    private int _patternTwoHash { get; set; }
    private int _patternThiHash { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        _wonderParameterHash = Animator.StringToHash("Wonder.Idle");
        _chaseParameterHash = Animator.StringToHash("Chase.Run");
        skills = DataManager.Instance.GetDictionary<GoreHaulSkillTable>();
    }

    protected override void OnWave()
    {
        base.OnWave();
        FSM.ChangePhase<GoreHaul_Phase_Chase>();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    protected override void OnDead()
    {
        base.OnDead();
        if (IsDead)
        {
            FSM.ChangePhase<GoreHaul_Phase_Dead>();
        }
    }

    private void OnIsWonderPhase() { if (IsIdle) animator.Play(_wonderParameterHash); }
    private void OnIsChasePhase() { if (IsRun) animator.Play(_chaseParameterHash); }

    private void OnIsPunch() => animator.SetBool("IsPunch", IsPunch);
    private void OnIsLowAttack() => animator.SetBool("IsLowAttack", IsLowAttack);
    private void OnIsJumpAttack() => animator.SetBool("IsJumpAttack", IsJumpAttack);
}