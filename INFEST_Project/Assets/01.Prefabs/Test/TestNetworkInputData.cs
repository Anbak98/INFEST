using Fusion;
using UnityEngine;

public struct TestNetworkInputData : INetworkInput
{
    public const byte MOUSEBUTTON0 = 1;
    public const byte MOUSEBUTTON1 = 2;
    public const byte MOUSEBUTTON2 = 3;

    public bool reloadPressed;
    public bool isFiringHeld;
    public NetworkButtons buttons;
    public Vector3 direction;
}