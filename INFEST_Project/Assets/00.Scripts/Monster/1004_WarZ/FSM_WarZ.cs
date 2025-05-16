using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_WarZ : MonsterFSM<Monster_WarZ>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            // Ÿ���� ü���� 0�� �ƴ϶�� �߰�, 0�̸� �߰����� �ʴ´ٸ�?           
            // target�� null�̶� wander�� walk������ randomPosition���� SetDestination�ϴϱ� ������

            monster.TryAddTarget(other.transform);

            monster.SetTargetRandomly();
            monster.FSM.ChangePhase<WarZ_Phase_Chase>();
        }
    }
}
