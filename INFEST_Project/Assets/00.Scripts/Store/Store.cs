using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour
{
    public UIStore uIStore;
    // Ÿ�̸�
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
    /// ���� ������ Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void ActivateTimer()
    {
        _storeTimer = TickTimer.CreateFromSeconds(Runner, _newStoreTime);
    }

    /// <summary>
    /// ���� ��ȣ�ۿ�� Ÿ�̸� Ȱ��ȭ
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
