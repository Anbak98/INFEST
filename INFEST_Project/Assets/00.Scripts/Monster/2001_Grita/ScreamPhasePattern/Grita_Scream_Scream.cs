using Fusion;
using INFEST.Game;
using UnityEngine;

// Wave�� �ƴϴ�
// Scream���¿��� �� ��
// �Ҹ� ������
public class Grita_Scream_Scream : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Scream>
{        
    public override void Enter()
    {
        base.Enter();
        
        // Scream
        monster.IsScream = true;
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
        NetworkGameManager.Instance.monsterSpawner.CallWave(monster.target, false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_PlayerScreamingSount()
    {
        AudioManager.instance.PlaySfx(Sfxs.GritaScream);
    }
}
