using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    public float MaxHealth { get; set; }
    public float MoveSpeed { get; set; }
    public float JumpPower { get; set; }
    public float AttackPower { get; set; }
    public float DefensePower { get; set; }
    public float CurrentHealth { get; set; }

    public event Action OnDeath;
    public event Action OnHealthChanged;

    public void Init(float maxHealth, float moveSpeed, float jumpPower, float attackPower, float defensePower)
    {
        MaxHealth = maxHealth;
        MoveSpeed = moveSpeed;
        JumpPower = jumpPower;
        AttackPower = attackPower;
        DefensePower = defensePower;
        CurrentHealth = MaxHealth;
    }
    // 아이템 사용, 장비 시 stat 변동
    public void SetModifier(float? health = null, float? speed = null, float? jump = null, float? attack = null, float? defense = null)
    {
        if (health.HasValue) MaxHealth = health.Value;
        if (speed.HasValue) MoveSpeed = speed.Value;
        if (jump.HasValue) JumpPower = jump.Value;
        if (attack.HasValue) AttackPower = attack.Value;
        if (defense.HasValue) DefensePower = defense.Value;
    }

    // 피격
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        OnHealthChanged?.Invoke();
        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            HandleDeath();
        }
    }
    public void SetHealth(float amount)
    {
        if (CurrentHealth <= 0f)
            return;

        CurrentHealth = amount;
        OnHealthChanged?.Invoke();
        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            //HandleDeath();
        }
    }
    // 회복
    public void Heal(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        OnHealthChanged?.Invoke();
    }
    // 사망
    public void HandleDeath()
    {
        // MyDebug.Log($"PlayerStatHandler HandleDeath : {CurrentHealth}"); 
        OnDeath?.Invoke();
    }
    // 지속시간 스탯 버프 처리
    public void ApplyTemporaryBuff(
    float? speedDelta = null,
    float? jumpDelta = null,
    float? attackDelta = null,
    float? defenseDelta = null,
    float duration = 5f)
    {
        StartCoroutine(ApplyBuffCoroutine(speedDelta, jumpDelta, attackDelta, defenseDelta, duration));
    }
    private IEnumerator ApplyBuffCoroutine(
    float? speedDelta,
    float? jumpDelta,
    float? attackDelta,
    float? defenseDelta,
    float duration)
    {
        // 스탯 변경
        if (speedDelta.HasValue) MoveSpeed += speedDelta.Value;
        if (jumpDelta.HasValue) JumpPower += jumpDelta.Value;
        if (attackDelta.HasValue) AttackPower += attackDelta.Value;
        if (defenseDelta.HasValue) DefensePower += defenseDelta.Value;

        yield return new WaitForSeconds(duration);

        // 스탯 복구
        if (speedDelta.HasValue) MoveSpeed -= speedDelta.Value;
        if (jumpDelta.HasValue) JumpPower -= jumpDelta.Value;
        if (attackDelta.HasValue) AttackPower -= attackDelta.Value;
        if (defenseDelta.HasValue) DefensePower -= defenseDelta.Value;
    }


}
