using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour
{
    public UIStore uIStore;
    // 타이머
    [Networked] private NetworkBool _activeTime { get; set; }
    [Networked] private TickTimer _storeTimer { get; set; }
    private float _newStoreTime = 10f;
    private float _activateTime = 5f;

    public override void Spawned()
    {
        ActivateTimer();
    }


    public override void FixedUpdateNetwork()
    {
        Debug.Log(_storeTimer.RemainingTime(Runner));
        if (_storeTimer.ExpiredOrNotRunning(Runner))
        {
            _activeTime = true;
            ExitShopZone();
        }
    }

    /// <summary>
    /// 상점 생성시 타이머 활성화
    /// </summary>
    public void ActivateTimer()
    {
        _storeTimer = TickTimer.CreateFromSeconds(Runner, _newStoreTime);
    }

    /// <summary>
    /// 상점 상호작용시 타이머 활성화
    /// </summary>
    public void NewStoreTimer()
    {
        if (_activeTime)
        {
            _storeTimer = TickTimer.None;
            _storeTimer = TickTimer.CreateFromSeconds(Runner, _activateTime);
            _activeTime = false;
        }
    }

    public void EnterShopZone()
    {
        uIStore.button.gameObject.SetActive(true);
    }

    public void ExitShopZone()
    {
        uIStore.button.gameObject.SetActive(false);
    }
}
