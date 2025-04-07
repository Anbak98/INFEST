using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;


public class Weapon : NetworkBehaviour
{
    [Header("Basic"), SerializeField]
    public string name; // ���ݷ�
    public float damage; // ���ݷ�
    public bool isAutomatic; // ���� or �ܹ�
    public float atkRange; // ���� ��Ÿ�
    public float atkSpeed; // ���� �ӵ�
    public float atkAccuracy; // ���� ��Ȯ��
    public int maxBullet; // �ִ� ź��
    public int price; // ����



}
