using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Consume : NetworkBehaviour
{
    public int key;
    public ConsumeInstance instance;

    [Networked] public int curNum { get; set; }    // 현재 아이템 갯수

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
    public override void Spawned()
    {
        instance = new(key);
        curNum = 1;
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

