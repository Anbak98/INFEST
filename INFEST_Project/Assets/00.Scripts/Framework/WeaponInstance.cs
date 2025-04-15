using Microsoft.Unity.VisualStudio.Editor;

public class WeaponInstance
{
    public readonly WeaponInfo data;

    public WeaponInstance(int key)
    {
        data = DataManager.Instance.WeaponInfoLoader.GetByKey(key);
    }

    public float curMagazineBullet => data.MagazineBullet;  // 현재 탄창의 총알
    public float curBullet => data.MaxBullet;               // 현재 보유중인 총알
    public float RecoilForce => data.RecoilForce;           // 반동 (조준시 변경)
    public float concentration => data.Concentration;       // 집탄율 (조준시 변경)

    public Image icon;                                      // UI에 사용될 아이콘

    public float fireRate;                                  // 공격 속도

    public bool isAutomatic;                                // 사격방식 (단발 or 자동)
}
