using Microsoft.Unity.VisualStudio.Editor;

public class WeaponInstance
{
    public readonly WeaponInfo data;

    public WeaponInstance(int key)
    {
        data = DataManager.Instance.WeaponInfoLoader.GetByKey(key);
    }

    public float curMagazineBullet => data.MagazineBullet;  // ���� źâ�� �Ѿ�
    public float curBullet => data.MaxBullet;               // ���� �������� �Ѿ�
    public float RecoilForce => data.RecoilForce;           // �ݵ� (���ؽ� ����)
    public float concentration => data.Concentration;       // ��ź�� (���ؽ� ����)

    public Image icon;                                      // UI�� ���� ������

    public float fireRate;                                  // ���� �ӵ�

    public bool isAutomatic;                                // ��ݹ�� (�ܹ� or �ڵ�)
}
