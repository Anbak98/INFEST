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

    // ���ο��� ����ϴ� ����
    public const byte BUTTON_FIREPRESSED = 10;    // ���콺 ����Ŭ�� ����
    public const byte BUTTON_ZOOMPRESSED = 11;    // ���콺 ������Ŭ�� ����

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

    // ���� ����(Input Action�� ����X)
    public bool isShotgunOnFiring;
    public bool isOnZoom;



    /// �÷��̾��� ���¸� ���⿡ �����ؼ� ������ �Ѵ�    
    /// �ִϸ��̼��� ���¸� ������ �ʴ´�
    /// move, ���� ��������, � ���⸦ �����ߴ��� ������ �Ѿ����
    /// �� ������ ���� ��밡 �׿� �´� �ִϸ��̼��� ���ÿ��� ����
    /// 
    /// PlayerStatHandler�� ������ �����;��Ѵ�
}