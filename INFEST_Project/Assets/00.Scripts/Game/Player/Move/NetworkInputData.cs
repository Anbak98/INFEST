using Fusion;
using UnityEngine;


public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkButtons buttons;

    // ���콺
    public const byte BUTTON_FIRE = 0;      // ���콺 ����
    public const byte BUTTON_ZOOM = 1;      // ���콺 ������

    // Ű����
    public const byte BUTTON_JUMP = 2;      // space
    public const byte BUTTON_RELOAD = 3;    // R
    public const byte BUTTON_INTERACT = 4;  // F
    public const byte BUTTON_USEITEM = 5;   // E
    public const byte BUTTON_RUN = 6;       // lShift
    public const byte BUTTON_SIT = 7;       // lCtrl
    public const byte BUTTON_SCOREBOARD = 8;    // Tab
}