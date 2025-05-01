using UnityEngine;

public class PickUpCoins : MonoBehaviour
{
    [SerializeField] private PickUpCoinController _pckUpCoinController;
    private int _playerLayer = 7;
    public bool canPickUp = true;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (other.gameObject.layer == _playerLayer && player)
        {
            if (canPickUp) return;

            _pckUpCoinController.RPC_AddGold(player);
            canPickUp = true;

            _pckUpCoinController.RPC_CollisionChk();
        }
    }

}
