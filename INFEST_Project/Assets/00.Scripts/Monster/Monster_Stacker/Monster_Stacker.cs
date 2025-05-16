using System.Threading;
using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Stacker : BaseMonster<Monster_Stacker>
{

    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsIdle { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsWalk { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsRun { get; set; } = false;

    public Dictionary<int, CommonSkillTable> commonSkill;

    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsWonderPhase { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsChasePhase { get; set; } = false;


    [Networked, OnChangedRender(nameof(OnIsPunch))]
    public NetworkBool IsPunch { get; set; } = false;


    private int _wonderParameterHash { get; set; }

    private int _chaseParameterHash { get; set; }

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

        CurMovementSpeed = info.SpeedMoveWave;
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
}
