using Fusion;
using UnityEngine;

public class Store : NetworkBehaviour
{
    public UIStore uIStore;
    // Ÿ�̸�
    [Networked] private NetworkBool _activeTime { get; set; }
    [Networked] public TickTimer storeTimer { get; private set; }

    private float _newStoreTime = 1000f;
    private float _activateTime = 500f;
    public bool isInteraction = false;
    public bool inZone= false;

    public override void Spawned()
    {
        Activate();
    }

    public override void FixedUpdateNetwork()
    {
        //Debug.Log(_storeTimer.RemainingTime(Runner));
        if (storeTimer.ExpiredOrNotRunning(Runner))
        {
            _activeTime = true;
            EndShopZone();
        }
    }

    /// <summary>
    /// ���� ������ Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void Activate()
    {
        storeTimer = TickTimer.CreateFromSeconds(Runner, _newStoreTime);
    }

    /// <summary>
    /// ���� ��ȣ�ۿ�� Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void Interaction()
    {
        isInteraction = true;
        uIStore.panel.gameObject.SetActive(true);
        uIStore.interactionText.gameObject.SetActive(false);

        if (_activeTime)
        {
            storeTimer = TickTimer.None;
            storeTimer = TickTimer.CreateFromSeconds(Runner, _activateTime);
            _activeTime = false;
        }
    }

    public void Exit()
    {
        isInteraction = false;
    }

    public void EnterShopZone()
    {
        uIStore.panel.gameObject.SetActive(false);
        uIStore.interactionText.gameObject.SetActive(true);
    }

    public void EndShopZone()
    {
        isInteraction = false;
        uIStore.interactionText.gameObject.SetActive(false);
        uIStore.panel.gameObject.SetActive(false);
    }
}
