using Fusion;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [Networked] public NetworkString<_32> Nickname { get; set; }
    [Networked] public NetworkString<_16> Job { get; set; }

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            // ���� �÷��̾�� ���� �г��� ���� (��: PlayerPrefs)
            RPC_SetInfo(PlayerPrefs.GetString("Nickname"), PlayerPrefs.GetString("Job")); // ���� ����
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetInfo(string nickname, string job)
    {
        this.Nickname = nickname;
        this.Job = job;

        Debug.Log($"[PlayerInfo] Spawned: {Nickname} ({Job})");
        Matching.Instance.sessionController.UpdateRoom(this);
    }
}