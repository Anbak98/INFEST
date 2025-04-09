using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : ItemObject
{
    public List<T> Objects { get; private set; } 
    private BaseObjectFactory<T> _factory;
}
