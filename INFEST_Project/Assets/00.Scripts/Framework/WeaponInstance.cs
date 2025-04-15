using System.ComponentModel;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class WeaponInstance
{
    public readonly WeaponInfo data;
    public float curMagazineBullet => data.MagazineBullet;  // ���� źâ�� �Ѿ�
    public float curBullet => data.MaxBullet;               // ���� �������� �Ѿ�
    public float RecoilForce => data.RecoilForce;           // �ݵ� (���ؽ� ����)
    public float concentration => data.Concentration;       // ��ź�� (���ؽ� ����)

    public Image icon;                                      // UI�� ���� ������

    public float fireRate;                                  // ���� �ӵ�

    public bool isAutomatic;                                // ��ݹ�� (�ܹ� or �ڵ�)

}
