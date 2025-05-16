using Fusion;
using UnityEngine;

public class PickUpCoinController : NetworkBehaviour
{
    public PickUpCoins[] pickUpCoins;
    int _index = 0;

    public override void Spawned()
    {
        _index = Random.Range(0, pickUpCoins.Length);

        pickUpCoins[_index].canPickUp = false;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
     public void RPC_CollisionChk()
    {
        pickUpCoins[_index].canPickUp = true;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_AddGold(Player player)
    {
        Debug.Log("¡¢√À");
        player.statHandler.CurGold += 1000;
    }
}
