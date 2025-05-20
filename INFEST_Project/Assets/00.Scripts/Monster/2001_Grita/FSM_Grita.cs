using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Grita : MonsterFSM<Monster_Grita>
{
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;

    public TickTimer screamDealy;



    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTarget(other.transform);
            monster.SetTargetRandomly();

            /// Ÿ������ �÷��̾ ������
            /// ���⿡�� Scream Ƚ��, Scream�� ��Ÿ�� �Ǵ��ϰ�
            /// ���� ������ �ε����� �����Ͽ� �ܼ��� Chase�� ������
            /// Scream�� �� ������ �����Ѵ�

            if (screamDealy.ExpiredOrNotRunning(Runner))
            {
                screamDealy = TickTimer.CreateFromSeconds(Runner, 50f);
                monster.FSM.ChangePhase<Grita_Phase_Scream>();
            }
        }
    }


    /// ���⿡�� ���� ���¸� �Ǵ��ؾ��Ѵ�



}
