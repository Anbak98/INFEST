using System.Collections;
using UnityEngine;
using INFEST.Game;

public class Siren : MonoBehaviour
{
    public bool isTrigger = false;

    private int _playerLayer = 7;
    private Player _player;
    [SerializeField] private SirenController _controller;

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (isTrigger) return;

        if(NetworkGameManager.Instance.gameState != GameState.Wave)
        {
            Player player = other.GetComponentInParent<Player>();
            if (other.gameObject.layer == _playerLayer && player)
            {
                _player = player;
                isTrigger = true;

                float delay = Random.Range(5f, 30f);
                StartCoroutine(DelayedSiren(delay, _player));
            }
        }
    }

    private IEnumerator DelayedSiren(float delay, Player player)
    {
        yield return new WaitForSeconds(delay);

        if (_controller != null && player != null)
        {
            NetworkGameManager.Instance.monsterSpawner.CallWave(player.transform);
            //AnalyticsManager.analyticsWave(2, NetworkGameManager.Instance.Runner.SimulationTime, , )
            _controller.RPC_PlaySirenSound(player, player.Object.InputAuthority);
        }
    }
}
