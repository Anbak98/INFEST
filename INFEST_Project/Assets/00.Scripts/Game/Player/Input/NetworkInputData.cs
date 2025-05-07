using Fusion;
using UnityEngine;


/// <summary>
/// �÷��̾��� �Է� �����͸� ��Ʈ��ũ�� ����ȭ�ϱ� ���� ����ϴ� ����ü
/// �÷��̾��� �Է� ���¸� ��Ʈ��ũ�� ���� ����
/// </summary>
public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;           // �̵�����(wasd)
    public Vector2 lookDelta;           // ���콺 ������(���� ȸ��)
    public NetworkButtons buttons;      // ��ư ����
    public Vector2 scrollValue;           // ���콺 �� ��;

    // ���콺
    public const byte BUTTON_FIRE = 0;      // ���콺 ����
    public const byte BUTTON_ZOOM = 1;      // ���콺 ������
    public const byte BUTTON_SWAP = 2;      // ���콺 ��

    // Ű����
    public const byte BUTTON_JUMP = 3;      // space
    public const byte BUTTON_RELOAD = 4;    // R
    public const byte BUTTON_INTERACT = 5;  // F
    public const byte BUTTON_USEITEM = 6;   // E
    public const byte BUTTON_RUN = 7;       // lShift
    public const byte BUTTON_SIT = 8;       // lCtrl
    public const byte BUTTON_SCOREBOARD = 9;    // Tab
    public const byte BUTTON_MENU = 10;    // ESC
    public const byte BUTTON_CHANGECAMERA = 11; // Q

    // ���ο��� ����ϴ� ����
    public const byte BUTTON_FIREPRESSED = 12;    // ���콺 ����Ŭ�� ����
    public const byte BUTTON_ZOOMPRESSED = 13;    // ���콺 ������Ŭ�� ����

    // InputAction�� ����
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
    public bool isChangingCamera;


    // ���� ����(Input Action�� ����X)
    public bool isShotgunOnFiring;
    public bool isOnZoom;



    /// �÷��̾��� ���¸� ���⿡ �����ؼ� ������ �Ѵ�    
    /// �ִϸ��̼��� ���¸� ������ �ʴ´�
    /// move, ���� ��������, � ���⸦ �����ߴ��� ������ �Ѿ����
    /// �� ������ ���� ��밡 �׿� �´� �ִϸ��̼��� ���ÿ��� ����
    /// 
    /// PlayerStatHandler�� ������ �����;��Ѵ�
    /// 
    public override string ToString()
    {
        string result =
            $"Direction: {direction}\n" +
            $"LookDelta: {lookDelta}\n" +
            $"ScrollValue: {scrollValue}\n" +
            $"Buttons: {buttons}\n" +
            $"IsJumping: {isJumping}\n" +
            $"IsReloading: {isReloading}\n" +
            $"IsFiring: {isFiring}\n" +
            $"IsZooming: {isZooming}\n" +
            $"IsInteracting: {isInteracting}\n" +
            $"IsUsingItem: {isUsingItem}\n" +
            $"IsRunning: {isRunning}\n" +
            $"IsSitting: {isSitting}\n" +
            $"IsScoreBoardPopup: {isScoreBoardPopup}\n" +
            $"IsMenuPopup: {isMenuPopup}\n" +
            $"IsChangingCamera: {isChangingCamera}\n" +
            $"IsShotgunOnFiring: {isShotgunOnFiring}\n" +
            $"IsOnZoom: {isOnZoom}";
        return result;
    }

}