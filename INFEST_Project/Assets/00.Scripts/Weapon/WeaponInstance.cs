using UnityEngine.UI;

public class WeaponInstance 
{
    public readonly WeaponInfo data;

    public WeaponInstance(int key)
    {
        data = DataManager.Instance.GetByKey<WeaponInfo>(key);  
        //curMagazineBullet = data.MagazineBullet;
        //curBullet = data.MaxBullet;
        RecoilForce = data.RecoilForce;
        concentration = data.Concentration;
    }

    //public int curMagazineBullet { get; private set; }                          // ���� źâ�� �Ѿ�
    //public int curBullet { get; private set; }                                  // ���� �������� �Ѿ�
    public float RecoilForce { get; private set; }                              // �ݵ� (���ؽ� ����)
    public float concentration { get; private set; }                            // ��ź�� (���ؽ� ����)
    public Image icon { get; set; }                                             // UI�� ���� ������

    //public void ReloadShotgun(int _curBullet, int _curMagazineBullet)
    //{
    //    curBullet = _curBullet;
    //    curMagazineBullet = _curMagazineBullet;
    //}

    //public void Reload(int _curBullet, int _curMagazineBullet)
    //{
    //    curBullet = _curBullet;
    //    curMagazineBullet = _curMagazineBullet;
    //}

    //public void Fire(int _curBullet)
    //{
    //    curMagazineBullet = _curBullet;
    //}

    public void IsAiming()
    {
        concentration += 0.3f;
    }

    public void StopAiming()
    {
        concentration -= 0.3f;
    }

    //public void SupplementBullet()
    //{
    //    curBullet += data.MagazineBullet;
    //    curBullet = Mathf.Min(curBullet, data.MaxBullet);
    //}
}
