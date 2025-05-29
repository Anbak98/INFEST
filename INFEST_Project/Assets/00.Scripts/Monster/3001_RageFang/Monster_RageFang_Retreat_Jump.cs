using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_RageFang_Retreat_Jump : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Retreat>
{
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;
    public float jumpDistance = 10f;
    public float obstacleDetectDistance = 2f;
    public LayerMask obstacleLayer;

    private bool isJumping = false;

    public override void Enter()
    {
        base.Enter();
        monster.AIPathing.enabled = false;
        isJumping = true;
        Vector3 targetPoint = transform.root.position + transform.root.forward * jumpDistance;
        StartCoroutine(JumpOverObstacle(targetPoint));
    }

    public override void Execute()
    {
        base.Execute();
        if(!isJumping)
        {
            phase.ChangeState<Monster_RageFang_Retreat_Retreat>();
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.AIPathing.enabled = true;
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
