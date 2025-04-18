using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class StoreController : NetworkBehaviour
{
    [Networked] public NetworkBool activeTime { get; set; }
    [Networked] public TickTimer storeTimer { get; set; }

    public float newStoreTime = 120f;
    public float activateTime = 60f;

    public override void Spawned()
    {
        Activate();
        activeTime = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (storeTimer.ExpiredOrNotRunning(Runner)) // 타이머 메소드 이동예정
        {
            activeTime = true;
            RPC_EndTImer();
        }
    }

    /// <summary>
    /// 상점 생성시 타이머 활성화
    /// </summary>
    public void Activate()  // 타이머 메소드 이동예정
    {
        if (HasStateAuthority)
        {
            storeTimer = TickTimer.CreateFromSeconds(Runner, newStoreTime);
            Debug.Log(storeTimer.RemainingTime(Runner));
        }
    }

    /// <summary>
    /// 상점 시간이 지나면 꺼지는 메소드.
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EndTImer()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UIStore uIStore = GetComponentInChildren<Store>().uIStore;
        if (uIStore == null) return;
        uIStore.interactionText.gameObject.SetActive(false);
        uIStore.panel.gameObject.SetActive(false);
    }


}
