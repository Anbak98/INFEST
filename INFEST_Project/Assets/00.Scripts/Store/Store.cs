using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour
{
    public UIStore uIStore;
    // Ÿ�̸�
    [Networked] private NetworkBool _activeTime { get; set; }
    [Networked] public TickTimer storeTimer { get; private set; }
    [Networked] public NetworkBool isInteraction { get; set; } = false;


    private float _newStoreTime = 120f;
    private float _activateTime = 60f;

    public override void Spawned()
    {
        Activate();
        _activeTime = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (storeTimer.ExpiredOrNotRunning(Runner))
        {
            _activeTime = true;
            RPC_EndTImer();
        }
    }

    /// <summary>
    /// ���� ������ Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void Activate()
    {
        if (HasStateAuthority)
        {
            storeTimer = TickTimer.CreateFromSeconds(Runner, _newStoreTime);
            Debug.Log(storeTimer.RemainingTime(Runner));
        }
    }

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

        uIStore = _player.GetComponentInChildren<UIStore>();
        if (uIStore == null) return;

        isInteraction = false;
        uIStore.panel.gameObject.SetActive(true);
        uIStore.interactionText.gameObject.SetActive(false);
        if (_activeTime)
        {
            storeTimer = TickTimer.CreateFromSeconds(Runner, _activateTime);
            _activeTime = false;
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
        Debug.Log(_playerRef);

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
        Debug.Log(uIStore);
        if (uIStore == null) return;

        uIStore.panel.gameObject.SetActive(false);
        uIStore.interactionText.gameObject.SetActive(true);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestLeaveShopZone(NetworkObject _player, PlayerRef _playerRef)
    {
        RPC_LeaveShopZone(_player, _playerRef);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_LeaveShopZone(NetworkObject _player, [RpcTarget] PlayerRef _playerRef)
    {
        //if (Runner.LocalPlayer != _playerRef) return;

        uIStore = _player.GetComponentInChildren<UIStore>();
        if (uIStore == null) return;

        uIStore.interactionText.gameObject.SetActive(false);
        uIStore.panel.gameObject.SetActive(false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EndTImer()
    {
        uIStore.interactionText.gameObject.SetActive(false);
        uIStore.panel.gameObject.SetActive(false);
    }
}
