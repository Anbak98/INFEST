using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : NetworkBehaviour
{
    // Networked 붙으면 서버로 자동 전송
    [Networked] public int MaxHealth { get; set; }
    [Networked] public int MoveSpeed { get; set; }
    [Networked] public int MoveSpeedModifier { get; set; }
    [Networked] public int RotationDamping { get; set; }

    [Networked] public int JumpPower { get; set; }
    [Networked] public int AttackPower { get; set; }
    [Networked] public int DefensePower { get; set; }
    [Networked] public int CurrentHealth { get; set; }
    [Networked] public bool IsDead { get; set; }

    public event Action OnDeath;
    public event Action OnRespawn;
    public event Action<float> OnHealthChanged;



    // 직업에 따라 다른 능력치로 생성
    public void Init(int maxHealth, int moveSpeed, int moveSpeedModifier, int rotationDamping, int jumpPower, int attackPower, int defensePower)
    {
        MaxHealth = maxHealth;
        MoveSpeed = moveSpeed;
        MoveSpeedModifier = moveSpeedModifier;
        RotationDamping = rotationDamping;
        JumpPower = jumpPower;
        AttackPower = attackPower;
        DefensePower = defensePower;
        CurrentHealth = MaxHealth;
        IsDead = false;
    }

    // 리팩토링을 위한 메서드
    // StatData를 사용하는 식으로 개선
    public void InitFromData(PlayerStatData data)
    {
        MaxHealth = data.maxHp;
        MoveSpeed = data.speedMove;
        JumpPower = 10; // 데이터에 없으면 기본값
        AttackPower = 20; // 데이터에 없으면 기본값
        DefensePower = data.def;
        CurrentHealth = MaxHealth;
        // 그 외 여러가지 추가
    }
    public void SetToData(PlayerStatData data)
    {

    }



    // 아이템 사용, 장비 시 stat 변동
    public void SetModifier(int? health = null, int? speed = null, int? jump = null, int? attack = null, int? defense = null)
    {
        if (health.HasValue) MaxHealth = health.Value;
        if (speed.HasValue) MoveSpeed = speed.Value;
        if (jump.HasValue) JumpPower = jump.Value;
        if (attack.HasValue) AttackPower = attack.Value;
        if (defense.HasValue) DefensePower = defense.Value;
    }

    // 피격
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        OnHealthChanged?.Invoke(-amount);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            HandleDeath();
        }
    }
    public void SetHealth(int amount)
    {
        //if (CurrentHealth <= 0)
        //    return;

        CurrentHealth = amount;
        OnHealthChanged?.Invoke(amount);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            //HandleDeath();
        }
        else if (IsDead && CurrentHealth > 0)
        {
            IsDead = false;
            HandleRespawn();
        }
    }
    // 회복
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
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
    // 지속시간 스탯 버프 처리
    public void ApplyTemporaryBuff(
    int? speedDelta = null,
    int? jumpDelta = null,
    int? attackDelta = null,
    int? defenseDelta = null,
    int duration = 5)
    {
        StartCoroutine(ApplyBuffCoroutine(speedDelta, jumpDelta, attackDelta, defenseDelta, duration));
    }
    private IEnumerator ApplyBuffCoroutine(
    int? speedDelta,
    int? jumpDelta,
    int? attackDelta,
    int? defenseDelta,
    int duration)
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
