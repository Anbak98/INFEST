using System.Collections;
using Fusion;
using UnityEngine;

public class SirenController : NetworkBehaviour
{
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_PlaySirenSound(Player _player, PlayerRef _playerRef)
    {
        AudioManager.instance.PlaySfx(Sfxs.Siren);
        StartCoroutine(StopSirenSoundAfterSeconds(9.3f));

        var allMonsters = FindObjectsOfType<MonsterNetworkBehaviour>();
        foreach (var monster in allMonsters)
        {
            monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;
        }

        StartCoroutine(MonsterDetectTime(30f, allMonsters));
        //StartCoroutine(ResetTriggerAfterDelay(300f));
    }

    private IEnumerator StopSirenSoundAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        AudioManager.instance.StopSfx();        
    }

    private IEnumerator MonsterDetectTime(float delay, MonsterNetworkBehaviour[] allMonsters)
    {
        yield return new WaitForSeconds(delay);

        foreach (var monster in allMonsters)
        {
            monster.PlayerDetectorCollider.radius = monster.info.DetectAreaNormal;
        }
    }

    //private IEnumerator ResetTriggerAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    Siren.isTrigger = false;
    //}
}
