using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detector에 붙는 스크립트
public class GritaPlayerDetector : NetworkBehaviour
{
    private int _playerDetectLayer = 7;
    [SerializeField] private Monster_Grita _grita;

    // host가 관리하니까 Networked변수가 아니어도 된다
    public bool isTriggered = false; // 중복 트리거 방지(모든 플레이어가 같은 값을 가져야하는 변수)

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (isTriggered) return;    // 중복 트리거 방지

        //Player player = other.GetComponentInParent<Player>();
        //if (player == null || other.gameObject.layer != _playerDetectLayer) return;

        if (!_grita.targets.Contains(other.transform))
        {
            _grita.targets.Add(other.transform);
            if (_grita.target == null || other.gameObject.layer == _playerDetectLayer)
            {
                Player player = other.GetComponentInParent<Player>();
                if (player == null) return;

                _grita.SetTargetRandomly();
            }
        }
        // MonsterGrita 에 변수 선언을 하지 않기 위해 여기서 isTriggered 변수를 수정한다
        isTriggered = true;
    }
}
