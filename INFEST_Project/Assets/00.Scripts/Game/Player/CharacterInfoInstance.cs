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

    public int curHealth { get; set; }                          // ü��
    public int curDefGear { get; set; }                         // �� ü��
    public int curGold { get; set; }                            // ���� ���
    public int curTeamCoin { get; set; }                        // ���� ������
    public int curstate { get; set; }                           // ĳ���� ����


}
