using UnityEngine;

public class CharacterInfoInstance
{
    public readonly CharacterInfo data;

    public CharacterInfoInstance(int key)
    {
        data = DataManager.Instance.GetByKey<CharacterInfo>(key);
        CurHealth = data.Health;
        CurDefGear = data.DefGear;
        CurDef = data.Def;
        CurGold = data.StartGold;
        CurTeamCoin = data.StartTeamCoin;
        CurSpeedMove = data.SpeedMove;
        curstate = data.State;
    }

    public int CurSpeedMove { get; set; }
    public int CurHealth { get; set; }                          // 체력
    public int CurDefGear { get; set; }                         // 방어구 체력
    public int CurDef { get; set; }
    public int CurGold { get; set; }                            // 시작 골드
    public int CurTeamCoin { get; set; }                        // 시작 팀코인
    public int curstate { get; set; }                           // 캐릭터 상태
}
