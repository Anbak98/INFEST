using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Store : NetworkBehaviour // 상점의 로직(무기 지급, UI띄어주기 등) 수행해준다.
{
    public StoreController _storeController;
    public Action<string> changeUI;

    public List<int> idList;
    public UIStore uIStore;

    // 플레이어 골드
    // 플레이어 방어구
    // 팀 골드
    // 아이템 데이터들

    // 상호작용 체크용 불값
    [Networked] public NetworkBool isInteraction { get; set; } = false; // 플레이어마다 있어야 할것같다. 리팩토링 예정

    #region  상점 콜라이더 트리거 메소드
    /// <summary>
    /// 상호작용시 요청하는 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestInteraction(NetworkObject _player, PlayerRef _playerRef)
    {
        RPC_Interaction(_player, _playerRef);
    }

    /// <summary>
    /// 상호작용 로직
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
    /// 상점의 영역에 들어갔을때 요청하는 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestEnterShopZone(NetworkObject _player, PlayerRef _playerRef)
    {
        RPC_EnterShopZone(_player, _playerRef);

    }

    /// <summary>
    /// 상점 영역 로직
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
    /// 상점을 떠났을때 요청하는 메소드
    /// </summary>
    /// <param name="_player"></param>
    /// <param name="_playerRef"></param>
    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestLeaveShopZone(NetworkObject _player, PlayerRef _playerRef)
    {
        RPC_LeaveShopZone(_player, _playerRef);
    }

    /// <summary>
    /// 상점을 떠났을때 + 상호작용 종료 로직
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

        Debug.Log("구매 전 :" + player.gold);

        if (idList[index] % 10000 < 600) // 무기
        {
            WeaponInstance _weaponInstance = new(idList[index]);

            player.gold -= _weaponInstance.data.Price;

            player.inventory.AddWeponItme(_weaponInstance);
        }
        else if (idList[index] % 10000 < 1000) // 아이템
        {
            ConsumeInstance _consumeInstance = new(idList[index]);
            player.inventory.AddConsumeItme(_consumeInstance);
        }

        Debug.Log("구매 후 :" + player.gold);

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

        Debug.Log("판매 전 :" + player.gold);
        if (player == null) return;

        if (idList[index] % 10000 < 200 && player.inventory.auxiliaryWeapon != 0 && type == EBtnType.SaleauxiliaryWeapon) // 보조무기
        {

            player.gold += DataManager.Instance.GetByKey<WeaponInfo>(player.inventory.auxiliaryWeapon).Price;

            player.inventory.RemoveWeaponItem(player.inventory.auxiliaryWeapon, index);

        }
        else if(idList[index] % 10000 < 600 && player.inventory.weapon[index] != 0 && type == EBtnType.Weapon) // 무기
        {
            player.gold += DataManager.Instance.GetByKey<WeaponInfo>(player.inventory.weapon[index]).Price ;

            player.inventory.RemoveWeaponItem(player.inventory.weapon[index], index);
            
        }
        else if (idList[index] % 10000 < 1000 && player.inventory.consume[index] != 0 && type == EBtnType.Consume) // 아이템
        {
            // player.gold += DataManager.Instance.GetByKey<ConsumeItem>(player.inventory.weapon[index]).Price / 2;

            player.inventory.RemoveConsumeItem(player.inventory.consume[index], index);
        }
        Debug.Log("판매 후 :" + player.gold);

    }


}
