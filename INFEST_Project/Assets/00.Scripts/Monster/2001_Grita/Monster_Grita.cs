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

    public TickTimer screemCooldownTickTimer;   // ��ų�� ��Ÿ���� grita�� ��� phase, state�� �����ؾ��Ѵ�

    public const int screamMaxCount = 2;  // �ִ� 2���� �����ϴ�
    public int screamCount = 0;  // �ִ� 2���� �����ϴ�

    public const float ScreamCooldownSeconds = 50f;


    // �������� ����
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
        // Animation ����
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
    }

    // �Ҹ��� Host�� ��� Player�� �� �ִ� RPC 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_Scream()
    {
        Debug.Log("Scream!");   // ���߿� ����� Ŀ��

        if (_screamSound != null && _screamSoundClip != null)
            _screamSound.PlayOneShot(_screamSoundClip);

        screamCount++;

        // ��Ÿ�� ����(�ι�° ���ʹ� ��Ÿ�� 50��)
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
