using UnityEngine;

public class PlayerMethodFromMonster : MonoBehaviour
{
    [SerializeField] private PlayerStatHandler playerStatHandler;

    public void ApplyDamage(int monsterKey, int damage)
    {
        playerStatHandler.TakeDamage(damage);
    }
}
