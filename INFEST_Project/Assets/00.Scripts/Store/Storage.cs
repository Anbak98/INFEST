using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<WeaponInstance> _weaponInstance = new List<WeaponInstance>();
    [SerializeField] private List<ConsumeInstance> _consumeInstance = new();
    [SerializeField] private int _maxCount;

    public IReadOnlyList<WeaponInstance> weaponInstances => _weaponInstance;
    public IReadOnlyList<ConsumeInstance> consumeInstance => _consumeInstance;
    public int MaxCount => _maxCount;
    

}
