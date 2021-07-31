using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterKat.Pooling
{
    public interface IPooledGameObject
    {
        IGameObjectPool Pool { get; }
        GameObject gameObject { get; }
        void SetPoolOwner(IGameObjectPool _pool);
        void ReturnToPool();
    }
}
