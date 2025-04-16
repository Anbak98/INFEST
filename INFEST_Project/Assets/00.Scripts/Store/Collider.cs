using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class Collider : MonoBehaviour
{
    private readonly HashSet<NetworkObject> _inside = new ();
    private Store _store;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        var networkObj = other.GetComponent<NetworkObject>();
        if (networkObj == null) return;

        _store = GetComponentInParent<Store>();
        _store.inZone = true;

        if (!_inside.Contains(networkObj))
        {
            Debug.Log("접촉");
            _inside.Add(networkObj);
            _store.EnterShopZone();
        }
        else if(_inside.Contains(networkObj) && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("상호작용");
            _store.Interaction();
            Debug.Log(_store.isInteraction);
        }
        else if(_store.isInteraction && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("상호작용 해제");
            _inside.Remove(networkObj); 
            _store.EndShopZone();
        }

        if (_store.storeTimer.ExpiredOrNotRunning(_store.Runner))
        {
            Debug.Log("접촉 해제");
            _inside.Remove(networkObj);
            _store.EndShopZone();
        }
    }
}
