using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Collider : MonoBehaviour
{
    private readonly HashSet<NetworkObject> _inside = new ();
    private Store _store;

    private void Awake()
    {
        _store = GetComponent<Store>();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Debug.Log("접촉");

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        var _player = other.GetComponent<NetworkObject>();
        if (_player == null) return;

        
        if (_store == null) return;

        if (!_inside.Contains(_player))
        {
            Debug.Log("접촉");
            _inside.Add(_player);
            _store.RPC_RequestEnterShopZone(_player, _player.Runner.LocalPlayer);
        }
        else if(_inside.Contains(_player) && _store.isInteraction)
        {
            Debug.Log("상호작용");
            _store.RPC_RequestInteraction(_player, _player.Runner.LocalPlayer);
        }
        else if(_store.isInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("상호작용 해제");
            _inside.Remove(_player); 
            _store.RPC_RequestLeaveShopZone(_player, _player.Runner.LocalPlayer);
        }

        if (_store.storeTimer.ExpiredOrNotRunning(_store.Runner))
        {
            Exit(_player, _store);
        }

        StopAllCoroutines();
        StartCoroutine(ICorutine(_player, _store));
    }

    IEnumerator ICorutine(NetworkObject _player, Store _store)
    {
        yield return new WaitForSeconds(2f);
        Exit(_player, _store);
    }

    public void Exit(NetworkObject _player, Store _store)
    {
        Debug.Log("접촉 해제");
        _inside.Remove(_player);
        _store.RPC_RequestLeaveShopZone(_player, _player.Runner.LocalPlayer);
    }
}
