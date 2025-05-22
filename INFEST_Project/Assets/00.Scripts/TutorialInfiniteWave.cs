using Fusion;
using INFEST.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInfiniteWave : NetworkBehaviour
{
    private bool IsTriggerd = false;
    public LayerMask playerLayerMask;
    private TickTimer timer;
    private TickTimer boosTimer;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if(((1 << other.gameObject.layer) & playerLayerMask) != 0)
        {
            IsTriggerd = true;
            timer = TickTimer.CreateFromSeconds(Runner, 10);
            boosTimer = TickTimer.CreateFromSeconds(Runner, 60);
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (IsTriggerd)
        {
            if(timer.ExpiredOrNotRunning(Runner))
            {
                NetworkGameManager.Instance.monsterSpawner.CallWave(FindAnyObjectByType<Player>().transform);
                timer = TickTimer.CreateFromSeconds(Runner, 10);
            }

            if (boosTimer.Expired(Runner))
            {
                NetworkGameManager.Instance.monsterSpawner.JustWaveSpawn(FindAnyObjectByType<Player>().transform, 3001);
                IsTriggerd = false;
            }
        }
    }
}
