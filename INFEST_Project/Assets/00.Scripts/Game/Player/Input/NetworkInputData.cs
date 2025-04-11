using Fusion;
using UnityEngine;

/// <summary>
/// 플레이어의 입력 데이터를 네트워크로 동기화하기 위해 사용하는 구조체
/// 플레이어의 입력 상태를 네트워크를 통해 전달
/// </summary>
public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;           // 이동방향(wasd)
    public Vector2 lookDelta;             // 마우스 움직임(시점 회전)
    public NetworkButtons buttons;      // 버튼 상태

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

    // 내부에서 사용하는 변수
    public const byte BUTTON_ONPRESSED = 9;    // 마우스 왼쪽클릭 지속

}