using UnityEngine.InputSystem;

public class CharacterInfoInstance
{
    public readonly CharacterInfo data;

    public CharacterInfoInstance(int key)
    {
        data = DataManager.Instance.CharacterInfoLoader.GetByKey(key);
    }

    public int health => data.Health;                           // 체력
    public int defGear => data.DefGear;                         // 방어구 체력
    public int startGold => data.StartGold;                     // 시작 골드
    public int startTeamCoin => data.StartTeamCoin;             // 시작 팀코인
    public int State => data.State;                             // 캐릭터 상태
    public int Weapon1 => data.StartWeapon1;                    // 무기 1
    public int auxiliaryWeapon => data.StartAuxiliaryWeapon;    // 보조무기
    public int consumeItem1 => data.StartConsumeItem1;          // 소모 아이템 1

    public int Weapon2;                                         // 무기 2
    public int consumeItem2;                                    // 소모 아이템 2
    public int consumeItem3;                                    // 소모 아이템 3
}
