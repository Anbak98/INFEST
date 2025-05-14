using Fusion;
using UnityEngine;

public class Monster_Bowmeter : BaseMonster<Monster_Bowmeter>
{
    public EliteMonsterSkillTable skillTable;
    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsIdle { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsWalk { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsRun { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsPunch))]
    public NetworkBool IsPunch { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsBwack))]
    public NetworkBool IsBwack { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsShoot))]
    public NetworkBool IsShoot { get; set; } = false;

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
        skillTable = DataManager.Instance.GetByKey<EliteMonsterSkillTable>(2003);
        _wonderParameterHash = Animator.StringToHash("Wonder.Idle");
        _chaseParameterHash = Animator.StringToHash("Chase.Run");
    }


    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    private void OnIsWonderPhase() { if (IsIdle) animator.Play(_wonderParameterHash); }
    private void OnIsChasePhase() { if (IsRun) animator.Play(_chaseParameterHash); }

    private void OnIsPunch() => animator.SetBool("IsPunch", IsPunch);
    private void OnIsBwack() => animator.SetBool("IsBwack", IsBwack);
    private void OnIsShoot() => animator.SetBool("IsShoot", IsShoot);
}