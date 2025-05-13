public class Monster_RageFang_FSM : MonsterFSM<Monster_RageFang>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            if (monster.target == null || monster.target.gameObject.layer != 7)
            {
                monster.SetTarget(other.transform);
            }
        }
    }
}
