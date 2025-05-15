public class FSM_Bowmeter : MonsterFSM<Monster_Bowmeter>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
            monster.FSM.ChangePhase<Bowmeter_Phase_Chase>();
        }
    }
}
