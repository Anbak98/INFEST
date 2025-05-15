using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class StoreController : NetworkBehaviour
{
    public UIShopView uIShopView;

    [Networked] public NetworkBool activeTime { get; set; }
    [Networked] public TickTimer storeTimer { get; set; }
    [Networked] private int _randomIndex { get; set; }

    private int[] _activeIndex = new int[3] { 0, 0, 0 };

    public List<Store> aiiStores;

    public float newStoreTime = 10f;
    public float activateTime = 5f;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            activeTime = true;
            Activate();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            if (storeTimer.ExpiredOrNotRunning(Runner))
            {
                activeTime = true;
                if (aiiStores.Count < 4)
                {
                    RPC_Hide(_randomIndex);
                    LightHide(_randomIndex);
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        _randomIndex = _activeIndex[i];
                        RPC_Hide(_randomIndex);
                        LightHide(_randomIndex);
                        _activeIndex[i] = -1;
                    }
                }

                RPC_EndTImer();
                Activate();
            }

            //if (!storeTimer.ExpiredOrNotRunning(Runner))
            //{
            //    Activate();
            //}

        }
    }

    /// <summary>
    /// ���� ������ Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void Activate()
    {
        if (HasStateAuthority)
        {
            if (aiiStores.Count < 4)
            {
                _randomIndex = UnityEngine.Random.Range(0, aiiStores.Count);
                RPC_Show(_randomIndex);
                LightShow(_randomIndex);

            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    _randomIndex = UnityEngine.Random.Range(0, aiiStores.Count);
                    while (i != 0 && (_activeIndex[0] == _randomIndex || _activeIndex[1] == _randomIndex))
                    {
                        _randomIndex = UnityEngine.Random.Range(0, aiiStores.Count);
                    }
                    _activeIndex[i] = _randomIndex;
                    RPC_Show(_randomIndex);
                    LightShow(_randomIndex);
                }
            }


            storeTimer = TickTimer.CreateFromSeconds(Runner, newStoreTime);
        }

    }

    ///// <summary>
    ///// ���� ��ȣ�ۿ�� Ÿ�̸� �ð� ����
    ///// </summary>
    //[Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    //public void RPC_Timer(PlayerRef _playerRef)
    //{
    //    if (HasStateAuthority)
    //    {
    //        float _remainingTime = storeTimer.RemainingTime(Runner) ?? 0f;
    //        storeTimer = TickTimer.CreateFromSeconds(Runner, activateTime + _remainingTime);
    //        activeTime = false;
    //        Debug.Log("��ȣ�ۿ� �� \n ���� ���� �ð� : " + storeTimer.RemainingTime(Runner));
    //    }
    //}

    /// <summary>
    /// ���� ��ȣ�ۿ�� Ÿ�̸� �ð� ����
    /// </summary>
    public void AddTimer()
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
        if (!Player.local.inStoreZoon) return;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Player.local.inStoreZoon = false;
        //uIShopView.interactionText.gameObject.SetActive(false);
        //uIShopView.bg.gameObject.SetActive(false);
        Global.Instance.UIManager.Hide<UIInteractiveView>();
        Global.Instance.UIManager.Hide<UIShopView>();
    }


    /// <summary>
    /// ���� ��Ȱ��ȭ
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Hide(int index)
    {
        aiiStores[index].activatelighting.SetActive(false);
    }

    /// <summary>
    /// ���� Ȱ��ȭ
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Show(int index)
    {
        aiiStores[index].activatelighting.SetActive(true);
    }

    private void LightHide(int index)
    {
        aiiStores[index].col.enabled = false;
        Debug.Log(index + "��° ������ ��Ȱ��ȭ ��");

    }
    private void LightShow(int index)
    {
        aiiStores[index].col.enabled = true;
        Debug.Log(index + "��° ������ Ȱ��ȭ ��");

    }

}
