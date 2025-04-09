using Fusion;
using UnityEngine;


public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkButtons buttons;

    // 마우스
    public const byte BUTTON_FIRE = 0;      // 마우스 왼쪽
    public const byte BUTTON_ZOOM = 1;      // 마우스 오른쪽

    // 키보드
    public const byte BUTTON_JUMP = 2;      // space
    public const byte BUTTON_RELOAD = 3;    // R
    public const byte BUTTON_INTERACT = 4;  // F
    public const byte BUTTON_USEITEM = 5;   // E
    public const byte BUTTON_RUN = 6;       // lShift
    public const byte BUTTON_SIT = 7;       // lCtrl
    public const byte BUTTON_SCOREBOARD = 8;    // Tab
}