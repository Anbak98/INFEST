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

        Player player = other.GetComponentInParent<Player>();
        if (player == null || other.gameObject.layer != _playerDetectLayer) return;

        // MonsterGrita �� ���� ������ ���� �ʱ� ���� ���⼭ isTriggered ������ �����Ѵ�
        isTriggered = true;
    }
}
