using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct System<S> where S: MonoService
{
    private S _cachedSystem;
    public bool Exists => ServiceLocator.HasSystem<S>();

    public S Value
    {
        get
        {
            if (_cachedSystem == null)
            {
                _cachedSystem = ServiceLocator.GetSystem<S>();
            }

            return _cachedSystem;
        }
    }
}
