using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class StoreController : NetworkBehaviour
{
    public UIShopView uIShopView;

    [Networked] private NetworkBool _runingTime { get; set; }
    [Networked] public NetworkBool activeTime { get; set; }
    [Networked] public TickTimer storeTimer { get; set; }
    [Networked] private int _randomIndex { get; set; }

    public List<Store> aiiStores;

    public float newStoreTime = 10f;
    public float activateTime = 5f;

    public override void Spawned()
    {
        activeTime = true;
        Activate();
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            if (storeTimer.ExpiredOrNotRunning(Runner))
            {
                activeTime = true;
                RPC_Hide(_randomIndex);
                RPC_EndTImer();
            }

            if (!_runingTime)
            {
                Activate();
            }

        }
    }

    /// <summary>
    /// ���� ������ Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void Activate()
    {
        if (HasStateAuthority)
        {
            _randomIndex = Random.Range(0, aiiStores.Count);
            RPC_Show(_randomIndex);
            storeTimer = TickTimer.CreateFromSeconds(Runner, newStoreTime);
            _runingTime = true;
        }
        uIShopView.StoreInIt(aiiStores[_randomIndex]);

    }

    /// <summary>
    /// ���� ��ȣ�ۿ�� Ÿ�̸� �ð� ����
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Timer(PlayerRef _playerRef)
    {
        if (HasStateAuthority)
        {
            float _remainingTime = storeTimer.RemainingTime(Runner) ?? 0f;
            storeTimer = TickTimer.CreateFromSeconds(Runner, activateTime + _remainingTime);
            activeTime = false;
            Debug.Log("��ȣ�ۿ� �� \n ���� ���� �ð� : " + storeTimer.RemainingTime(Runner));
        }
    }

    /// <summary>
    /// ���� �ð��� ������ ������ �޼ҵ�.
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EndTImer()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Player.local.inStoreZoon = false;
        uIShopView.interactionText.gameObject.SetActive(false);
        uIShopView.bg.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���� ��Ȱ��ȭ
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Hide(int index)
    {
        aiiStores[index].gameObject.SetActive(false);
        _runingTime = false;
    }

    /// <summary>
    /// ���� Ȱ��ȭ
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Show(int index)
    {
        aiiStores[index].gameObject.SetActive(true);
    }
}
