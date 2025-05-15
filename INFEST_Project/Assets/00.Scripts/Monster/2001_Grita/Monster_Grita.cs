using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Monster_Grita : BaseMonster<Monster_Grita>
{
    [SerializeField] private AudioSource _screamSound;
    [SerializeField] private AudioClip _screamSoundClip;

    public TickTimer screemCooldownTickTimer;   // ��ų�� ��Ÿ���� grita�� ��� phase, state�� �����ؾ��Ѵ�

    public const int screamMaxCount = 2;  // �ִ� 2���� �����ϴ�
    public int screamCount = 0;  // �ִ� 2���� �����ϴ�

    public const float ScreamCooldownSeconds = 50f;

    //public GritaPlayerDetector playerDetector; // ���� �ʿ�

    // �������� ����
    [SerializeField] public MonsterSpawner spawner;


    #region animotor parameter
    //[Networked, OnChangedRender(nameof(OnLookXChanged))]
    //public float LookX { get; set; } = 0f;

    [Networked, OnChangedRender(nameof(OnIsScreamChanged))]
    public NetworkBool IsScream { get; set; } = false;
    [Networked, OnChangedRender(nameof(OnIsCooltimeCharged))]
    public NetworkBool IsCooltimeCharged { get; set; } = true;
    [Networked, OnChangedRender(nameof(OnScreamCount))]
    public int ScreamCount { get; set; } = 0;
    

    //private void OnLookXChanged() => animator.SetFloat("LookX", LookX);
    private void OnIsScreamChanged() => animator.SetBool("IsScream", IsScream);
    private void OnIsCooltimeCharged() => animator.SetBool("IsCooltimeCharged", IsCooltimeCharged);
    private void OnScreamCount() => animator.SetInteger("ScreamCount", ScreamCount);
    #endregion

    // Timer �ʱ�ȭ ���� ���� 
    public override void Spawned()
    {
        base.Spawned();

        // ��ų�� Timer �ʱ�ȭ(ó�� 1���� �׳� �� �� �־�� �Ѵ�)
        screemCooldownTickTimer = TickTimer.CreateFromSeconds(Runner, 0f);
    }

    public override void Render()
    {
        // Animation ����
        animator.SetFloat("MovementSpeed", CurMovementSpeed);
        if (IsAttack)
            animator.SetTrigger("IsAttack");
        if (IsDead)
            animator.SetBool("IsDead", IsDead);
        AIPathing.speed = CurMovementSpeed;

        // ��Ÿ�� ���� �ڵ� üũ
        if (screemCooldownTickTimer.Expired(Runner))
        {
            IsCooltimeCharged = true;
            //playerDetector.isTriggered = false;
        }

        //// Ÿ���� ������ ��� LookX ���
        //if (target != null)
        //{
        //    Vector3 toTarget = (target.position - transform.position).normalized;
        //    toTarget.y = 0; // ���� ����

        //    // forward�κ����� �¿� ����: 90������ ���� 1, ����/���ݴ뿡�� 0
        //    LookX = Vector3.Dot(transform.right, toTarget);
        //    LookX = Mathf.Clamp(LookX, -1f, 1f);
        //    animator.SetFloat("LookX", LookX);
        //}
    }


    public bool CanScream()
    {
        return screemCooldownTickTimer.Expired(Runner);
    }

    // �Ҹ��� Host�� ��� Player�� �� �ִ� RPC 
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void Rpc_Scream()
    {
        Debug.Log("Scream!");   // ���߿� ����� Ŀ��

        if (_screamSound != null && _screamSoundClip != null)
            _screamSound.PlayOneShot(_screamSoundClip);

        screamCount++;

        // ��Ÿ�� ����(�ι�° ���ʹ� ��Ÿ�� 50��)
        screemCooldownTickTimer = TickTimer.CreateFromSeconds(Runner, ScreamCooldownSeconds);
        IsCooltimeCharged = false;
    }

    //#region ��Ÿ�� �� isTriggered�� false��
    //public void StartScreamCooldown()
    //{
    //    playerDetector.isTriggered = true;
    //    // 50�� �Ŀ� ResetTrigger �Լ� �ڵ� ȣ��
    //    Invoke(nameof(ResetTrigger), ScreamCooldownSeconds);
    //}
    //private void ResetTrigger()
    //{
    //    playerDetector.isTriggered = false;
    //    IsCooltimeCharged = true;  
    //    Debug.Log("��Ÿ�� ����: isTriggered = false");
    //}
    //#endregion

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
