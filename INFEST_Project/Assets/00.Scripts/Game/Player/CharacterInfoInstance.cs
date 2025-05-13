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
    public int CurHealth { get; set; }                          // ü��
    public int CurDefGear { get; set; }                         // �� ü��
    public int CurDef { get; set; }
    public int CurGold { get; set; }                            // ���� ���
    public int CurTeamCoin { get; set; }                        // ���� ������
    public int curstate { get; set; }                           // ĳ���� ����
}
