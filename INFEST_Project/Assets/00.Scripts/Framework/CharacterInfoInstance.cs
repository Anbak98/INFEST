using UnityEngine.InputSystem;

public class CharacterInfoInstance
{
    public readonly CharacterInfo data;

    public CharacterInfoInstance(int key)
    {
        data = DataManager.Instance.CharacterInfoLoader.GetByKey(key);
    }

    public int health => data.Health;                           // ü��
    public int defGear => data.DefGear;                         // �� ü��
    public int startGold => data.StartGold;                     // ���� ���
    public int startTeamCoin => data.StartTeamCoin;             // ���� ������
    public int State => data.State;                             // ĳ���� ����
    public int Weapon1 => data.StartWeapon1;                    // ���� 1
    public int auxiliaryWeapon => data.StartAuxiliaryWeapon;    // ��������
    public int consumeItem1 => data.StartConsumeItem1;          // �Ҹ� ������ 1

    public int Weapon2;                                         // ���� 2
    public int consumeItem2;                                    // �Ҹ� ������ 2
    public int consumeItem3;                                    // �Ҹ� ������ 3
}
