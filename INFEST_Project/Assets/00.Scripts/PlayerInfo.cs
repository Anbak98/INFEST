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
            // 로컬 플레이어는 직접 닉네임 설정 (예: PlayerPrefs)
            RPC_SetInfo(PlayerPrefs.GetString("Nickname"), PlayerPrefs.GetString("Job")); // 예시 직업
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