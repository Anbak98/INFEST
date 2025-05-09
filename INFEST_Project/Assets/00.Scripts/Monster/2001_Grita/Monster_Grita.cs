using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Grita : MonsterNetworkBehaviour
{
    private int _playerDetectLayer = 7;

    [Networked] private bool IsTriggered { get; set; } // 중복 트리거 방지(모든 플레이어가 같은 값을 가져야하는 변수)

    public override void Render()
    {

    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        // 타겟이 범위에 들어와야한다
        Player player = other.GetComponentInParent<Player>();
        if (player == null || other.gameObject.layer != _playerDetectLayer) return;

        if (HasInputAuthority)  // 각 Client
            Rpc_RequestTrigger();

        if (HasStateAuthority)  // Host(해당 오브젝트의 네트워크 상태(동기화 변수 등)를 최종적으로 결정할 권한이 있는지 알려줍니다.
        {
            if (IsTriggered) return;    // 중복 트리거 방지
            IsTriggered = true;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_RequestTrigger()
    {
        if (IsTriggered) return;

        // Wave 상태일때와 아닐때 다르다. if문을 쓰지 않는다면, 함수를 구분하는 것이 좋은 방법

        // 몬스터를 7~10 추가 스폰
        // 몬스터별로 스폰 확률이 다르니까 기획서를 참고
    }

    public void Rpc_Scream()
    {
        // 소리지르기(Rpc)
        Debug.Log("Scream!");   // 나중에 지우고 커밋

        // 다른 몬스터들 Spawn된다
        // 가장 가까운 Spawn포인트에서 몇마리를 스폰하고
        // 현재 위치로 뛰어오게 만든다



    }

    // 추가 스폰

}
