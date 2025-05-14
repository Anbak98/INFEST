using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : NetworkBehaviour
{
    public CharacterInfoInstance info;

    [Networked] public bool IsDead { get; set; }

    [Networked] public int CurSpeedMove { get; set; }
    [Networked] public int CurHealth { get; set; }                          // 체력
    [Networked] public int CurDefGear { get; set; }                         // 방어구 체력
    [Networked] public int CurDef { get; set; }
    [Networked] public int CurGold { get; set; }                            // 시작 골드
    [Networked] public int CurTeamCoin { get; set; }                        // 시작 팀코인
    [Networked] public int curstate { get; set; }                           // 캐릭터 상태

    public event Action OnDeath;
    public event Action OnRespawn;
    public event Action<float> OnHealthChanged;


    public override void Spawned()
    {
        base.Spawned();

        info = new(1);
        CurHealth = info.data.Health;
        CurDefGear = info.data.DefGear;
        CurDef = info.data.Def;
        CurGold = info.data.StartGold;
        CurTeamCoin = info.data.StartTeamCoin;
        CurSpeedMove = info.data.SpeedMove;
        curstate = info.data.State;
    }

    // 피격
    public void TakeDamage(int amount)
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
            damage -= CurDefGear;
            CurDefGear = 0;

            CurHealth -= damage;
            OnHealthChanged?.Invoke(-amount);
            if (CurHealth <= 0)
            {
                CurHealth = 0;
                HandleDeath();
            }
        }

        if (CurDefGear < 0)
            CurDefGear = 0;

        Debug.Log(CurHealth);
    }

    public void SetHealth(int amount)
    {
        //if (CurrentHealth <= 0)
        //    return;

        CurHealth = amount;
        OnHealthChanged?.Invoke(amount);
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

    // 회복
    public void Heal(int amount)
    {
        CurHealth = Mathf.Min(CurHealth + amount, CurHealth);
        OnHealthChanged?.Invoke(amount);
    }
    // 사망
    public void HandleDeath()
    {
        // MyDebug.Log($"PlayerStatHandler HandleDeath : {CurrentHealth}"); 
        IsDead = true;

        OnDeath?.Invoke();
    }
    // 리스폰
    public void HandleRespawn()
    {
        OnRespawn?.Invoke();
    }

    //// 지속시간 스탯 버프 처리
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
    //    // 스탯 변경
    //    if (speedDelta.HasValue) info.CurSpeedMove += speedDelta.Value;
    //    if (jumpDelta.HasValue) JumpPower += jumpDelta.Value;
    //    if (attackDelta.HasValue) AttackPower += attackDelta.Value;
    //    if (defenseDelta.HasValue) info.CurDefGear += defenseDelta.Value;

    //    yield return new WaitForSeconds(duration);

    //    // 스탯 복구
    //    if (speedDelta.HasValue) info.CurSpeedMove -= speedDelta.Value;
    //    if (jumpDelta.HasValue) JumpPower -= jumpDelta.Value;
    //    if (attackDelta.HasValue) AttackPower -= attackDelta.Value;
    //    if (defenseDelta.HasValue) info.CurDefGear -= defenseDelta.Value;
    //}
}
