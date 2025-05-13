using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Idle : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    public TickTimer idleTickTimer;

    public GritaPlayerDetector ditector;


    [Networked] private Quaternion _targetRotation { get; set; }
    [Networked] private NetworkBool _isRotationCompleted { get; set; }
    [Networked] private TickTimer _waitAfterRotation { get; set; }

    [SerializeField] private float _rotationSpeed = 120f; // 초당 회전 각도
    [SerializeField] private float _angleThreshold = 30f; // 허용 각도


    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        idleTickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }
    public override void Execute()
    {
        base.Execute();
        if (ditector.isTriggered && monster.CanScream() && monster.screamCount < Monster_Grita.screamMaxCount)
            phase.ChangeState<Grita_Scream>();

        if (idleTickTimer.Expired(Runner)) // 일정 시간 초과하면 true 리턴
        {
            phase.ChangeState<Grita_Walk>(); // state 교체
        }
    }
}
