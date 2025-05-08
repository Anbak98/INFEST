using Cinemachine;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public GrenadeProjectile grenadeProjectile;

    [SerializeField] private LayerMask _playerLayer = 7;
    [SerializeField] private LayerMask _monsterLayer = 12;

    private int _damage;
    private Player _player;

    private PlayerCameraHandler _playerCameraHandler;  // 흔들릴 카메라
    private CinemachineBasicMultiChannelPerlin noise;

    public void Awake()
    {
        _damage = grenadeProjectile.obj.GetComponent<Grenade>().instance.data.Effect;
        _player = grenadeProjectile.obj.GetComponent<Grenade>().throwPoint.GetComponentInParent<Player>();
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (other.gameObject.layer == 7 && player)
        {
            _playerCameraHandler = player.GetComponentInChildren<PlayerCameraHandler>();
            noise = _playerCameraHandler.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _playerCameraHandler.Shake(3f, 1f, 3f);
        }
    }
    public void Explosion()
    {
        int layerMask = (1 << _playerLayer) | (1 << _monsterLayer);

        UnityEngine.Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, layerMask);

        foreach (UnityEngine.Collider other in colliders)
        {
            if (other.gameObject.layer == _playerLayer)
            {
                Player _otherplayer = other.GetComponentInParent<Player>();

                _otherplayer.statHandler.TakeDamage(_damage / 100);

            }

            if (other.gameObject.layer == _monsterLayer)
            {
                MonsterNetworkBehaviour _monster = other.GetComponentInParent<MonsterNetworkBehaviour>();
                Debug.Log("몬스터 이름" + _monster);
                ApplyDamage(_monster, transform.position, (transform.position - _monster.transform.position).normalized);

            }
        }
    }

    private void ApplyDamage(MonsterNetworkBehaviour _monster, Vector3 pos, Vector3 dir)
    {
        if (_monster == null) return;

        if (_monster.CurrentHealth == 0 || _monster.IsDead == true) return;

        if (_monster.ApplyDamage(_player.Runner.LocalPlayer, _damage, pos, dir, 0, false) == false)
            return;

    }
}