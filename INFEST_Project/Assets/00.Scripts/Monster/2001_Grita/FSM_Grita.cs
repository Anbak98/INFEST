using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Grita : MonsterFSM<Monster_Grita>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // MonsterSpawner�� Ÿ���� ��쿡 
        //if (monster.targets.Contains(other.transform))
        //{
        //    _grita.targets.Add(other.transform);
        //    if (_grita.target == null || other.gameObject.layer == _playerDetectLayer)
        //    {
        //        Player player = other.GetComponentInParent<Player>();
        //        if (player == null) return;

        //        _grita.SetTargetRandomly();
        //    }
        //}

        if (other.gameObject.layer == 7)
        {
            /// �׽�Ʈ�� ���� MonsterNetworkBehaviour�� �ӽ÷� �߰��� ClearTargetList�Լ�
            /// private�̶� ����Ʈ�� ���� ���� ����
            // ���� Spawner�� �������Ѵ�
            monster.ClearTargetList();

            /// �÷��̾��� Detector�� PlayerMethodFromMonster�� ������ ���� �ʾƼ� false
            /// �÷��̾ Ÿ������ �������ʴ´�
            monster.TryAddTarget(other.transform);

            monster.SetTargetRandomly();
            monster.FSM.ChangePhase<Grita_Phase_Chase>();
        }
        //else if (other.gameObject.name.Equals(MonsterSpawner)
        //{
        //    //����Spawner�� ���ִٸ� ������ 
        //}
    }
}
