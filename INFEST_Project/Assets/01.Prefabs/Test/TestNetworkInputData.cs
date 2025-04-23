using Fusion;
using UnityEngine;

public struct TestNetworkInputData : INetworkInput
{
    public const byte MOUSEBUTTON0 = 1;
    public const byte MOUSEBUTTON1 = 2;
    public const byte MOUSEBUTTON2 = 3;

    public float scrollWheelValue;
    public bool scrollWheel;
    public bool isFiringHeld;
    public bool reloadPressed;

    public NetworkButtons buttons;
    public Vector3 direction;
}