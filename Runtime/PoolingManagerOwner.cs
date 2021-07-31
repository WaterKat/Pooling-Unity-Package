using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WaterKat.Pooling
{
    public class PoolingManagerOwner : MonoBehaviour
    {
        public event EmptyPoolingHandler OnPoolingManagerOwnerDestroyed;
        public delegate void EmptyPoolingHandler();
        private void OnDestroy()
        {
            OnPoolingManagerOwnerDestroyed?.Invoke();
        }
    }
}
