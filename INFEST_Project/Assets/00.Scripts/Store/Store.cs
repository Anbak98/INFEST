using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Store : NetworkBehaviour // ������ ����(���� ����, UI����ֱ� ��) �������ش�.
{
    public StoreController _storeController;
    public Action<string> changeUI;

    public List<int> idList;
    public UIStore uIStore;

    // �÷��̾� ���
    // �÷��̾� ��
    // �� ���
    // ������ �����͵�

    // ��ȣ�ۿ� üũ�� �Ұ�
    [Networked] public NetworkBool isInteraction { get; set; } = false; // �÷��̾�� �־�� �ҰͰ���. �����丵 ����

    #region  ���� �ݶ��̴� Ʈ���� �޼ҵ�
    /// <summary>
    /// ��ȣ�ۿ�� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestInteraction(NetworkObject _player, PlayerRef _playerRef)
    {
        RPC_Interaction(_player, _playerRef);
    }

    /// <summary>
    /// ��ȣ�ۿ� ����
    /// </summary>
    /// 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Interaction(NetworkObject _player, [RpcTarget] PlayerRef _playerRef)
    {
        // if (_playerRef != _player.Runner.LocalPlayer) return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        uIStore = _player.GetComponentInChildren<UIStore>();
        if (uIStore == null) return;

        isInteraction = false;
        uIStore.panel.gameObject.SetActive(true);
        uIStore.interactionText.gameObject.SetActive(false);
        if (_storeController.activeTime)
        {
            _storeController.storeTimer = TickTimer.CreateFromSeconds(_storeController.Runner, _storeController.activateTime);
            _storeController.activeTime = false;
        }

    }

    /// <summary>
    /// ������ ������ ������ ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestEnterShopZone(NetworkObject _player, PlayerRef _playerRef)
    {
        RPC_EnterShopZone(_player, _playerRef);

    }

    /// <summary>
    /// ���� ���� ����
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EnterShopZone(NetworkObject _player, [RpcTarget] PlayerRef _playerRef)
    {
        //if (_playerRef != _player.Runner.LocalPlayer) return;

        uIStore = _player.GetComponentInChildren<UIStore>();
        if (uIStore == null) return;

        uIStore.panel.gameObject.SetActive(false);
        uIStore.interactionText.gameObject.SetActive(true);
    }

    /// <summary>
    /// ������ �������� ��û�ϴ� �޼ҵ�
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestLeaveShopZone(NetworkObject _player, PlayerRef _playerRef)
    {
        RPC_LeaveShopZone(_player, _playerRef);
    }

    /// <summary>
    /// ������ �������� + ��ȣ�ۿ� ���� ����
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_LeaveShopZone(NetworkObject _player, [RpcTarget] PlayerRef _playerRef)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //if (Runner.LocalPlayer != _playerRef) return;

        uIStore = _player.GetComponentInChildren<UIStore>();
        if (uIStore == null) return;

        uIStore.interactionText.gameObject.SetActive(false);
        uIStore.panel.gameObject.SetActive(false);
    }


    #endregion


    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTryBuy(NetworkObject _player, PlayerRef _playerRef, int index)
    {
        RPC_TryBuy(_player, _playerRef, index);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TryBuy(NetworkObject _player, [RpcTarget] PlayerRef _playerRef, int index)
    {
        if (idList[index] == 0) return;

        Player player = _player.GetComponent<Player>();
        if (player == null) return;

        Debug.Log("���� �� :" + player.gold);

        if (idList[index] % 10000 < 600) // ����
        {
            WeaponInstance _weaponInstance = new(idList[index]);

            player.gold -= _weaponInstance.data.Price;

            player.inventory.AddWeponItme(_weaponInstance);
        }
        else if (idList[index] % 10000 < 1000) // ������
        {
            ConsumeInstance _consumeInstance = new(idList[index]);
            player.inventory.AddConsumeItme(_consumeInstance);
        }

        Debug.Log("���� �� :" + player.gold);

    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTrySale(NetworkObject _player, PlayerRef _playerRef, int index, EBtnType type)
    {
        RPC_TrySale(_player, _playerRef, index, type);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_TrySale(NetworkObject _player, [RpcTarget] PlayerRef _playerRef, int index, EBtnType type)
    {
        Player player = _player.GetComponent<Player>();

        Debug.Log("�Ǹ� �� :" + player.gold);
        if (player == null) return;

        if (idList[index] % 10000 < 200 && player.inventory.auxiliaryWeapon != 0 && type == EBtnType.SaleauxiliaryWeapon) // ��������
        {

            player.gold += DataManager.Instance.GetByKey<WeaponInfo>(player.inventory.auxiliaryWeapon).Price;

            player.inventory.RemoveWeaponItem(player.inventory.auxiliaryWeapon, index);

        }
        else if(idList[index] % 10000 < 600 && player.inventory.weapon[index] != 0 && type == EBtnType.Weapon) // ����
        {
            player.gold += DataManager.Instance.GetByKey<WeaponInfo>(player.inventory.weapon[index]).Price ;

            player.inventory.RemoveWeaponItem(player.inventory.weapon[index], index);
            
        }
        else if (idList[index] % 10000 < 1000 && player.inventory.consume[index] != 0 && type == EBtnType.Consume) // ������
        {
            // player.gold += DataManager.Instance.GetByKey<ConsumeItem>(player.inventory.weapon[index]).Price / 2;

            player.inventory.RemoveConsumeItem(player.inventory.consume[index], index);
        }
        Debug.Log("�Ǹ� �� :" + player.gold);

    }


}
