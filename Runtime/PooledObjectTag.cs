using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterKat.Pooling
{
    public class PooledObjectTag : MonoBehaviour, IPooledGameObject
    {
        private IGameObjectPool pool;

        public IGameObjectPool Pool { get => pool; }

        public void SetPoolOwner(IGameObjectPool _pool)
        {
            pool = _pool;
        }
        public void ReturnToPool()
        {
            pool.Relinquish(this);
        }
    }
}
