using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Consume : NetworkBehaviour
{
    public int key;
    public ConsumeInstance instance;
    public Player _player;
    public float coolTime;
    public float lastUsedTime;
    public bool isCoolingDown;

    [Networked] public int curNum { get; set; } = 0;  // ���� ������ ����

    public void Awake()
    {
        instance = new(key);
    }

    public void AddNum()
    {
        curNum++;
        curNum = Mathf.Min(curNum, instance.data.MaxNum);
    }
    public void RemoveNum()
    {
        curNum--;
        curNum = Mathf.Max(curNum, 0);
    }


    public virtual void Throw()
    {

    }

    public void CollThrow()
    {
        Throw();
    }

    public virtual void Heal()
    {

    }

    public void CollHeal()
    {
        Heal();
    }

    public virtual void Mounting()
    {

    }

    public void CollMounting()
    {
        Mounting();
    }

}

