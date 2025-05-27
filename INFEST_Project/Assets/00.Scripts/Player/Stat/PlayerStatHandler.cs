using Fusion;
using INFEST.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : NetworkBehaviour
{
    public CharacterInfoInstance info;

    [Networked] public bool IsDead { get; set; }

    [Networked] public int CurSpeedMove { get; set; }
    [Networked] public int CurHealth { get; set; }                          // ü��
    [Networked] public int CurDefGear { get; set; }                         // �� ü��
    [Networked] public int CurDef { get; set; }
    // �� ü��
    public int CurGold
    {
        get => NetworkGameManager.Instance.gamePlayers.GetGoldCount(Object.InputAuthority);
        set => NetworkGameManager.Instance.gamePlayers.SetGoldCount(Object.InputAuthority, value);
    }
    // ���� ���
    [Networked] public int CurTeamCoin { get; set; }                        // ���� ������
    [Networked] public int curstate { get; set; }                           // ĳ���� ����

    public event Action OnDeath;
    public event Action OnRespawn;
    public event Action<float> OnHealthChanged;

    public PlayerAttackedEffectController effect;

    public void Init()
    {
        info = new(1);

        if (HasStateAuthority)
        {
            CurGold += info.data.StartGold;
        }

        CurHealth = info.data.Health;
        CurDefGear = info.data.DefGear;
        CurDef = info.data.Def;
        CurTeamCoin = info.data.StartTeamCoin;
        CurSpeedMove = info.data.SpeedMove;
        curstate = info.data.State;
    }

    // �ǰ�
    public void TakeDamage(MonsterNetworkBehaviour attacker, int amount)
    {
        int damage = amount - CurDef;

        if (damage <= 0)
        {
            damage = 1;
        }

        if (CurDefGear > damage)
        {
            CurDefGear -= damage;
        }
        else if (CurDefGear >= 0)
        {
            if (!IsDead)
            {
                damage -= CurDefGear;
                CurDefGear = 0;

                CurHealth -= damage;

                RPC_Effect(-1);

                if (CurHealth <= 0)
                {
                    CurHealth = 0;

                    if (!IsDead)
                    {
                        HandleDeath();
                        if (attacker != null)
                            AnalyticsManager.analyticsPlayerDie(attacker.key, (int)Runner.SimulationTime, NetworkGameManager.Instance.monsterSpawner.WaveNum, $"{transform.position}");
                    }
                }
            }
        }

        if (CurDefGear < 0)
            CurDefGear = 0;
    }

    public void SetHealth(int amount)
    {
        //if (CurrentHealth <= 0)
        //    return;

        CurHealth = amount;
        RPC_Effect(1);
        if (CurHealth <= 0)
        {
            CurHealth = 0;
        }
        else if (IsDead && CurHealth > 0)
        {
            IsDead = false;
            HandleRespawn();
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_Effect(float amount)
    {
        effect.CalledWhenPlayerAttacked(-1);
    }

    // ȸ��eathCount = 0;

    //foreach (var p in NetworkGameManager.Instance.gamePlayers.GetPlayerRefs())
    //{
    //    NetworkGameManager.Instance.gamePlayers.GetPlayerObj(p);
    //}
    public void Heal(int amount)
    {
        CurHealth = Mathf.Min(CurHealth + amount, info.data.Health);
        if (HasInputAuthority)
            OnHealthChanged?.Invoke(amount);
    }
    // ���
    public void HandleDeath()
    {
        //// MyDebug.Log($"PlayerStatHandler HandleDeath : {CurrentHealth}"); 
        IsDead = true;

        OnDeath?.Invoke();
        ////NetworkGameManager.Instance.playerCount--;

        //int deathCount = 0;

        //foreach (var player in NetworkGameManager.Instance.gamePlayers.GetPlayerRefs())
        //{
        //    if (NetworkGameManager.Instance.gamePlayers.GetPlayerObj(player).IsDead)
        //    {
        //        deathCount++;
        //    }
        //}

        //if (deathCount >= Runner.SessionInfo.PlayerCount)
        //{
        //    NetworkGameManager.Instance.DefeatGame();
        //    RPC_HideDeathScreen();
        //}
        //else
        //{
        //    RPC_ShowDeathScreen();
        //}

        //int d
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_ShowDeathScreen()
    {
        Global.Instance.UIManager.Show<UIDeathScreen>();
        Global.Instance.UIManager.Hide<UIStateView>();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    public void RPC_HideDeathScreen()
    {
        Global.Instance.UIManager.Hide<UIDeathScreen>();
    }

    // ������
    public void HandleRespawn()
    {
        OnRespawn?.Invoke();
    }

    //// ���ӽð� ���� ���� ó��
    //public void ApplyTemporaryBuff(
    //int? speedDelta = null,
    //int? jumpDelta = null,
    //int? attackDelta = null,
    //int? defenseDelta = null,
    //int duration = 5)
    //{
    //    StartCoroutine(ApplyBuffCoroutine(speedDelta, jumpDelta, attackDelta, defenseDelta, duration));
    //}
    //private IEnumerator ApplyBuffCoroutine(
    //int? speedDelta,
    //int? jumpDelta,
    //int? attackDelta,
    //int? defenseDelta,
    //int duration)
    //{
    //    // ���� ����
    //    if (speedDelta.HasValue) info.CurSpeedMove += speedDelta.Value;
    //    if (jumpDelta.HasValue) JumpPower += jumpDelta.Value;
    //    if (attackDelta.HasValue) AttackPower += attackDelta.Value;
    //    if (defenseDelta.HasValue) info.CurDefGear += defenseDelta.Value;

    //    yield return new WaitForSeconds(duration);

    //    // ���� ����
    //    if (speedDelta.HasValue) info.CurSpeedMove -= speedDelta.Value;
    //    if (jumpDelta.HasValue) JumpPower -= jumpDelta.Value;
    //    if (attackDelta.HasValue) AttackPower -= attackDelta.Value;
    //    if (defenseDelta.HasValue) info.CurDefGear -= defenseDelta.Value;
    //}
}
