using Fusion;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayerProfile : MonoBehaviour
{
    public Image JobIcon;
    public TMPro.TMP_Text Nickname;
    public Sprite NoIcon;
    public Sprite BattleMedicIcon;
    public Sprite DefenderIcon;
    public Sprite CommanderIcon;
    public Sprite DemolatorIcon;

    public void Set(string NickName, JOB job)
    {
        this.Nickname.text = NickName;
        switch (job)
        {
            case JOB.Commander:
                JobIcon.sprite = CommanderIcon;
                break;
            case JOB.BattleMedic:
                JobIcon.sprite = BattleMedicIcon;
                break;
            case JOB.Defender:
                JobIcon.sprite = DefenderIcon;
                break;
            case JOB.Demolator:
                JobIcon.sprite = DemolatorIcon;
                break;
            default:
                JobIcon.sprite = NoIcon;
                break ;
        }
    }

    public void Set(Profile profile)
    {
        this.Nickname.text = profile.NickName.ToString();
        switch (profile.Job)
        {
            case JOB.BattleMedic:
                JobIcon.sprite = BattleMedicIcon;
                break;
            case JOB.Defender:
                JobIcon.sprite = DefenderIcon;
                break;
            case JOB.Commander:
                JobIcon.sprite = CommanderIcon;
                break;
            case JOB.Demolator:
                JobIcon.sprite = DemolatorIcon;
                break;
        }
    }

    public void Clear()
    {
        this.Nickname.text = "";
        JobIcon.sprite = null;
    }
}
