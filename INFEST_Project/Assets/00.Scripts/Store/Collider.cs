    using System.Collections.Generic;
    using UnityEngine;

    public class Collider : MonoBehaviour
    {
        private List<Player> _inAreaPlayer = new List<Player>();
        public Store _store;

        private int _playerLayer = 7;


        private void OnTriggerEnter(UnityEngine.Collider other) 
        {
            Player player = other.GetComponentInParent<Player>();
            if (other.gameObject.layer == _playerLayer && player)
            { 
                _inAreaPlayer.Add(player);

                if (_store == null) return;
                player.store = _store;

                _store.RPC_EnterShopZone(player, player.Object.InputAuthority);
                player.inStoreZoon = true;

            }
        }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (other.gameObject.layer == _playerLayer && player)
        {
            _inAreaPlayer.Remove(player);
            player.inStoreZoon = false;

            if (_store == null) return;

            _store.RPC_LeaveShopZone(player, player.Object.InputAuthority);
            player.store = null;

        }
    }
    private void OnDisable()
    {
        foreach (Player player in _inAreaPlayer)
        {
            player.store = null;
            player.inStoreZoon = false;
            _store.RPC_LeaveShopZone(player, player.Object.InputAuthority);
        }
        _inAreaPlayer.Clear();
    }

}
