using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static List<MonoService> _services = new List<MonoService>();

    public static T GetSystem<T>() where T: MonoService
    {
        foreach (MonoService service in _services)
        {
            if (SystemCompare<T>(service.GetType()))
            {
                return (T)service;
            }
        }
        Debug.LogError($"[ServiceLocator] Get for type {typeof(T)} failed. No registered service. Call Has<T>() before getting.");

        return null;
    }
    
    public static bool HasSystem<T>() where T: MonoService
    {
        foreach (MonoService service in _services)
        {
            if (SystemCompare<T>(service.GetType()))
            {
                return true;
            }
        }

        return false;
    }
    
    private static bool SystemCompare<T>(Type actualSystemType) where T:MonoService
    {
        Type tDerived = typeof(T);
        bool validInterfaceComparison = tDerived.IsInterface && tDerived.IsAssignableFrom(actualSystemType);
        return (actualSystemType.IsSubclassOf(tDerived) || tDerived == actualSystemType || validInterfaceComparison);
    }

    public static void Register(MonoService service)
    {
        foreach (MonoService existing in _services)
        {
            if (existing.GetType() == service.GetType()) //compare against exact type, unlike when looking for system.
            {
                Debug.LogError($"[ServiceLocator] Registering pre-existing service. {service.GetType()}." +
                    $"Not registering duplicate {service.gameObject.name}");
                return;
            }
        }
        _services.Add(service);
    }

    public static void UnRegister(MonoService service)
    {
        _services.Remove(service);
    }
}
