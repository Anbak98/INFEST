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
        Debug.Log("����");

        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        var _player = other.GetComponent<NetworkObject>();
        if (_player == null) return;

        
        if (_store == null) return;

        if (!_inside.Contains(_player))
        {
            Debug.Log("����");
            _inside.Add(_player);
            _store.RPC_RequestEnterShopZone(_player, _player.Runner.LocalPlayer);
        }
        else if(_inside.Contains(_player) && _store.isInteraction)
        {
            Debug.Log("��ȣ�ۿ�");
            _store.RPC_RequestInteraction(_player, _player.Runner.LocalPlayer);
        }
        else if(_store.isInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("��ȣ�ۿ� ����");
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
        Debug.Log("���� ����");
        _inside.Remove(_player);
        _store.RPC_RequestLeaveShopZone(_player, _player.Runner.LocalPlayer);
    }
}
