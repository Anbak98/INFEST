using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInputButton
{
    move,       // wasd
    jump,       // space
    fire,     // lButton
    zoom,       // rButton

    Reload,     // R
    interaction,    // F
    useItem,        // E
    run,        // lShift
    sit,        // lCtrl
    scoreboard  // Tab
}

/// <summary>
/// �÷��̾��� �Է��� �޾Ƽ� ����
/// ��Ʈ��ũ ����ȭ, FSM, Controller ��� ������ ����
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; } = Vector3.zero;      // �̵� ���� (Vector3)
    public Vector2 LookInput { get; private set; } = Vector2.zero;     // �ü� ���� (Vector2)

    public float RotationX { get; private set; } = 0f;       // ���콺 X ȸ�� (pitch)
    public float RotationY { get; private set; } = 0f;       // ���콺 Y ȸ�� (yaw)

    public bool IsJumping { get; private set; } = false;

    public bool IsRunning { get; private set; } = false;

    public bool IsFiring { get; private set; } = false;

    public bool IsZooming { get; private set; } = false;

    private void Start()
    {

    }
}
