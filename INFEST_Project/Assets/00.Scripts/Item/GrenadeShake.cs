using UnityEngine;

public class GrenadeShake : MonoBehaviour
{
    private int _playerLayer = 7;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (other.gameObject.layer == _playerLayer && player)
        {
            player.GetComponentInChildren<Recoil>().ApplyCamRecoil();
        }
    }
}
