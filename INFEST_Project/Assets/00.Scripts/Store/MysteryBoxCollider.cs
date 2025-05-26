using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxCollider : MonoBehaviour
{
    private List<Player> _inAreaPlayer = new List<Player>();
    
    public MysteryBox mysteryBox;

    private int _playerLayer = 7;


    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (other.gameObject.layer == _playerLayer && player)
        {
            _inAreaPlayer.Add(player);

            if (mysteryBox == null) return;

            mysteryBox.RPC_EnterMysteryBoxZone(player, player.Object.InputAuthority);

            player.inMysteryBoxZoon = true;

        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (other.gameObject.layer == _playerLayer && player)
        {
            _inAreaPlayer.Remove(player);
            player.inMysteryBoxZoon = false;

            mysteryBox.RPC_LeaveMysteryBoxZone(player, player.Object.InputAuthority);
        }
    }
    private void OnDisable()
    {
        foreach (Player player in _inAreaPlayer)
        {
            player.inMysteryBoxZoon = false;
        }
        _inAreaPlayer.Clear();
    }

}
