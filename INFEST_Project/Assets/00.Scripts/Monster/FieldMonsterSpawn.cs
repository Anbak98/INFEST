using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class FieldMonsterSpawn : NetworkBehaviour
{
    public NetworkPrefabRef Monster;

    Player[] players;
    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            if (players == null)
            {
                players = FindObjectsOfType<Player>();
            }

            int rand = Random.Range(10, 20);

            for (int i = 0; i < rand; i++)
            {
                MonsterNetworkBehaviour mnb = Runner.Spawn(Monster, PossiblePosition()).GetComponent<MonsterNetworkBehaviour>();
                mnb.GetComponent<NavMeshAgent>().enabled = true;
                mnb.TrySetTarget(players[Random.Range(0, players.Length)].transform);
            }
        }
    }

    public Vector3 PossiblePosition()
    {
        float radius = 100f;
        Vector2 randomVector = Random.insideUnitCircle * radius;
        Vector3 randomPos = new Vector3(randomVector.x, 0, randomVector.y);

        NavMeshHit hit;
        bool isValid = NavMesh.SamplePosition(
            randomPos,     // 검사할 위치
            out hit,
            0.1f,                // 매우 짧은 거리 → 계산 최소화
            NavMesh.AllAreas     // 모든 네브메시 영역
        );

        if(isValid)
            return randomPos;

        return PossiblePosition();
    }
}
