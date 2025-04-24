using UnityEngine;

public class CharacterInfoInstance
{
    public readonly CharacterInfo data;

    public CharacterInfoInstance(int key)
    {
        data = DataManager.Instance.GetByKey<CharacterInfo>(key);
        curHealth = data.Health;
        curDefGear = data.DefGear;
        curGold = data.StartGold;
        curTeamCoin = data.StartTeamCoin;
        curstate = data.State;
    }

    public int curHealth { get; set; }                          // 체력
    public int curDefGear { get; set; }                         // 방어구 체력
    public int curGold { get; set; }                            // 시작 골드
    public int curTeamCoin { get; set; }                        // 시작 팀코인
    public int curstate { get; set; }                           // 캐릭터 상태


}
