using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾��� ��������
public class PlayerStatData : NetworkBehaviour
{
    public uint networkId;
    public string id;              //�÷��̾� ID ����
    public string name;         //�÷��̾� �̸� ����
    public uint team;            //�� ����
    public uint maxHp;           //�ִ� ü��
    public uint curHp;           //���� ü��
    public int weapon;          //��� �ִ� ���� ����

    public uint kills;           // ų ��
    public uint deaths;          // ���� ��
    public uint assists;         // ��� ��
    public bool isAlive;        // ���� ����

    /// <summary>
    /// Animator�� ������ ����
    /// </summary>
    public Vector3 position;    //�÷��̾� �� ��ġ
    public Vector3 lookInput;   //��� ���� �ִ��� ����     
    public Vector3 moveInput;   //�̵� ���� ����

    public float rotationX;
    public float rotationY;     //���콺 ȸ�� ��

    public bool isJumping;      //���� ������
    public bool isFiring;       //���� ������
    public bool hitSuccess;     //���ݿ� ���� �ߴ���
    public string hitTargetId;  //������ Ÿ���� ID�� ��������
}
