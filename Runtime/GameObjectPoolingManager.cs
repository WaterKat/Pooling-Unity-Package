using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace WaterKat.Pooling
{
    [Serializable]
    public class GameObjectPoolingManager: IGameObjectPool
    {
        private GameObject templateGameObject;

        private Stack<GameObject> storedPooledObjects;
        private Stack<GameObject> activePooledObjects;

        private MonoBehaviour parentMonobehavior;
        PoolingManagerOwner poolingManagerOwner;

        private GameObject poolContainer;

        public int SoftMinimumCount = 10;
        public int HardMinimumCount = 5;

        public int SoftMaximumCount = 50;
        public int HardMaximumCount = 100;

        private GameObjectPoolingManager() {}
        public GameObjectPoolingManager(MonoBehaviour _parentMonobehavior,GameObject _gameObject)
        {
            poolContainer = new GameObject(_gameObject.name+" Pool");

            templateGameObject = UnityEngine.Object.Instantiate(_gameObject);
            templateGameObject.SetActive(false);
            templateGameObject.AddComponent<PooledObjectTag>();

            parentMonobehavior = _parentMonobehavior;

            poolingManagerOwner = parentMonobehavior.gameObject.GetComponent<PoolingManagerOwner>();
            if (poolingManagerOwner == null)
                poolingManagerOwner = poolingManagerOwner.gameObject.AddComponent<PoolingManagerOwner>();
            poolingManagerOwner.OnPoolingManagerOwnerDestroyed += OnDestroy;

            storedPooledObjects = new Stack<GameObject>();

            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < SoftMinimumCount; i++)
            {
                CreateNewPooledObject();
            }
        }

        private void CreateNewPooledObject()
        {
            GameObject _gameObject = UnityEngine.GameObject.Instantiate(templateGameObject);
            storedPooledObjects.Push(_gameObject);
        }

        public void OnDestroy()
        {
            foreach (GameObject pooledObject in storedPooledObjects)
            {
                UnityEngine.Object.Destroy(pooledObject);
            }
            UnityEngine.Object.Destroy(poolContainer);
        }

        public GameObject Obtain()
        {
            GameObject gameObject = storedPooledObjects.Pop();
            activePooledObjects.Push(gameObject);
            return gameObject;
        }

        public GameObject Relinquish(IPooledGameObject _pooledObject)
        {
            if (_pooledObject.Pool == this)
            {
                _pooledObject.gameObject.SetActive(false);
                storedPooledObjects.Push(_pooledObject.gameObject);
            }
            else
            {
                Debug.LogError
            }
        }

        public GameObject Terminate(IPooledGameObject _pooledObject)
        {
            throw new NotImplementedException();
        }
    }
}
