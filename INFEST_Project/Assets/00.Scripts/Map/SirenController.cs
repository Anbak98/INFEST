using System.Collections;
using Fusion;
using UnityEngine;

public class SirenController : NetworkBehaviour
{
    [SerializeField] private Siren siren;

    [SerializeField] private AudioSource sirenSound;
    [SerializeField] private AudioClip sirenSoundClip;        

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

        siren.isTrigger = true;
        StartCoroutine(ResetTriggerAfterDelay(420f));
    }

    private IEnumerator ResetTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        siren.isTrigger = false;
    }
}
