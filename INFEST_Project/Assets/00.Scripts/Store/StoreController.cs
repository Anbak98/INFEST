using System;
using System.Collections.Generic;
using Fusion;
using INFEST.Game;
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
        }
    }

    /// <summary>
    /// 상점 생성시 타이머 활성화
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
    /// <summary>
    /// 상점 상호작용시 타이머 시간 연장
    /// </summary>
    public void AddTimer()
    {
        if (HasStateAuthority)
        {
            float _remainingTime = storeTimer.RemainingTime(Runner) ?? 0f;
            storeTimer = TickTimer.CreateFromSeconds(Runner, activateTime + _remainingTime);
            activeTime = false;
            Debug.Log("상호작용 후 \n 현재 남은 시간 : " + storeTimer.RemainingTime(Runner));
        }
    }

    /// <summary>
    /// 상점 시간이 지나면 꺼지는 메소드.
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_EndTImer()
    {
        if (!NetworkGameManager.Instance.gamePlayers.GetPlayerObj(NetworkGameManager.Instance.Runner.LocalPlayer).inStoreZoon) return; // 각각의 플레이어 정보를 넘겨주지않으면 불가능.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        NetworkGameManager.Instance.gamePlayers.GetPlayerObj(NetworkGameManager.Instance.Runner.LocalPlayer).inStoreZoon = false;
        //uIShopView.interactionText.gameObject.SetActive(false);
        //uIShopView.bg.gameObject.SetActive(false);
        Global.Instance.UIManager.Hide<UIInteractiveView>();
        Global.Instance.UIManager.Hide<UIShopView>();
    }


    /// <summary>
    /// 상점 비활성화
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Hide(int index)
    {
        aiiStores[index].activatelighting.SetActive(false);
    }

    /// <summary>
    /// 상점 활성화
    /// </summary>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_Show(int index)
    {
        aiiStores[index].activatelighting.SetActive(true);
    }

    private void LightHide(int index)
    {
        aiiStores[index].col.enabled = false;
        Debug.Log(index + "번째 상점이 비활성화 됨");

    }
    private void LightShow(int index)
    {
        aiiStores[index].col.enabled = true;
        Debug.Log(index + "번째 상점이 활성화 됨");

    }

    //[Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    //private void RPC_SetStoreKeyList()
    //{
    //    int i;
    //    int j;
    //    int[] toRegisterNum = new int[aiiStores[0].idList.Count];                       // 상점에 등록될수있는 수
    //    bool[] randomActive = new bool[aiiStores.Count];                                // 
    //    bool[,] maxRegistered = new bool[aiiStores.Count, aiiStores[0].idList.Count];   // 최대 등록갯수 초과하는지
    //    int randomNum;

    //    for (i = 0; i < aiiStores[0].idList.Count; i++)
    //    {
    //        toRegisterNum[i] = 7;
    //    }





    //    for (i = 0; i < aiiStores[0].idList.Count; i++) 
    //    {
    //        for (j = 0; j < toRegisterNum.Length - toRegisterNum[i]; j++)
    //        {

    //            randomNum = UnityEngine.Random.RandomRange(0, aiiStores.Count);
    //            while (randomNum && !maxRegistered[0])
    //                randomActive[randomNum] = false;
    //        }


    //        for(j = 0; j < aiiStores.Count; j++)
    //        {

    //            if (!randomActive[j])
    //            {
    //                aiiStores[i].idList[i] = 0;
    //            }

    //            toRegisterNum[i]--;
    //        }

    //    }


    //}

    //private bool AuxiliaryWeaponChk()
    //{
    //    return true;
    //}
}
