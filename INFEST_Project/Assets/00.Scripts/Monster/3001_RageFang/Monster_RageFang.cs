using Fusion;
using INFEST.Game;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Monster_RageFang : BaseMonster<Monster_RageFang>
{
    public Dictionary<int, RageFangSkillTable> skills;
    public Dictionary<int, BossRunPoint> nextRegion;
    public int regionIndex = 1;

    public TickTimer RetreatTimer;
    public bool IsValidRetreat = true;

    [Networked, OnChangedRender(nameof(OnPhaseIndexChanged))]
    public short PhaseIndex { get; set; } = 0;

    [Networked, OnChangedRender(nameof(OnIsStretchChanged))]
    public NetworkBool IsStretch { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsRightPunch))]
    public NetworkBool IsRightPunch { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsLeftSwip))]
    public NetworkBool IsLeftSwip { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsJumping))]
    public NetworkBool IsJumping { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsFlexingMuscles))]
    public NetworkBool IsFlexingMuscles { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsRush))]
    public NetworkBool IsRush { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsJumpAttack))]
    public NetworkBool IsJumpAttack { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsContinousAttack))]
    public NetworkBool IsContinousAttack { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsQuickRollToRun))]
    public NetworkBool IsQuickRollToRun { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsTurnAttack))]
    public NetworkBool IsTurnAttack{ get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsRoaring))]
    public NetworkBool IsRoaring { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsPhaseWonder))]
    public NetworkBool IsPhaseWonder { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsPhaseAttack))]
    public NetworkBool IsPhaseAttack { get; set; } = false;



    public override void Spawned()
    {
        base.Spawned();
        skills = DataManager.Instance.GetDictionary<RageFangSkillTable>();
        nextRegion = DataManager.Instance.GetDictionary<BossRunPoint>();

        OnIsPhaseWonder();
        OnIsPhaseAttack();
    }

    public override void Render()
    {
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    protected override void OnDead()
    {
        base.OnDead();
        if(IsDead)
        {
            FSM.ChangePhase<Monster_RageFang_Phase_Dead>();
        }

        NetworkGameManager.Instance.VictoryGame();
    }

    public override bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, EWeaponType weaponType, bool isCritical)
    {
        target = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(instigator).transform;
        if(FSM.currentPhase is Monster_RageFang_Phase_Wonder)
        {
            FSM.ChangePhase<Monster_RageFang_Phase_Attack>();
        }

        return base.ApplyDamage(instigator, damage, position, direction, weaponType, isCritical);
    }

    public IEnumerator Buff(float sec, int dmg, int def)
    {
        OffsetDamage += dmg;
        OffsetDef += def;

        CurDamage = BaseDamage + OffsetDamage;
        CurDef = BaseDef + OffsetDef;

        yield return new WaitForSeconds(sec);

        OffsetDamage -= dmg;
        OffsetDef -= def;

        CurDamage = BaseDamage + OffsetDamage;
        CurDef = BaseDef + OffsetDef;
    }

    public void Buff(int dmg, int def)
    {
        OffsetDamage += dmg;
        OffsetDef += def;

        CurDamage = BaseDamage + OffsetDamage;
        CurDef = BaseDef + OffsetDef;
    }

    private void OnPhaseIndexChanged() => animator.SetInteger("PhaseIndex", PhaseIndex);
    private void OnIsStretchChanged() => animator.SetBool("IsStretch", IsStretch);
    private void OnIsRightPunch() => animator.SetBool("IsRightPunch", IsRightPunch);
    private void OnIsLeftSwip() => animator.SetBool("IsLeftSwip", IsLeftSwip);
    private void OnIsJumping() => animator.SetBool("IsJumping", IsJumping);
    private void OnIsFlexingMuscles() => animator.SetBool("IsFlexingMuscles", IsFlexingMuscles);
    private void OnIsRush() => animator.SetBool("IsRush", IsRush);
    private void OnIsJumpAttack() => animator.SetBool("IsJumpAttack", IsJumpAttack);
    private void OnIsContinousAttack() => animator.SetBool("IsContinousAttack", IsContinousAttack);
    private void OnIsQuickRollToRun() => animator.SetBool("IsQuickRollToRun", IsQuickRollToRun);
    private void OnIsTurnAttack() => animator.SetBool("IsTurnAttack", IsTurnAttack);
    private void OnIsRoaring() { if (IsRoaring) animator.Play("Retreat.Monster_RageFang_Roaring"); }
    private void OnIsPhaseAttack() { if (IsPhaseAttack) animator.Play("Attack.Monster_RageFang_Run"); }
    private void OnIsPhaseWonder() { if (IsPhaseWonder) animator.Play("Wonder.Monster_RageFang_Idle"); }
}
