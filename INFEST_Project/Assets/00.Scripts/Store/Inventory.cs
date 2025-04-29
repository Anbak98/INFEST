using UnityEngine;

public class Inventory : MonoBehaviour
{ 
    public Weapon[] weapon = new Weapon[2];
    public Weapon[] auxiliaryWeapon = new Weapon[1];
    public ConsumeInstance[] consume = new ConsumeInstance[3];
    public Weapon equippedWeapon;

    public void AddWeponItme(Weapon item)
    {
        if (item.instance.data.key % 10000 > 200)
        {
            if (weapon[0] == null)
                weapon[0] = item;

            else
                weapon[1] = item;
        }
        else
        {
            auxiliaryWeapon[0] = item;
        }
    }

    public void AddConsumeItme(ConsumeInstance item)
    {
        int key = item.data.key % 10000;
        if (key < 800)
        {
            if (consume[0] == null)
                consume[0] = item;

            else
                consume[0].AddNum();
        }
        else if (key > 800 && key < 900)
        {
            if (consume[1] == null)
                consume[1] = item;
            
            else
                consume[1].AddNum();

        }
        else
        {
            if (consume[2] == null)
                consume[2] = item;

            else
                consume[2].AddNum();
        }
            
    }

    public void RemoveWeaponItem(Weapon item, int index)
    {
        if (item.instance.data.key % 10000 < 200)
            auxiliaryWeapon[index] = null; 
        else
            weapon[index] = null;
    }

    public void RemoveConsumeItem(int index)
    {
        consume[index].RemoveNum();
        if (consume[index].curNum == 0)
        consume[index] = null;
    }
}
