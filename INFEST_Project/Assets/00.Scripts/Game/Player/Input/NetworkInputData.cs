using Fusion;
using UnityEngine;

/// <summary>
/// �÷��̾��� �Է� �����͸� ��Ʈ��ũ�� ����ȭ�ϱ� ���� ����ϴ� ����ü
/// �÷��̾��� �Է� ���¸� ��Ʈ��ũ�� ���� ����
/// </summary>
public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;           // �̵�����(wasd)
    public Vector2 lookDelta;             // ���콺 ������(���� ȸ��)
    public NetworkButtons buttons;      // ��ư ����

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

    // ���ο��� ����ϴ� ����
    public const byte BUTTON_ONPRESSED = 9;    // ���콺 ����Ŭ�� ����

}