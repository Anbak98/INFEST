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
            Debug.Log("����");
            _inside.Add(networkObj);
            _store.EnterShopZone();
        }
        else if(_inside.Contains(networkObj) && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("��ȣ�ۿ�");
            _store.Interaction();
            Debug.Log(_store.isInteraction);
        }
        else if(_store.isInteraction && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("��ȣ�ۿ� ����");
            _inside.Remove(networkObj); 
            _store.EndShopZone();
        }

        if (_store.storeTimer.ExpiredOrNotRunning(_store.Runner))
        {
            Debug.Log("���� ����");
            _inside.Remove(networkObj);
            _store.EndShopZone();
        }
    }
}
