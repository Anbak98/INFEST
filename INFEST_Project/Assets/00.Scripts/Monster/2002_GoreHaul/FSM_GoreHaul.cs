public class FSM_GoreHaul : MonsterFSM<Monster_GoreHaul>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTargetRandomly();
            monster.SetTarget(other.transform);
        }
    }
}
