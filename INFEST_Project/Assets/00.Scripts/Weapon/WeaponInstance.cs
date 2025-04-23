using UnityEngine;
using UnityEngine.UI;

public class WeaponInstance
{
    public readonly WeaponInfo data;

    public WeaponInstance(int key)
    {
        data = DataManager.Instance.GetByKey<WeaponInfo>(key);  
        curMagazineBullet = data.MagazineBullet;
        curBullet = data.MaxBullet;
        RecoilForce = data.RecoilForce;
        concentration = data.Concentration;
    }

    public float curMagazineBullet { get; private set; }                        // ���� źâ�� �Ѿ�
    public float curBullet { get; private set; }                                // ���� �������� �Ѿ�
    public float RecoilForce { get; private set; }                              // �ݵ� (���ؽ� ����)
    public float concentration { get; private set; }                            // ��ź�� (���ؽ� ����)
    public Image icon { get; set; }                                             // UI�� ���� ������
    public float fireRate { get; private set; } = 2;                            // ���� �ӵ�
    public bool isAutomatic { get; private set; } = true;                       // ��� ��� (�ܹ� or �ڵ�)
    public int projectilesPerShot { get; private set; } = 1;                    // ��ݴ� ������ źȯ ����
    public EWeaponType weaponType { get; private set; } = EWeaponType.Rifle;    // ���� ���� (������, ���� ��)

    public void ReloadShotgun()
    {
        curBullet--;
        curMagazineBullet++;
    }

    public void Reload()
    {
        curBullet += curMagazineBullet;
        curMagazineBullet = Mathf.Min(curBullet, data.MagazineBullet);
        curBullet -= Mathf.Min(curBullet, data.MagazineBullet);
    }

    public void IsAiming()
    {
        concentration += 0.3f;
    }

    public void StopAiming()
    {
        concentration -= 0.3f;
    }
}
