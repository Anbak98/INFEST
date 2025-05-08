using UnityEngine.UI;

public class ConsumeInstance
{
    public readonly ConsumeItem data;

    public ConsumeInstance(int key)
    {
        data = DataManager.Instance.GetByKey<ConsumeItem>(key);
        //curNum = 1;
    }

    //public int curNum { get; private set; }                                     // ���� ������ ����
    public Image icon { get; set; }                                             // UI�� ���� ������


}
