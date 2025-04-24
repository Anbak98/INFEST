using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Store : NetworkBehaviour // ������ ����(���� ����, UI����ֱ� ��) �������ش�.
{
    public StoreController _storeController;
    public Action<string> changeUI;
    public List<int> idList;
    public List<NetworkObject> itemPrefabList;
    public Transform weaponPibot;
    private int[] _buyArr = new int[6];
    
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
        if (_playerRef != _player.Runner.LocalPlayer) return;

        _storeController.uIShopView.bg.gameObject.SetActive(false);
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
        _storeController.uIShopView.interactionText.gameObject.SetActive(false);
    }
    #endregion

    #region ���� ���� & �Ǹ� �޼ҵ�
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

        Debug.Log("���� �� :" + _player.gold + " ");

        if (idList[index] % 10000 < 600) // ����
        {
            WeaponInstance _weaponInstance = new(idList[index]);

            _player.gold -= _weaponInstance.data.Price;
            //SpawnWeapon(_player,index);
            _player.inventory.AddWeponItme(_weaponInstance);
        }
        else if (idList[index] % 10000 < 1000) // ������
        {
            ConsumeInstance _consumeInstance = new(idList[index]);

            _player.gold -= _consumeInstance.data.Price;

            _player.inventory.AddConsumeItme(_consumeInstance);
        }
        Debug.Log("���� �� :" + _player.gold + " ");
    }
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
                _player.gold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.auxiliaryWeapon[0].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.auxiliaryWeapon[0], 0);
                break;
            case 1: // �ֹ��� 1
                if (_player.inventory.weapon[0] == null) return;
                _player.gold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.weapon[0].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[0], 0);
                break;
            case 2: // �ֹ��� 2
                if (_player.inventory.weapon[1] == null) return;
                _player.gold += DataManager.Instance.GetByKey<WeaponInfo>(_player.inventory.weapon[1].data.key).Price;
                _player.inventory.RemoveWeaponItem(_player.inventory.weapon[1], 1);
                break;
            case 3: // ������ 1
                if (_player.inventory.consume[0] == null) return;
                _player.gold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[0].data.key).Price;
                _player.inventory.RemoveConsumeItem(0);
                break;
            case 4: // ������ 2
                if (_player.inventory.consume[1] == null) return;
                _player.gold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[1].data.key).Price;
                _player.inventory.RemoveConsumeItem(1);
                break;
            case 5: // ������ 3
                if (_player.inventory.consume[2] == null) return;
                _player.gold += DataManager.Instance.GetByKey<ConsumeItem>(_player.inventory.consume[2].data.key).Price;
                _player.inventory.RemoveConsumeItem(2);
                break;
        }
         
        Debug.Log("�Ǹ� �� :" + _player.gold + "\n�ֹ��� : " + _player.inventory.auxiliaryWeapon + "\n�������� : " + _player.inventory.weapon + "\n������ : " + _player.inventory.consume);

    }

    public void SpawnWeapon(Player player, int index)
    {
        if (!HasStateAuthority) return;
        Vector3 spawnPosition = player.transform.position + player.transform.forward * 2f;
        NetworkObject item = player.Runner.Spawn(itemPrefabList[index], spawnPosition, Quaternion.identity, player.networkObject.InputAuthority, (runner, obj) => obj.transform.SetParent(player.transform));

    }

    #endregion

}
