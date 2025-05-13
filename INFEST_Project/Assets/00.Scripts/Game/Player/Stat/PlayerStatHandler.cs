using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : NetworkBehaviour
{
    // Networked ������ ������ �ڵ� ����
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



    // ������ ���� �ٸ� �ɷ�ġ�� ����
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

    // �����丵�� ���� �޼���
    // StatData�� ����ϴ� ������ ����
    public void InitFromData(PlayerStatData data)
    {
        MaxHealth = data.maxHp;
        MoveSpeed = data.speedMove;
        JumpPower = 10; // �����Ϳ� ������ �⺻��
        AttackPower = 20; // �����Ϳ� ������ �⺻��
        DefensePower = data.def;
        CurrentHealth = MaxHealth;
        // �� �� �������� �߰�
    }
    public void SetToData(PlayerStatData data)
    {

    }



    // ������ ���, ��� �� stat ����
    public void SetModifier(int? health = null, int? speed = null, int? jump = null, int? attack = null, int? defense = null)
    {
        if (health.HasValue) MaxHealth = health.Value;
        if (speed.HasValue) MoveSpeed = speed.Value;
        if (jump.HasValue) JumpPower = jump.Value;
        if (attack.HasValue) AttackPower = attack.Value;
        if (defense.HasValue) DefensePower = defense.Value;
    }

    // �ǰ�
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
    // ȸ��
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        OnHealthChanged?.Invoke(amount);
    }
    // ���
    public void HandleDeath()
    {
        // MyDebug.Log($"PlayerStatHandler HandleDeath : {CurrentHealth}"); 
        IsDead = true;
        OnDeath?.Invoke();
    }
    // ������
    public void HandleRespawn()
    {
        OnRespawn?.Invoke();
    }
    // ���ӽð� ���� ���� ó��
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
        // ���� ����
        if (speedDelta.HasValue) MoveSpeed += speedDelta.Value;
        if (jumpDelta.HasValue) JumpPower += jumpDelta.Value;
        if (attackDelta.HasValue) AttackPower += attackDelta.Value;
        if (defenseDelta.HasValue) DefensePower += defenseDelta.Value;

        yield return new WaitForSeconds(duration);

        // ���� ����
        if (speedDelta.HasValue) MoveSpeed -= speedDelta.Value;
        if (jumpDelta.HasValue) JumpPower -= jumpDelta.Value;
        if (attackDelta.HasValue) AttackPower -= attackDelta.Value;
        if (defenseDelta.HasValue) DefensePower -= defenseDelta.Value;
    }
}
