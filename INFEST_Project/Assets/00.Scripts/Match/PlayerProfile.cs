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
public struct Profile : INetworkStruct
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
        SetInfo();

        Debug.Log($"[Profile] Player joined: {Runner.TryGetNetworkedBehaviourId(this)}");
        base.Spawned();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        Debug.Log($"[Room] Player left: {Runner.TryGetNetworkedBehaviourId(this)}");
        var Room = FindObjectOfType<Room>();
        if ( Room != null )
            Room.RPC_RemoveProfileToAll(this);
        base.Despawned(runner, hasState);
    }

    public void SetInfo()
    {
        var Room = FindObjectOfType<Room>();
        if (Room != null)
        {
            if (HasInputAuthority)
            {
                Room.MyProfile = this;

                Profile profile = new Profile()
                {
                    NickName = PlayerPrefs.GetString("Nickname"),
                    Job = (JOB)PlayerPrefs.GetInt("Job"),
                };

                RPC_SetInfo(profile);
            }
            Room.RPC_SendProfileToAll(this);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SetInfo(Profile profile)
    {
        Info = profile;
    }
}