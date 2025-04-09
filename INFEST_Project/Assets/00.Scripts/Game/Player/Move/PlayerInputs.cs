using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerInputs : INetworkStruct
{
    // All player specific inputs go here
    public Vector2 dir;
}

/// <summary>
/// 여러 플레이어를 위한 입력 수집:
/// </summary>
public struct CombinedPlayerInputs : INetworkInput
{
    // For this example we assume 4 players max on one peer
    public PlayerInputs PlayerA;
    public PlayerInputs PlayerB;
    public PlayerInputs PlayerC;
    public PlayerInputs PlayerD;

    // Example indexer for easier access to nested player structs
    public PlayerInputs this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return PlayerA;
                case 1: return PlayerB;
                case 2: return PlayerC;
                case 3: return PlayerD;
                default: return default;
            }
        }

        set
        {
            switch (i)
            {
                case 0: PlayerA = value; return;
                case 1: PlayerB = value; return;
                case 2: PlayerC = value; return;
                case 3: PlayerD = value; return;
                default: return;
            }
        }
    }
}