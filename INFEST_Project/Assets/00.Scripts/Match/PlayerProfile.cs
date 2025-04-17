using Fusion;
using System;
using UnityEngine;

public enum JOB
{
    Medic,
    Defender,
    SWAT,
    Demolator
}
[Serializable]
public struct Profile: INetworkStruct
{
    public NetworkString<_16> NickName;
    public JOB Job;
}

public class PlayerProfile : NetworkBehaviour
{
    [Networked, SerializeField]
    public Profile Info { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        if (HasInputAuthority)
        {
            Profile profile = new Profile()
            {
                NickName = PlayerPrefs.GetString("Nickname"),
                Job = JOB.Medic
            };

            RPC_SetInfo(profile);
        }

        var Room = FindObjectOfType<Room>();
        Debug.Log($"[Profile] Player joined: {Runner.TryGetNetworkedBehaviourId(this)}");
        Room.RPC_SendProfileToAll(this);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (HasInputAuthority)
        {

            foreach (var profile in MatchManager.Instance.uiProfils)
            {
                profile.NickName.text = "";
            }
        }


        Debug.Log($"[Room] Player left: {Runner.TryGetNetworkedBehaviourId(this)}");
        var Room = FindObjectOfType<Room>();
        //Room?.RPC_RemoveProfileToAll(this);
        base.Despawned(runner, hasState);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SetInfo(Profile profile)
    {
        Info = profile;
    }
}