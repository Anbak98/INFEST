﻿using Fusion;
using UnityEngine;


/// <summary>
/// 플레이어의 입력 데이터를 네트워크로 동기화하기 위해 사용하는 구조체
/// 플레이어의 입력 상태를 네트워크를 통해 전달
/// </summary>
public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;           // 이동방향(wasd)
    public Vector2 lookDelta;           // 마우스 움직임(시점 회전)
    public NetworkButtons buttons;      // 버튼 상태
    public Vector2 scrollValue;           // 마우스 휠 값;

    // 마우스
    public const byte BUTTON_FIRE = 0;      // 마우스 왼쪽
    public const byte BUTTON_ZOOM = 1;      // 마우스 오른쪽
    public const byte BUTTON_SWAP = 2;      // 마우스 휠

    // 키보드
    public const byte BUTTON_JUMP = 3;      // space
    public const byte BUTTON_RELOAD = 4;    // R
    public const byte BUTTON_INTERACT = 5;  // F
    public const byte BUTTON_USEITEM = 6;   // E
    public const byte BUTTON_RUN = 7;       // lShift
    public const byte BUTTON_SIT = 8;       // lCtrl
    public const byte BUTTON_SCOREBOARD = 9;    // Tab

    // 내부에서 사용하는 변수
    public const byte BUTTON_FIREPRESSED = 10;    // 마우스 왼쪽클릭 지속
    public const byte BUTTON_ZOOMPRESSED = 11;    // 마우스 오른쪽클릭 지속


    // InputAction
    public bool isJumping;
    public bool isReloading;
    public bool isFiring;
    public bool isZooming;
    public bool isInteracting;
    public bool isUsingItem;
    public bool isRunning;
    public bool isSitting;
    public bool isScoreBoardPopup;
    public bool isMenuPopup;

    // (Input Action) 내부변수
    public bool isShotgunOnFiring;
    public bool isOnZoom;

}