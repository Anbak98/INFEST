using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnterRegionTrigger : MonoBehaviour
{
    public string regionName;
    public string toolTop;

    private List<PlayerRef> triggered = new();

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.TryGetComponent(out TargetableFromMonster tfm))
        {
            if (!triggered.Contains(tfm.Object.InputAuthority))
            {
                triggered.Add(tfm.Object.InputAuthority);
                RPC_ShowUIEnterRegion(tfm.Object.InputAuthority);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ShowUIEnterRegion([RpcTarget]PlayerRef player)
    {
        Global.Instance.UIManager.Show<UIEnterRegion>().SetText(regionName, toolTop);
    }
}
