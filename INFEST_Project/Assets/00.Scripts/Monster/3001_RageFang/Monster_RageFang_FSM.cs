using Fusion;
using UnityEngine;

public class Monster_RageFang_FSM : MonsterFSM<Monster_RageFang>
{
    public LayerMask targetLayerMask;

    public override void FixedUpdateNetwork()
    {
        if(monster.regionIndex < 5)
        {
            if (monster.IsValidRetreat && monster.RetreatTimer.Expired(Runner))
            {
                ChangePhase<Monster_RageFang_Phase_Retreat>();
                monster.IsValidRetreat = false;
            }
        }

        base.FixedUpdateNetwork();
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if ((targetLayerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
        }
    }
}
