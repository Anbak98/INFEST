using System.Collections;
using System.Threading;
using Fusion;
using UnityEngine;

public class SirenController : NetworkBehaviour
{
    [SerializeField] private Siren siren;

    [SerializeField] private AudioSource sirenSound;
    [SerializeField] private AudioClip sirenSoundClip;

    private MonsterNetworkBehaviour _monster;

    public override void Spawned()
    {
        _monster = FindObjectOfType<Monster_PJ_HI>();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_RequestTrigger(Player _player, PlayerRef _playerRef)
    {
        RPC_PlaySirenSound(_player, _playerRef);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    private void RPC_PlaySirenSound(Player _player, PlayerRef _playerRef)
    {
        if (sirenSound != null && sirenSoundClip != null)
        {
            sirenSound.PlayOneShot(sirenSoundClip);
        }

        _monster.PlayerDetectorCollider.radius = _monster.info.DetectAreaWave;
        siren.isTrigger = true;
        StartCoroutine(MonsterDetectTime(30f));
        StartCoroutine(ResetTriggerAfterDelay(420f));
    }

    private IEnumerator MonsterDetectTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        _monster.PlayerDetectorCollider.radius = _monster.info.DetectAreaNormal;
    }

    private IEnumerator ResetTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        siren.isTrigger = false;
    }
}
