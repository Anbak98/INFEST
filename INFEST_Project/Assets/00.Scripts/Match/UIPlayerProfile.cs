using Fusion;
using UnityEngine;
using UnityEngine.UI;


public class UIPlayerProfile : MonoBehaviour
{
    public Image JobIcon;
    public TMPro.TMP_Text NickName;
    public Sprite MedicIcon;
    public Sprite DefenderIcon;
    public Sprite SWATIcon;
    public Sprite DemolatorIcon;

    public void Set(string NickName, JOB job)
    {
        this.NickName.text = NickName;
        switch (job)
        {
            case JOB.Medic:
                JobIcon.sprite = MedicIcon;
                break;
            case JOB.Defender:
                JobIcon.sprite = DefenderIcon;
                break;
            case JOB.SWAT:
                JobIcon.sprite = SWATIcon;
                break;
            case JOB.Demolator:
                JobIcon.sprite = DemolatorIcon;
                break;
        }
    }
}
