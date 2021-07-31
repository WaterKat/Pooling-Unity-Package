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

        private MonoBehaviour parentMonobehavior;
        PoolingManagerOwner poolingManagerOwner;

        private GameObject poolContainer;

        public int SoftMinimumCount = 10;
        public int HardMinimumCount = 5;

        public int SoftMaximumCount = 50;
        public int HardMaximumCount = 100;

        public bool FlexibleMaxCount = true;

        private GameObjectPoolingManager() {}
        public GameObjectPoolingManager(MonoBehaviour _parentMonobehavior,GameObject _gameObject)
        {
            if (_parentMonobehavior == null) { Debug.LogWarning("Invalid Monobehavior"); }
            if (_gameObject == null) { Debug.LogWarning("Invalid GameObject"); }

            poolContainer = new GameObject(_gameObject.name+" Pool");

            templateGameObject = UnityEngine.Object.Instantiate(_gameObject);
            templateGameObject.transform.parent = poolContainer.transform;
            templateGameObject.name = _gameObject.name;
            templateGameObject.SetActive(false);
            templateGameObject.AddComponent<PooledObjectTag>();

            parentMonobehavior = _parentMonobehavior;

            poolingManagerOwner = parentMonobehavior.gameObject.GetComponent<PoolingManagerOwner>();
            if (poolingManagerOwner == null)
                poolingManagerOwner = parentMonobehavior.gameObject.AddComponent<PoolingManagerOwner>();
            poolingManagerOwner.OnPoolingManagerOwnerDestroyed += OnDestroy;

            storedPooledObjects = new Stack<GameObject>();

            RefillPool();
        }

        private void RefillPool()
        {
            while (storedPooledObjects.Count < SoftMinimumCount)
            {
                CreateNewPooledObject();
            }
        }
        private void CreateNewPooledObject()
        {
            GameObject _gameObject = UnityEngine.GameObject.Instantiate(templateGameObject);
            _gameObject.transform.parent = poolContainer.transform;

            IPooledGameObject pooledGameObject = _gameObject.GetComponent<IPooledGameObject>();
            pooledGameObject?.SetPoolOwner(this);

            storedPooledObjects.Push(_gameObject);
        }

        public void OnDestroy()
        {
            UnityEngine.Object.Destroy(poolContainer);
        }

        public GameObject Obtain()
        {
            GameObject gameObject = storedPooledObjects.Pop();

            while (gameObject == null)
            {
                RefillPool();
                gameObject = storedPooledObjects.Pop();
            }

            RefillPool();
            return gameObject;
        }

        public void Relinquish(IPooledGameObject _pooledObject)
        {
            if (_pooledObject.Pool != this)
            {
                Debug.LogError("The Pooled Object does not belong to this pool");
                return;
            }

            if ((storedPooledObjects.Count >= HardMaximumCount)&& (!FlexibleMaxCount))
            {
                Terminate(_pooledObject);
                return;
            }

            if (storedPooledObjects.Contains(_pooledObject.gameObject))
            {
                Debug.LogError("The Pooled Object is already in pool");
                return;
            }

            _pooledObject.gameObject.SetActive(false);

            storedPooledObjects.Push(_pooledObject.gameObject);
        }

        public void Terminate(IPooledGameObject _pooledObject)
        {
            UnityEngine.Object.Destroy(_pooledObject.gameObject);
        }
    }
}
