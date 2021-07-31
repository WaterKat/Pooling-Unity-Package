using System;
using UnityEngine;

namespace WaterKat.Pooling
{
    public interface IPool { }

    public interface IGameObjectPool : IPool
    {
        GameObject Obtain();
        GameObject Relinquish(IPooledGameObject _pooledObject);

        GameObject Terminate(IPooledGameObject _pooledObject);
    }

    public interface IObjectPool : IPool
    {
        UnityEngine.Object Obtain();
        UnityEngine.Object Relinquish(IPooledGameObject _pooledObject);

        UnityEngine.Object Terminate(IPooledGameObject _pooledObject);
    }
}