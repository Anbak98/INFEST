using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum EPlayerInput
{
    move,
    look,
    jump,
    fire,
    zoom,
    reload,
    interaction,
    useItem,
    run,
    sit,
    scoreboard,
    swap,
    menu
}

/// <summary>
/// Input Action 관리, 이벤트 제공
/// 
/// 
/// GetInput()으로 외부에서 액션 접근 가능하게 함.
/// Update() 사용 안 하고 이벤트 기반 처리.
/// 장점: Player Action의 c# 스크립트를 사용하여 자동으로 매핑되어있어서 따로 매핑할 필요가 없다.
/// </summary>
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// 플레이어의 ActionMap
    /// InputAction은 직렬화가 안 되는 타입
    /// PlayerActionMap을 ScriptableObject나 MonoBehaviour로 인식하지 않는다
    /// 인스펙터에서 연결 불가능
    /// 
    /// </summary>
    public PlayerActionMap PlayerAction { get; private set; }
    public PlayerActionMap.PlayerActions playerActions { get; private set; }
    // PlayerAction.Player.Move.performed += 이벤트함수
    // 무엇이 있는지는 PlayerActionMap을 참고

    private void Awake()
    {
        /// 이런 연결 방식은 심화강의 1-11 참고
        PlayerAction = new PlayerActionMap();
        playerActions = PlayerAction.Player;        
    }

    private void OnEnable()
    {
        PlayerAction?.Enable();
    }

    private void OnDisable()
    {
        PlayerAction?.Disable();
    }

    public void SetActive(bool active)
    {
        if (active)
            PlayerAction?.Enable();
        else
            PlayerAction?.Disable();
    }
    /// <summary>    
    /// PlayerController에서 호출하여 해당 InputAction에 맞는 이벤트를 추가한다
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public InputAction GetInput(EPlayerInput input)
    {
        return input switch
        {
            EPlayerInput.move => PlayerAction.Player.Move,
            EPlayerInput.look => PlayerAction.Player.Look,
            EPlayerInput.jump => PlayerAction.Player.Jump,
            EPlayerInput.fire => PlayerAction.Player.Fire,
            EPlayerInput.zoom => PlayerAction.Player.Zoom,
            EPlayerInput.reload => PlayerAction.Player.Reload,
            EPlayerInput.interaction => PlayerAction.Player.Interaction,
            EPlayerInput.useItem => PlayerAction.Player.UseItem,
            EPlayerInput.run => PlayerAction.Player.Run,
            EPlayerInput.sit => PlayerAction.Player.Sit,
            EPlayerInput.scoreboard => PlayerAction.Player.ScoreBoard,
            EPlayerInput.swap => PlayerAction.Player.Swap,
            EPlayerInput.menu => PlayerAction.Player.Menu,
            _ => null
        };
    }
}
