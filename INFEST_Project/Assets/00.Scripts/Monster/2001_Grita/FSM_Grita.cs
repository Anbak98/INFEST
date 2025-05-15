using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Grita : MonsterFSM<Monster_Grita>
{
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;



    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("����");
            monster.TryAddTarget(other.transform);
            monster.SetTarget(other.transform);
            monster.SetTargetRandomly();
            
            /// Ÿ������ �÷��̾ ������
            /// ���⿡�� Scream Ƚ��, Scream�� ��Ÿ�� �Ǵ��ϰ�
            /// ���� ������ �ε����� �����Ͽ� �ܼ��� Chase�� ������
            /// Scream�� �� ������ �����Ѵ�

            monster.FSM.ChangePhase<Grita_Phase_Scream>();
        }
    }


    /// ���⿡�� ���� ���¸� �Ǵ��ؾ��Ѵ�
    


}
