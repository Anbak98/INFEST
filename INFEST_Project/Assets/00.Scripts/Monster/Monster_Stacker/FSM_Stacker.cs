public class FSM_Stacker : MonsterFSM<Monster_Stacker>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
            monster.SetTarget(other.transform);
            monster.FSM.ChangePhase<Stacker_Phase_Chase>();
        }
    }
}
