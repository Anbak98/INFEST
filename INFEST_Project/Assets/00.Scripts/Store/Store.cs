using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour // ������ ����(���� ����, UI����ֱ� ��) �������ش�.
{
    public StoreController _storeController;
    public Action<string> changeUI;
    public List<int> idList;
    public List<NetworkObject> itemPrefabList;
    public Transform weaponPibot;
    
    #region  ���� �ݶ��̴� Ʈ���� �޼ҵ�
    /// <summary>
    /// ��ȣ�ۿ�� ��û�ϴ� �޼ҵ�
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
    /// ��ȣ�ۿ� ����
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
    /// ��ȣ�ۿ������� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestStopInteraction(PlayerRef _playerRef)
    {
        RPC_StopInteraction(_playerRef);

    }
    /// <summary>
    /// ��ȣ�ۿ� ��������
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
    /// ������ ������ ������ ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestEnterShopZone(Player _player, PlayerRef _playerRef)
    {
        RPC_EnterShopZone(_player, _playerRef);
    }

    /// <summary>
    /// ���� ���� ����
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
    /// ������ �������� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestLeaveShopZone(Player _player, PlayerRef _playerRef)
    {
        RPC_LeaveShopZone(_player, _playerRef);
    }

    /// <summary>
    /// ������ ��������
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

    #region ���� ���� & �Ǹ� �޼ҵ�

        #region ����
    /// <summary>
    /// ���� ��û �޼ҵ�
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
    /// ���� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    /// <param name="index"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryBuy(Player _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        if (idList[index] == 0) return;

        if (_player == null) return;

        Debug.Log("���� �� :" + _player.characterInfoInstance.curGold + " ");

        if (idList[index] % 10000 < 600) // ����
        {
            WeaponInstance _weaponInstance = new(idList[index]);

            _player.characterInfoInstance.curGold -= _weaponInstance.data.Price;
            //SpawnWeapon(_player,index);
            _player.inventory.AddWeponItme(_weaponInstance);
        }
        else if (idList[index] % 10000 < 1000) // ������
        {
            ConsumeInstance _consumeInstance    = new(idList[index]);

            _player.characterInfoInstance.curGold -= _consumeInstance.data.Price;

            _player.inventory.AddConsumeItme(_consumeInstance);
        }
        Debug.Log("���� �� :" + _player.characterInfoInstance.curGold + " ");
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

        #region �Ǹ�

    /// <summary>
    /// �Ǹ� ��û �޼ҵ�
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
    /// �Ǹ� �޼ҵ�
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
            case 0: // ��������
                if (_player.inventory.auxiliaryWeapon[0] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.auxiliaryWeapon[0].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.auxiliaryWeapon[0], 0);
                break;
            case 1: // �ֹ��� 1
                if (_player.inventory.weapon[0] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.weapon[0].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[0], 0);
                break;
            case 2: // �ֹ��� 2
                if (_player.inventory.weapon[1] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.weapon[1].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[1], 1);
                break;
            case 3: // ������ 1
                if (_player.inventory.consume[0] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[0].data.key).Price;
                _player.inventory.RemoveConsumeItem(0);
                break;
            case 4: // ������ 2
                if (_player.inventory.consume[1] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[1].data.key).Price;
                _player.inventory.RemoveConsumeItem(1);
                break;
            case 5: // ������ 3
                if (_player.inventory.consume[2] == null) return;
                _player.characterInfoInstance.curGold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[2].data.key).Price;
                _player.inventory.RemoveConsumeItem(2);
                break;
        }
         
        Debug.Log("�Ǹ� �� :" + _player.characterInfoInstance.curGold + "\n�ֹ��� : " + _player.inventory.auxiliaryWeapon + "\n�������� : " + _player.inventory.weapon + "\n������ : " + _player.inventory.consume);
        _storeController.uIShopView.UpdateButtonState();
    }
    #endregion

    /// <summary>
    /// ������ ����
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


#region ���� �޼ҵ�
    #region ź�� ����
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

        #region ������ ����
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

        #region ���� ����
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

        #region ����
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
        #region ������
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
