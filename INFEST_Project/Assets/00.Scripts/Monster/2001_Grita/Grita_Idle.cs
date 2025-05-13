using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Idle : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wander>
{
    public TickTimer _tickTimer;

    public GritaPlayerDetector ditector;


    [Networked] private Quaternion _targetRotation { get; set; }
    [Networked] private NetworkBool _isRotationCompleted { get; set; }
    [Networked] private TickTimer _waitAfterRotation { get; set; }

    [SerializeField] private float _rotationSpeed = 120f; // �ʴ� ȸ�� ����
    [SerializeField] private float _angleThreshold = 30f; // ��� ����



    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }
    public override void Execute()
    {
        base.Execute();
        // Ditector�� Trigger�� �ߵ��Ǿ��ٸ� ScreamState�� �ٲ���Ѵ�
        if (ditector.isTriggered && monster.CanScream() && monster.screamCount < Monster_Grita.screamMaxCount)
            phase.ChangeState<Grita_Scream>();


        if (_tickTimer.Expired(Runner)) // ���� �ð� �ʰ��ϸ� true ����
        {
            phase.ChangeState<Grita_Walk>(); // state ��ü
        }
    }
}
