using System;
using UnityEngine;

namespace WaterKat.Pooling
{   
    public interface IPool { }

    public interface IGameObjectPool : IPool
    {
        GameObject Obtain();
        void Relinquish(IPooledGameObject _pooledObject);

        void Terminate(IPooledGameObject _pooledObject);
    }

    public interface IObjectPool : IPool
    {
        UnityEngine.Object Obtain();
        void Relinquish(IPooledGameObject _pooledObject);

        void Terminate(IPooledGameObject _pooledObject);
    }
}