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

        //Player _player = GetComponent<Player>();
        //if (_player == null)
        //{

        //    for (int i = 0; i < _player.Weapons.Weapons.Count; i++)
        //    {
        //        if (_player.Weapons.Weapons[i].key == info.data.StartAuxiliaryWeapon)
        //        {
        //            _player.inventory.auxiliaryWeapon[0] = _player.Weapons.Weapons[i];
        //            _player.inventory.auxiliaryWeapon[0].IsCollected = true;
        //        }

        //        if (_player.Weapons.Weapons[i].key == info.data.StartWeapon1)
        //        {
        //            _player.inventory.weapon[0] = _player.Weapons.Weapons[i];
        //            _player.inventory.weapon[0].IsCollected = true;
        //        }

        //        if (_player.inventory.auxiliaryWeapon[0] != null && _player.inventory.weapon[0] != null)
        //            break;
        //    }

        //    for (int i = 0; i < _player.Consumes.Consumes.Count; i++)
        //    {
        //        if (_player.Consumes.Consumes[i].key == _player.statHandler.info.data.StartConsumeItem1)
        //        {
        //            int itemChk = _player.statHandler.info.data.StartConsumeItem1 % 10000;
        //            bool throwingWeapon = itemChk < 800 && itemChk > 700;
        //            bool recoveryItem = itemChk < 900 && itemChk > 800;
        //            bool shieldItme = itemChk < 1000 && itemChk > 900;

        //            if (throwingWeapon)
        //                _player.inventory.consume[0] = _player.Consumes.Consumes[i];

        //            if (recoveryItem)
        //                _player.inventory.consume[1] = _player.Consumes.Consumes[i];

        //            if (shieldItme)
        //                _player.inventory.consume[2] = _player.Consumes.Consumes[i];
        //            break;

        //        }

        //    }
        //    _player.inventory.equippedWeapon = _player.inventory.auxiliaryWeapon[0];
        //    _player.inventory.consume[0] = _player.Consumes.Consumes[2];
        //    _player.inventory.consume[1] = _player.Consumes.Consumes[3];
        //}
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
