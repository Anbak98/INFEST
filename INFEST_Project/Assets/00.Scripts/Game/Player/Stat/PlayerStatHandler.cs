using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : NetworkBehaviour
{
    public CharacterInfoInstance info;

    [Networked] public bool IsDead { get; set; }

    public event Action OnDeath;
    public event Action OnRespawn;
    public event Action OnHealthChanged;

    public override void Spawned()
    {
        base.Spawned();


        info = new(1);
    }

    // 피격
    public void TakeDamage(int amount)
    {
        int damage = amount - info.CurDef;

        if (damage <= 0)
        {
            damage = 1;
        }

        if (info.CurDefGear > damage)
        {
            info.CurDefGear -= damage;
        }
        else if (info.CurDefGear > 0)
        {
            damage -= info.CurDefGear;
            info.CurDefGear = 0;

            info.CurHealth -= damage;
            OnHealthChanged?.Invoke();
            if (info.CurHealth <= 0)
            {
                info.CurHealth = 0;
                HandleDeath();
            }
        }

        if (info.CurDefGear < 0)
            info.CurDefGear = 0;
    }

    public void SetHealth(int amount)
    {
        //if (CurrentHealth <= 0)
        //    return;

        info.CurHealth = amount;
        OnHealthChanged?.Invoke();
        if (info.CurHealth <= 0)
        {
            info.CurHealth = 0;
            //HandleDeath();
        }
        else if (IsDead && info.CurHealth > 0)
        {
            IsDead = false;
            HandleRespawn();
        }
    }

    // 회복
    public void Heal(int amount)
    {
        info.CurHealth = Mathf.Min(info.CurHealth + amount, info.CurHealth);
        OnHealthChanged?.Invoke();
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
