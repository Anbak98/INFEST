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
/// 플레이어의 입력을 받아서 저장
/// 네트워크 동기화, FSM, Controller 등에서 가져다 쓴다
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; } = Vector3.zero;      // 이동 방향 (Vector3)
    public Vector2 LookInput { get; private set; } = Vector2.zero;     // 시선 방향 (Vector2)

    public float RotationX { get; private set; } = 0f;       // 마우스 X 회전 (pitch)
    public float RotationY { get; private set; } = 0f;       // 마우스 Y 회전 (yaw)

    public bool IsJumping { get; private set; } = false;

    public bool IsRunning { get; private set; } = false;

    public bool IsFiring { get; private set; } = false;

    public bool IsZooming { get; private set; } = false;

    private void Start()
    {

    }
}
