using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ConsumeSpawner : NetworkBehaviour
{
    [SerializeField] private Player _player;
    public List<Consume> Consumes;

    public void Throw()
    {
        for(int i=0; i<3; i++)
        {
            if (Consumes[i].key == _player.inventory.consume[0]?.key)
                Consumes[i].CollThrow();
        }
    }

    public void Heal()
    {
        for (int i = 3; i < 4; ++i)
        {
            if (Consumes[i].key == _player.inventory.consume[1]?.key)
                Consumes[i].CollHeal();
        }
    }

    public void Mounting()
    {
        for (int i = 4; i < 5; ++i)
        {
            if (Consumes[i].key == _player.inventory.consume[2]?.key)
                Consumes[i].CollMounting();
        }
    }
}
