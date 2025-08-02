using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Base class for a service. Singleton but better.
public class MonoService : MonoBehaviour
{
    protected void OnEnable()
    {
        if (Application.isPlaying)
        {
            ServiceLocator.Register(this);
        }
        else 
        {
            try
            {
                ServiceLocator.Register(this);
            }
            catch (Exception e)
            {
            }
        }
        
    }

    
    protected void OnDisable()
    {
        ServiceLocator.UnRegister(this);
    }
}
