using Fusion;
using UnityEngine;

public enum JOB
{
    Medic,
    Defender,
    SWAT,
    Demolator
}

public struct Profile: INetworkStruct
{
    public NetworkString<_16> NickName;
    public JOB Job;
}

public class PlayerProfile : NetworkBehaviour
{
    [Networked]
    public Profile Info { get; set; }

    public override void Spawned()
    {
        base.Spawned();

        if(Object.HasStateAuthority)
        {
            Profile profile = Info;
            profile.NickName = PlayerPrefs.GetString("Nickname");
            profile.Job = JOB.Medic;
            Info = profile;
        }

        Debug.Log($"[Room] Player joined: {Runner.TryGetNetworkedBehaviourId(this)}");
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);

        Debug.Log($"[Room] Player left: {Runner.TryGetNetworkedBehaviourId(this)}");
    }
}