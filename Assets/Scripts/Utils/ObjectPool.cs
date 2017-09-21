using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Utils
{
    public class ObjectPool
    {
        private readonly GameObject _prefab;
        private readonly Queue<GameObject> _objects;
        private readonly Transform _parentTransform;

        public int Size => _objects.Count;

        public ObjectPool(GameObject prefab, Transform parent = null)
        {
            _prefab = prefab;
            _objects = new Queue<GameObject>();
            _parentTransform = parent;

            InitializePool(100);
        }

        public ObjectPool(GameObject prefab, int startingSize, Transform parent = null)
        {
            _prefab = prefab;
            _objects = new Queue<GameObject>();
            _parentTransform = parent;

            InitializePool(startingSize);
        }

        public GameObject GetObject()
        {
            if (_objects.Count == 0)
            {
                InstantiateNewObjectToPool();
            }

            GameObject newObject = _objects.Dequeue();
            newObject.SetActive(true);

            return newObject;
        }

        public void AddObject(GameObject newObject)
        {
            newObject.SetActive(false);
            _objects.Enqueue(newObject);
        }

        private void InitializePool(int startingSize)
        {
            for (int i = 0; i < startingSize; i++)
            {
                InstantiateNewObjectToPool();
            }
        }

        private void InstantiateNewObjectToPool()
        {
            GameObject newObject = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
            newObject.SetActive(false);

            if (_parentTransform != null)
            {
                newObject.transform.SetParent(_parentTransform);
            }

            _objects.Enqueue(newObject);
        }
    }
}