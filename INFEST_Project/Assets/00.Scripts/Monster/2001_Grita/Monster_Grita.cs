using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Grita : BaseMonster<Monster_Grita>
{
    [SerializeField] private AudioSource _screamSound;
    [SerializeField] private AudioClip _screamSoundClip;
   

    public const int screamMaxCount = 2;  // �ִ� 2���� �����ϴ�
    public int screamCount = 0;  // �ִ� 2���� �����ϴ�

    public TickTimer screamCooldownTimer;
    public const float ScreamCooldownSeconds = 50f;

    public GritaPlayerDetector playerDetector; // ���� �ʿ�

    // �������� �����ؾߵȴ�
    [SerializeField] public MonsterSpawner spawner;


    public override void Render()
    {
        // Animation ����
    }

    public bool CanScream()
    {
        return screamCooldownTimer.ExpiredOrNotRunning(Runner);
    }

    // �Ҹ��� Host�� ��� Player�� �� �ִ� RPC 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_Scream()
    {
        Debug.Log("Scream!");   // ���߿� ����� Ŀ��

        if (_screamSound != null && _screamSoundClip != null)
            _screamSound.PlayOneShot(_screamSoundClip);

        screamCount++;

        // ��Ÿ�� ����
        screamCooldownTimer = TickTimer.CreateFromSeconds(Runner, ScreamCooldownSeconds);       
    }
    #region ��Ÿ�� �� isTriggered�� false��
    public void StartScreamCooldown()
    {
        playerDetector.isTriggered = true;
        // 50�� �Ŀ� ResetTrigger �Լ� �ڵ� ȣ��
        Invoke(nameof(ResetTrigger), ScreamCooldownSeconds);
    }
    private void ResetTrigger()
    {
        playerDetector.isTriggered = false;
        Debug.Log("��Ÿ�� ����: isTriggered = false");
    }
    #endregion

    public float GetCurrentAnimLength()
    {
        AnimatorClipInfo[] clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfos.Length > 0)
        {
            return clipInfos[0].clip.length;
        }
        return 0f;
    }
    public IEnumerator SpawnAfterAnim(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("SpawnAfterAnim ����");

        spawner.SpawnMonsterOnWave(spawner.transform);
    }

}
