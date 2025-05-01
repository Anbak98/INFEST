using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    [SerializeField] private MVPStageSpawner _MVPStageSpawner;
    private int _playerLayer = 7;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (other.gameObject.layer == _playerLayer && player)
        {
            if (_MVPStageSpawner.monsterSpawner == null) return;

            _MVPStageSpawner.monsterSpawner.RPC_WaveStart();
            enabled = false;

        }
    }
}
