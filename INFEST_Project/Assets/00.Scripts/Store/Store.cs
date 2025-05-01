using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour // 상점의 로직(무기 지급, UI띄어주기 등) 수행해준다.
{
    public StoreController _storeController;
    public Action<string> changeUI;
    public List<int> idList;
    public InputManager inputManager;

    #region  상점 콜라이더 트리거 메소드

    #region 상호작용시
    /// <summary>
    /// 상호작용시 요청하는 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestInteraction(Player _player, PlayerRef _playerRef)
    {
        RPC_Interaction(_player, _playerRef);

        if (_storeController.activeTime)
        {
            _storeController.RPC_Timer(_playerRef);
        }
    }

    /// <summary>
    /// 상호작용 로직
    /// </summary>
    /// 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Interaction(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        // if (_playerRef != _player.Runner.LocalPlayer) return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _storeController.uIShopView.UpdateButtonState();

        //isInteraction = false;
        _storeController.uIShopView.bg.gameObject.SetActive(true);
        _storeController.uIShopView.profile.gameObject.SetActive(true);
        _storeController.uIShopView.interactionText.gameObject.SetActive(false); 
    }
    #endregion

    #region 상호작용해제시
    /// <summary>
    /// 상호작용해제시 요청하는 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestStopInteraction(PlayerRef _playerRef)
    {
        RPC_StopInteraction(_playerRef);

    }
    /// <summary>
    /// 상호작용 해제로직
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_StopInteraction([RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        _storeController.uIShopView.interactionText.gameObject.SetActive(true);
        _storeController.uIShopView.bg.gameObject.SetActive(false);
        _storeController.uIShopView.profile.gameObject.SetActive(false);
    }
    #endregion

    #region OnTriggerEnter 호출
    /// <summary>
    /// 상점의 영역에 들어갔을때 요청하는 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestEnterShopZone(Player _player, PlayerRef _playerRef)
    {
        RPC_EnterShopZone(_player, _playerRef);
    }

    /// <summary>
    /// 상점 영역 로직
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EnterShopZone(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        _storeController.uIShopView.bg.gameObject.SetActive(false);
        _storeController.uIShopView.profile.gameObject.SetActive(false);
        _storeController.uIShopView.interactionText.gameObject.SetActive(true);
    }
    #endregion

    #region OnTriggerExit 호출
    /// <summary>
    /// 상점을 떠났을때 요청하는 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestLeaveShopZone(Player _player, PlayerRef _playerRef)
    {
        RPC_LeaveShopZone(_player, _playerRef);
    }

    /// <summary>
    /// 상점을 떠났을때
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_LeaveShopZone(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _player.isInteraction = false;
        _storeController.uIShopView.bg.gameObject.SetActive(false);
        _storeController.uIShopView.profile.gameObject.SetActive(false);
        _storeController.uIShopView.interactionText.gameObject.SetActive(false);
    }
    #endregion

#endregion

#region 상점 구매 & 판매 메소드

    #region 구매
    /// <summary>
    /// 구매 요청 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryBuy(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TryBuy(_player, _playerRef, index);
    }

    /// <summary>
    /// 구매 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryBuy(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        if (idList[index] == 0) return;

        if (_player == null) return;

        Weapon _buyWeapon = null;

        if (idList[index] % 10000 < 700) // 무기
        {

            for (int i = 0; i< _player.Weapons.Weapons.Count; i++)
            {
                if (_player.Weapons.Weapons[i].key == idList[index])
                {
                    _buyWeapon = _player.Weapons.Weapons[i];
                    break;
                }
            }
            _player.characterInfoInstance.curGold -= _buyWeapon.instance.data.Price;
            _player.inventory.AddWeponItme(_buyWeapon);
            _buyWeapon.IsCollected = true;
            _player.Weapons._weapons.Add(_buyWeapon);
        }
        else if (idList[index] % 10000 < 1000) // 아이템
        {
            ConsumeInstance _consumeInstance = new(idList[index]);

            _player.characterInfoInstance.curGold -= _consumeInstance.data.Price;

            _player.inventory.AddConsumeItme(_consumeInstance);
        }
        var inv = _player.inventory;
        int[] invKey = {inv.auxiliaryWeapon[0] != null? inv.auxiliaryWeapon[0].instance.data.key : 0,
                        inv.weapon[0] != null? inv.weapon[0].instance.data.key : 0,
                        inv.weapon[1] != null? inv.weapon[1].instance.data.key : 0,
                        inv.consume[0] != null? inv.consume[0].data.key : 0,
                        inv.consume[1] != null? inv.consume[1].data.key : 0,
                        inv.consume[2] != null? inv.consume[2].data.key : 0};

        for (int i = 0; i < invKey.Length; i++)
        {
            if (invKey[i] == idList[index])
            {
                _storeController.uIShopView.SaleSet(i);
                if (i < 3)
                _storeController.uIShopView.WeaponSet(i);
                else
                _storeController.uIShopView.ItemSet(i-3);
            }
        }
        _storeController.uIShopView.UpdateButtonState();
        _storeController.uIShopView.UpdateSaleButtonState();



    }
    #endregion

    #region 판매
    /// <summary>
    /// 판매 요청 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTrySale(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TrySale(_player, _playerRef, index);
    }

    /// <summary>
    /// 판매 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TrySale(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        if (_player == null) return;
        

        switch (index)
        {
            case 0: // 보조무기
                if (_player.inventory.auxiliaryWeapon[0] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.auxiliaryWeapon[0].instance.data.Price / 2;


                    if (_player.inventory.equippedWeapon == _player.inventory.auxiliaryWeapon[0])
                    _player.Weapons.Swap(-1,true);

                //_player.Weapons.Swap(1);
                //    else if (_player.inventory.equippedWeapon == _player.Weapons._weapons[_player.Weapons._weapons.Count - 1])
                //        _player.Weapons.Swap(-1);
                //    else
                //        _player.Weapons.Swap(0);

                _player.inventory.RemoveWeaponItem(_player.inventory.auxiliaryWeapon[0], 0);

                //_player.Weapons._weapons.Remove(_player.inventory.auxiliaryWeapon[0]);
                _storeController.uIShopView.WeaponSet(0);
                break;
            case 1: // 주무기 1
                if (_player.inventory.weapon[0] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.weapon[0].instance.data.Price / 2;

          
                    if (_player.inventory.equippedWeapon == _player.inventory.weapon[0])
                        _player.Weapons.Swap(-1, true);

                //_player.Weapons.Swap(1);
                //    else if (_player.inventory.equippedWeapon == _player.Weapons._weapons[_player.Weapons._weapons.Count - 1])
                //        _player.Weapons.Swap(-1);
                //    else
                //        _player.Weapons.Swap(0);


                //_player.inventory.weapon[0].curBullet = _player.inventory.weapon[0].instance.data.MaxBullet;
                //_player.inventory.weapon[0].curMagazineBullet = _player.inventory.weapon[0].instance.data.MagazineBullet;
                //_player.inventory.weapon[0].IsCollected = false;
                //_player.Weapons._weapons.Remove(_player.inventory.weapon[0]); 
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[0], 0);

                _storeController.uIShopView.WeaponSet(1);
                break;
            case 2: // 주무기 2
                if (_player.inventory.weapon[1] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.weapon[1].instance.data.Price / 2;

                
                    if (_player.inventory.equippedWeapon == _player.inventory.weapon[1])
                    _player.Weapons.Swap(-1, true);

                //_player.Weapons.Swap(1);
                //    else if(_player.inventory.equippedWeapon == _player.Weapons._weapons[_player.Weapons._weapons.Count-1])
                //        _player.Weapons.Swap(-1);
                //    else
                //        _player.Weapons.Swap(0);
                //_player.inventory.weapon[1].curBullet = _player.inventory.weapon[1].instance.data.MaxBullet;
                //_player.inventory.weapon[1].curMagazineBullet = _player.inventory.weapon[1].instance.data.MagazineBullet;
                //_player.inventory.weapon[1].IsCollected = false;
                //_player.Weapons._weapons.Remove(_player.inventory.weapon[1]);
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[1], 1);

                _storeController.uIShopView.WeaponSet(2);
                break;
            case 3: // 아이템 1
                if (_player.inventory.consume[0] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.consume[0].data.Price / 2;
                _player.inventory.RemoveConsumeItem(0);
                _storeController.uIShopView.ItemSet(0);
                break;
            case 4: // 아이템 2
                if (_player.inventory.consume[1] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.consume[1].data.Price / 2;
                _player.inventory.RemoveConsumeItem(1);
                _storeController.uIShopView.ItemSet(1);
                break;
            case 5: // 아이템 3
                if (_player.inventory.consume[2] == null) return;
                _player.characterInfoInstance.curGold += _player.inventory.consume[2].data.Price / 2;
                _player.inventory.RemoveConsumeItem(2);
                _storeController.uIShopView.ItemSet(2);
                break;
        }

        Debug.Log("판매 후 :" + _player.characterInfoInstance.curGold + "\n주무기 : " + _player.inventory.auxiliaryWeapon + "\n보조무기 : " + _player.inventory.weapon + "\n아이템 : " + _player.inventory.consume);
        _storeController.uIShopView.UpdateButtonState();
        _storeController.uIShopView.UpdateSaleButtonState();

    }
    #endregion
#endregion

#region 보충 메소드

    #region 전부 보충
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryAllSupplement(Player _player, PlayerRef _playerRef)
    {
        RPC_TryAllSupplement(_player, _playerRef);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryAllSupplement(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        Weapon[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };
        ConsumeInstance[] itemInv = { _player.inventory.consume[0], _player.inventory.consume[1], _player.inventory.consume[2] };

        int weaponPrice = 0;
        int itemPrice = 0;
        int totalprice = 0;

        #region 무기
        if (weaponInv[0] != null)
        {
            weaponPrice += (weaponInv[0].instance.data.MaxBullet - weaponInv[0].curBullet) * weaponInv[0].instance.data.BulletPrice;
        }

        if (weaponInv[1] != null)
        {
            weaponPrice += (weaponInv[1].instance.data.MaxBullet - weaponInv[1].curBullet) * weaponInv[1].instance.data.BulletPrice;
        }

        if (weaponInv[2] != null)
        {
            weaponPrice += (weaponInv[2].instance.data.MaxBullet - weaponInv[2].curBullet) * weaponInv[2].instance.data.BulletPrice;
        }
        #endregion
        #region 아이템
        if (itemInv[0] != null)
        {
            itemPrice += (itemInv[0].data.MaxNum - itemInv[0].curNum) * itemInv[0].data.Price;
        }

        if (itemInv[1] != null)
        {
            itemPrice += (itemInv[1].data.MaxNum - itemInv[1].curNum) * itemInv[1].data.Price;
        }

        if (itemInv[2] != null)
        {
            itemPrice += (itemInv[2].data.MaxNum - itemInv[2].curNum) * itemInv[2].data.Price;
        }
        #endregion

        if (_player.characterInfoInstance.curDefGear >= 200)
        {
            totalprice += weaponPrice + itemPrice;
        }
        else
        {
            totalprice += 500 + weaponPrice + itemPrice;
        }

        if (totalprice > _player.characterInfoInstance.curGold) return;

        for (int i = 0; i < 3; i++)
        {
            if (weaponInv[i] != null)
            {
                while (weaponInv[i].instance.data.MaxBullet > weaponInv[i].curBullet)
                {
                    weaponInv[i].SupplementBullet();
                }
                _storeController.uIShopView.WeaponSet(i);
            }

            if (itemInv[i] != null)
            {
                while (itemInv[i].data.MaxNum > itemInv[i].curNum)
                {
                    itemInv[i].AddNum();
                }
                _storeController.uIShopView.ItemSet(i);
            }

        }

        _player.characterInfoInstance.curGold -= totalprice;
        _player.characterInfoInstance.curDefGear += 200;
        _player.characterInfoInstance.curDefGear = Mathf.Min(_player.characterInfoInstance.curDefGear, 200);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #region 방어구 보충
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryDefSupplement(Player _player, PlayerRef _playerRef)
    {
        RPC_TryDefSupplement(_player, _playerRef);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryDefSupplement(Player _player, [RpcTarget] PlayerRef _playerRef)
    {
        if (Player.local.characterInfoInstance.curDefGear >= 200) return;
        if (Player.local.characterInfoInstance.curGold < 500) return;


        Player.local.characterInfoInstance.curGold -= 500;
        Player.local.characterInfoInstance.curDefGear += 200;
        Player.local.characterInfoInstance.curDefGear = Mathf.Min(Player.local.characterInfoInstance.curDefGear, 200);
        _storeController.uIShopView.UpdateButtonState();
    }

    #endregion

    #region 탄약 보충
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryBulletSupplement(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TryBulletSupplement(_player, _playerRef, index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryBulletSupplement(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        Weapon[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };

        if (weaponInv[index] == null) return;
        if (weaponInv[index].curBullet >= weaponInv[index].instance.data.MaxBullet) return;

        if (weaponInv[index].curBullet + weaponInv[index].instance.data.MagazineBullet >= weaponInv[index].instance.data.MaxBullet)
            _player.characterInfoInstance.curGold -= weaponInv[index].instance.data.BulletPrice * (weaponInv[index].instance.data.MaxBullet - weaponInv[index].curBullet);
        else
            _player.characterInfoInstance.curGold -= weaponInv[index].instance.data.BulletPrice * (weaponInv[index].instance.data.MagazineBullet);

        weaponInv[index].SupplementBullet();
        _storeController.uIShopView.WeaponSet(index);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #region 아이템 보충
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryItmeSupplement(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TryItmeSupplement(_player, _playerRef, index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryItmeSupplement(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        ConsumeInstance[] itemInv = { _player.inventory.consume[0], _player.inventory.consume[1], _player.inventory.consume[2] };

        if (itemInv[index] == null) return;
        if (itemInv[index].curNum >= itemInv[index].data.MaxNum) return;

        _player.characterInfoInstance.curGold -= itemInv[index].data.Price;
        itemInv[index].AddNum();
        _storeController.uIShopView.ItemSet(index);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    #endregion










}
