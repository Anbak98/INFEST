using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RageFang_Retreat_Jump : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Retreat>
{
    public float moveSpeed = 20f;
    public float jumpHeight = 20f;

    private bool isJumping = false;

    [SerializeField] private ParticleSystem ps;

    public override void Enter()
    {
        base.Enter();
        isJumping = true;
    }

    public override void Execute()
    {
        base.Execute();
        if(!isJumping)
        {
            monster.FSM.ChangePhase<Monster_RageFang_Phase_Wonder>();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Effect()
    {
        base.Effect();
        RPC_PlayPS();
        StartCoroutine(JumpOverObstacle(phase.targetPosition));
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_PlayPS()
    {
        ps.Play();
    }


    private IEnumerator JumpOverObstacle(Vector3 target)
    {
        isJumping = true;

        Vector3 startPos = transform.position;
        float duration = Vector3.Distance(startPos, target) / moveSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // 수평 보간
            Vector3 horizontal = Vector3.Lerp(startPos, target, t);

            // 포물선 계산 (간단한 방식)
            float height = 4 * jumpHeight * t * (1 - t);
            transform.root.position = horizontal + Vector3.up * height;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.root.position = target;

        isJumping = false;
    }
}
