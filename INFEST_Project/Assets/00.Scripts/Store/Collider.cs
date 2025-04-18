using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Collider : MonoBehaviour // 유저가 상점에 진입했고 상호작용하는지 체크해준다.
{
    private readonly HashSet<NetworkObject> _inside = new ();
    public Store _store;
    public StoreController _storeController;

    private void Awake()
    {
        _store = GetComponent<Store>();
    }

    private void OnTriggerStay(UnityEngine.Collider other) // => 나눠야함
    {
        
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        var _player = other.GetComponent<NetworkObject>();
        if (_player == null) return;

        
        if (_store == null) return;

        if (!_inside.Contains(_player))
        {
            Debug.Log("접촉");
            _inside.Add(_player);
            _store.RPC_RequestEnterShopZone(_player, _player.InputAuthority);
        }
        else if(_inside.Contains(_player) && _store.isInteraction)
        {
            Debug.Log("상호작용");
            _store.RPC_RequestInteraction(_player, _player.InputAuthority);
        }
        else if(_store.isInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("상호작용 해제");
            _inside.Remove(_player); 
            _store.RPC_RequestLeaveShopZone(_player, _player.InputAuthority);
        }

        if (_storeController.storeTimer.ExpiredOrNotRunning(_storeController.Runner))
        {
            Exit(_player, _store);
        }

        StopAllCoroutines();
        StartCoroutine(ICorutine(_player, _store));
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        Debug.Log("나감");
    }

    IEnumerator ICorutine(NetworkObject _player, Store _store)
    {
        yield return new WaitForSeconds(0.5f);
        Exit(_player, _store);
    }

    public void Exit(NetworkObject _player, Store _store)
    {
        Debug.Log("접촉 해제");
        _inside.Remove(_player);
        _store.RPC_RequestLeaveShopZone(_player, _player.InputAuthority);
    }
}
