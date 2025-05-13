using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detector�� �ٴ� ��ũ��Ʈ
public class GritaPlayerDetector : NetworkBehaviour
{
    private int _playerDetectLayer = 7;
    [SerializeField] private Monster_Grita _grita;

    // host�� �����ϴϱ� Networked������ �ƴϾ �ȴ�
    public bool isTriggered = false; // �ߺ� Ʈ���� ����(��� �÷��̾ ���� ���� �������ϴ� ����)

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (isTriggered) return;    // �ߺ� Ʈ���� ����

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
        // MonsterGrita �� ���� ������ ���� �ʱ� ���� ���⼭ isTriggered ������ �����Ѵ�
        isTriggered = true;
    }
}
