using UnityEngine;
using UnityEngine.UI;

public class ConsumeInstance
{
    public readonly ConsumeItem data;

    public ConsumeInstance(int key)
    {
        data = DataManager.Instance.GetByKey<ConsumeItem>(key);
    }

    public int curNum { get; private set; } = 1;                                // ���� ������ ����
    public Image icon { get; set; }                                             // UI�� ���� ������

    public void AddNum()
    {
        curNum++;
        curNum = Mathf.Min(curNum, data.MaxNum);

    }
    public void RemoveNum()
    {
        curNum--;
        curNum = Mathf.Max(curNum, 0);
    }
}
