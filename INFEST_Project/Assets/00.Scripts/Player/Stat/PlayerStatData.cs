using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� ����, ��������
[Serializable]
public class PlayerStatData /*: NetworkBehaviour*/
{
    //public uint networkId;

    // �÷��̾��� ����
    public int id;             //�÷��̾� ID ����(key)
    public string name;        //�÷��̾� �̸� ����
    public int team;           //�� ����
    public int maxHp;          //�ִ� ü��(Health)
    public int curHp;          //���� ü��

    public int defGear;        // �� ü��
    public int def;            // ����

    public int speedMove;       // �̵��ӵ�

    public int startGold;       // ���� ���
    public int startTeamCoin;   // ���۽� ������

    public int playerState;     // �÷��̾��� ����: 100~112����

    public int startWeapon1Id;          // ����1 ID
    public int startAuxiliaryId;          // �������� ID

    public int startConsumeItem1Id;          // �Һ������1 ID            
}
