using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IService { }

public class ServiceLocator : MonoBehaviourSingleton<ServiceLocator>
{
    public ServiceLocator() { }
}