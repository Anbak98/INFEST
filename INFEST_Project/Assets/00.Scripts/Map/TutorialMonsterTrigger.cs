using System.Collections.Generic;

using Fusion;
using UnityEngine;


public class TutorialMonsterTrigger : NetworkBehaviour
{
    public TutorialController tutorialController;
    private int _monsterLayer = 14;
    [SerializeField] private MonsterNetworkBehaviour _monster;


    private void Update()
    {
        if (_monster == null && tutorialController.page == 2)
        {
            List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

            Runner.LagCompensation.OverlapSphere(
                origin: transform.position,
                radius: 20f,
                hits: hits,
                layerMask: 1 << _monsterLayer,
                queryTriggerInteraction: QueryTriggerInteraction.Ignore,
                player: Runner.LocalPlayer
            );

            foreach (var hit in hits)
            {
                _monster = hit.GameObject.GetComponentInParent<MonsterNetworkBehaviour>();
            }
            return;
        }
        else if(_monster == null)
            return;


        if (_monster.IsDead)
        {
            tutorialController.TextChanged();
            Runner.Despawn(Object);
        }
    }

    
    


}
