using Fusion;

public class Monster_RageFang_FSM : MonsterFSM<Monster_RageFang>
{
    public override void FixedUpdateNetwork()
    {
        if(monster.regionIndex < 5)
        {
            if (monster.IsValidRetreat && monster.RetreatTimer.ExpiredOrNotRunning(Runner))
            {
                ChangePhase<Monster_RageFang_Phase_Retreat>();
                monster.IsValidRetreat = false;
            }
        }

        base.FixedUpdateNetwork();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
        }
    }
}
