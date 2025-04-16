using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Collider : MonoBehaviour
{
    private readonly HashSet<NetworkObject> _inside = new ();

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        var _player = other.GetComponent<NetworkObject>();
        if (_player == null) return;

        var _store = GetComponentInParent<Store>();
        _store.inZone = true;

        if (!_inside.Contains(_player))
        {
            Debug.Log("접촉");
            _inside.Add(_player);
            _store.EnterShopZone();
        }
        else if(_inside.Contains(_player) && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("상호작용");
            _store.Interaction();
        }
        else if(_store.isInteraction && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("상호작용 해제");
            _inside.Remove(_player); 
            _store.EndShopZone();
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
        yield return new WaitForSeconds(3f);
        Exit(_player, _store);
    }

    public void Exit(NetworkObject _player, Store _store)
    {
        Debug.Log("접촉 해제");
        _inside.Remove(_player);
        _store.EndShopZone();
    }
}
