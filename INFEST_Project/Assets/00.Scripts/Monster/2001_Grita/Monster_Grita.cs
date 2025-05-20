using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Monster_Grita : BaseMonster<Monster_Grita>
{
    public Dictionary<int, GritaSkillTable> skill;
    [SerializeField] private AudioSource _screamSound;
    [SerializeField] private AudioClip _screamSoundClip;

    public TickTimer screemCooldownTickTimer;   // 스킬의 쿨타임은 grita의 모든 phase, state가 공유해야한다

    public const int screamMaxCount = 2;  // 최대 2번만 가능하다
    public int screamCount = 0;  // 최대 2번만 가능하다

    public const float ScreamCooldownSeconds = 50f;


    // 동적으로 연결
    [SerializeField] public MonsterSpawner spawner;


    #region animotor parameter

    [Networked, OnChangedRender(nameof(OnIsScreamChanged))]
    public NetworkBool IsScream { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsWonderPhase))]
    public NetworkBool IsWonderPhase { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsChasePhase))]
    public NetworkBool IsChasePhase { get; set; } = false;

    [Networked, OnChangedRender(nameof(OnIsPunch))]
    public NetworkBool IsPunch { get; set; } = false;

    private void OnIsWonderPhase() { if (IsWonderPhase) animator.Play("Wonder.Idle"); }
    private void OnIsChasePhase() { if (IsChasePhase) animator.Play("Chase.Grita_Run"); }
    private void OnIsScreamChanged() { if (IsScream) animator.Play("Scream.Grita_Scream"); }
    private void OnIsPunch() => animator.SetBool("IsPunch", IsPunch);
    #endregion

    public override void Spawned()
    {
        base.Spawned();
        skill = DataManager.Instance.GetDictionary<GritaSkillTable>();
    }

    public override void Render()
    {
        // Animation 관련
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    // 소리는 Host가 모든 Player로 쏴 주는 RPC 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_Scream()
    {
        Debug.Log("Scream!");   // 나중에 지우고 커밋

        if (_screamSound != null && _screamSoundClip != null)
            _screamSound.PlayOneShot(_screamSoundClip);

        screamCount++;

        // 쿨타임 시작(두번째 부터는 쿨타임 50초)
        screemCooldownTickTimer = TickTimer.CreateFromSeconds(Runner, ScreamCooldownSeconds);
    }

    protected override void OnWave()
    {
        base.OnWave();
        FSM.ChangePhase<Grita_Phase_Chase>();
    }

    protected override void OnDead()
    {
        base.OnDead();
        FSM.ChangePhase<Grita_Phase_Dead>();
    }
}
