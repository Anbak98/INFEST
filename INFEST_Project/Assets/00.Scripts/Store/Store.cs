using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour // 상점의 로직(무기 지급, UI띄어주기 등) 수행해준다.
{
    public StoreController _storeController;
    public Action<string> changeUI;
    public List<int> idList;
    public List<NetworkObject> itemPrefabList;
    public Transform weaponPibot;
    
    #region  상점 콜라이더 트리거 메소드
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

        Debug.Log("구매 전 :" + _player.characterInfoInstance.curGold + " ");

        if (idList[index] % 10000 < 600) // 무기
        {
            WeaponInstance _weaponInstance = new(idList[index]);

            _player.characterInfoInstance.curGold -= _weaponInstance.data.Price;
            //SpawnWeapon(_player,index);
            _player.inventory.AddWeponItme(_weaponInstance);
        }
        else if (idList[index] % 10000 < 1000) // 아이템
        {
            ConsumeInstance _consumeInstance    = new(idList[index]);

            _player.characterInfoInstance.curGold -= _consumeInstance.data.Price;

            _player.inventory.AddConsumeItme(_consumeInstance);
        }
        Debug.Log("구매 후 :" + _player.characterInfoInstance.curGold + " ");
        _storeController.uIShopView.UpdateButtonState();
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

        switch(index)
        {
            case 0: // 보조무기
                if (_player.inventory.auxiliaryWeapon[0] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.auxiliaryWeapon[0].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.auxiliaryWeapon[0], 0);
                break;
            case 1: // 주무기 1
                if (_player.inventory.weapon[0] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.weapon[0].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[0], 0);
                break;
            case 2: // 주무기 2
                if (_player.inventory.weapon[1] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.weapon[1].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[1], 1);
                break;
            case 3: // 아이템 1
                if (_player.inventory.consume[0] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[0].data.key).Price;
                _player.inventory.RemoveConsumeItem(0);
                break;
            case 4: // 아이템 2
                if (_player.inventory.consume[1] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[1].data.key).Price;
                _player.inventory.RemoveConsumeItem(1);
                break;
            case 5: // 아이템 3
                if (_player.inventory.consume[2] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[2].data.key).Price;
                _player.inventory.RemoveConsumeItem(2);
                break;
        }
         
        Debug.Log("판매 후 :" + _player.characterInfoInstance.curGold + "\n주무기 : " + _player.inventory.auxiliaryWeapon + "\n보조무기 : " + _player.inventory.weapon + "\n아이템 : " + _player.inventory.consume);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    /// <summary>
    /// 아이템 스폰
    /// </summary>
    /// <param name="player"></param>
    /// <param name="index"></param>
    public void SpawnWeapon(Player player, int index)
    {
        if (!HasStateAuthority) return;
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 2f;
        NetworkObject item = player.Runner.Spawn(itemPrefabList[index], spawnPosition, Quaternion.identity, player.Object.InputAuthority, (runner, obj) => obj.transform.SetParent(player.transform));

    }

    #endregion


#region 보충 메소드
    #region 탄약 보충
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryBulletSupplement(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TryBulletSupplement(_player, _playerRef, index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryBulletSupplement(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        WeaponInstance[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };

        if (weaponInv[index] == null) return;
        if (weaponInv[index].curBullet >= weaponInv[index].data.MagazineBullet) return;

        _player.characterInfoInstance.curGold -= weaponInv[index].data.BulletPrice * (weaponInv[index].data.MagazineBullet - weaponInv[index].curBullet);
        weaponInv[index].SupplementBullet();
        _storeController.uIShopView.WeaponSet(index);
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
    }
    #endregion

        #region 전부 보충
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryAllSupplement(Player _player, PlayerRef _playerRef, int index)
    {
        RPC_TrySale(_player, _playerRef, index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryAllSupplement(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        WeaponInstance[] weaponInv = { _player.inventory.auxiliaryWeapon[0], _player.inventory.weapon[0], _player.inventory.weapon[1] };
        ConsumeInstance[] itemInv = { _player.inventory.consume[0], _player.inventory.consume[1], _player.inventory.consume[2] };

        int weaponPrice = 0;
        int itemPrice  = 0;
        int totalprice = 0;

        #region 무기
        if (weaponInv[0] != null)
        {
            weaponPrice += (weaponInv[0].data.MagazineBullet - weaponInv[0].curBullet) * weaponInv[0].data.BulletPrice;
        }

        if (weaponInv[1] != null)
        {
            weaponPrice += (weaponInv[1].data.MagazineBullet - weaponInv[1].curBullet) * weaponInv[1].data.BulletPrice;
        }

        if (weaponInv[2] != null)
        {
            weaponPrice += (weaponInv[2].data.MagazineBullet - weaponInv[2].curBullet) * weaponInv[2].data.BulletPrice;
        }
        #endregion
        #region 아이템
        if (itemInv[0] != null)
        {
            itemPrice += (itemInv[0].data.MaxNum - itemInv[0].curNum) * itemInv[0].data.Price;
        }

        if (itemInv[1] != null)
        {
            itemPrice += (itemInv[1].data.MaxNum - itemInv[1].curNum) * itemInv[index].data.Price;
        }

        if (itemInv[2] != null)
        {
            itemPrice += (itemInv[2].data.MaxNum - itemInv[2].curNum) * itemInv[index].data.Price;
        }
        #endregion
        if(_player.characterInfoInstance.curDefGear >= _player.characterInfoInstance.data.DefGear)
        {
            totalprice += weaponPrice + itemPrice;
        }
        else
        {
            totalprice += 500 + weaponPrice + itemPrice;
        }

        for (int i = 0; i < 3; i++)
        {
            if (weaponInv[i] != null)
                weaponInv[i].SupplementBullet();
            if(itemInv[i] != null)
                itemInv[i].AddNum();
        }



    }
    #endregion
#endregion










}
