using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Grita : MonsterNetworkBehaviour
{
    private int _playerDetectLayer = 7;

    [Networked] private bool IsTriggered { get; set; } // �ߺ� Ʈ���� ����(��� �÷��̾ ���� ���� �������ϴ� ����)

    public override void Render()
    {

    }

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        // Ÿ���� ������ ���;��Ѵ�
        Player player = other.GetComponentInParent<Player>();
        if (player == null || other.gameObject.layer != _playerDetectLayer) return;

        if (HasInputAuthority)  // �� Client
            Rpc_RequestTrigger();

        if (HasStateAuthority)  // Host(�ش� ������Ʈ�� ��Ʈ��ũ ����(����ȭ ���� ��)�� ���������� ������ ������ �ִ��� �˷��ݴϴ�.
        {
            if (IsTriggered) return;    // �ߺ� Ʈ���� ����
            IsTriggered = true;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_RequestTrigger()
    {
        if (IsTriggered) return;

        // Wave �����϶��� �ƴҶ� �ٸ���. if���� ���� �ʴ´ٸ�, �Լ��� �����ϴ� ���� ���� ���

        // ���͸� 7~10 �߰� ����
        // ���ͺ��� ���� Ȯ���� �ٸ��ϱ� ��ȹ���� ����
    }

    public void Rpc_Scream()
    {
        // �Ҹ�������(Rpc)
        Debug.Log("Scream!");   // ���߿� ����� Ŀ��

        // �ٸ� ���͵� Spawn�ȴ�
        // ���� ����� Spawn����Ʈ���� ����� �����ϰ�
        // ���� ��ġ�� �پ���� �����



    }

    // �߰� ����

}
