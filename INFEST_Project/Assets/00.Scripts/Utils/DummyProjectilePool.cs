using System.Collections.Generic;
using UnityEngine;

public class DummyProjectilePool : MonoBehaviour
{
    [SerializeField] private DummyProjectile _projectilePrefab;
    [SerializeField] private int _initialCount = 22;

    private Queue<DummyProjectile> _pool = new Queue<DummyProjectile>();

    public static DummyProjectilePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        for(int i = 0; i < _initialCount; i++)
        {
            var proj = Instantiate(_projectilePrefab);
            proj.gameObject.SetActive(false);
            _pool.Enqueue(proj);
        }
    }

    public DummyProjectile Get()
    {
        DummyProjectile proj; 

        if(_pool.Count > 0)
        {
            proj = _pool.Dequeue();            
        }
        else
        {
            proj = Instantiate(_projectilePrefab);
        }

        proj.ResetProjectile();
        proj.gameObject.SetActive(true);
        return proj;
    }

    public void Return(DummyProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
        _pool.Enqueue(projectile);
    }
}
