using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<WeaponInstance> _weaponInstance = new();
    [SerializeField] private List<ConsumeInstance> _consumeInstance = new();
    [SerializeField] private int _maxCount;


    public event Action<WeaponInstance> addWeapon;
    public event Action<ConsumeInstance> addConsume;
    public List<WeaponInstance> weaponInstances => _weaponInstance;
    public List<ConsumeInstance> consumeInstance => _consumeInstance;
    public int MaxCount => _maxCount;
    

}
