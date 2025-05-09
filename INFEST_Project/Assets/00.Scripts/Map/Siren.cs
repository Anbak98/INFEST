using System.Collections;
using Fusion;
using UnityEngine;

public class Siren : NetworkBehaviour
{
    [SerializeField] private AudioSource sirenSound;
    [SerializeField] private AudioClip sirenSoundClip;

    [Networked] private bool IsTriggered { get; set; }

    private int _playerLayer = 7;

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player == null || other.gameObject.layer != _playerLayer) return;
        if (!player.Object.HasInputAuthority) return;

        if(HasInputAuthority)
        RPC_RequestTrigger();

        if (HasStateAuthority)
        {
            if (IsTriggered) return;
            IsTriggered = true;
            RPC_PlaySirenSound();
            StartCoroutine(ResetTriggerAfterDelay(420f));
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestTrigger()
    {
        if (IsTriggered) return;
        IsTriggered = true;
        RPC_PlaySirenSound();
        StartCoroutine(ResetTriggerAfterDelay(420f));
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlaySirenSound()
    {
        if (sirenSound != null && sirenSoundClip != null)
        {
            sirenSound.PlayOneShot(sirenSoundClip);
        }
    }

    private IEnumerator ResetTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsTriggered = false;
    }
}
