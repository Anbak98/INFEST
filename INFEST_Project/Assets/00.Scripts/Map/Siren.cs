using UnityEngine;

public class Siren : MonoBehaviour
{
    public bool isTrigger = false;

    private int _playerLayer = 7;
    private Player _player;
    [SerializeField] private SirenController _controller;

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (isTrigger) return;

        Player player = other.GetComponentInParent<Player>();
        if (other.gameObject.layer == _playerLayer && player)
        {
            _player = player;

            _controller.RPC_PlaySirenSound(_player, _player.Object.InputAuthority);
        }
    }    
}
