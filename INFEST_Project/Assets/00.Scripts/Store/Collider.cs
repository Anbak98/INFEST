using System.Collections.Generic;
using Fusion;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Collider : MonoBehaviour
{
    private readonly HashSet<NetworkObject> inside = new HashSet<NetworkObject>();

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        var networkObj = other.GetComponent<NetworkObject>();
        if (networkObj == null) return;
        

        if (!inside.Contains(networkObj))
        {
            inside.Add(networkObj);
            var store = GetComponentInParent<Store>();
            store.EnterShopZone();
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        Debug.Log("¡¢√À «ÿ¡¶");
        var networkObj = other.GetComponent<NetworkObject>();
        if (networkObj == null) return;

        if (inside.Contains(networkObj))
        {
            inside.Remove(networkObj);
            var store = GetComponentInParent<Store>();
            store.ExitShopZone();
        }
    }
}
