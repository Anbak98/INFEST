using Fusion;
using INFEST.Game;
using UnityEngine;

// Wave가 아니다
// Scream상태에서 할 일
// 소리 지르기
public class Grita_Scream_Scream : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Scream>
{

    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip _screamingSound;
    public override void Enter()
    {
        base.Enter();

        // Scream
        monster.IsScream = true;
        monster.IsCooltimeCharged = false;  // 기술 썼으니
        monster.ScreamCount++;
        monster.Rpc_Scream();
    }    

    public override void Exit()
    {
        base.Exit();
        monster.IsScream = false;
    }

    public override void Effect()
    {
        base.Effect();
        RPC_PlayerScreamingSount();
        NetworkGameManager.Instance.monsterSpawner.CallWave(monster.target);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_PlayerScreamingSount()
    {
        _audio.PlayOneShot(_screamingSound);
    }
}
