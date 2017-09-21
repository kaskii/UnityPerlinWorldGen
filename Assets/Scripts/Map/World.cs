using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Map
{
    public class World : MonoBehaviour
    {
        [SerializeField] private GameObject _chunkPrefab;
        [SerializeField] private Transform _playerTransform;

        private Dictionary<Vector3, Chunk> _chunks;

        private ObjectPool _chunkPool;
        private Queue<Vector3> _chunksToDestroy;
        private Queue<Vector3> _chunksToCreate;
        private Vector3 _lastChunkCheckPos;
        private bool _checkingChunks;
        private bool _handlingChunks;

        private ExtantNoise _extantNoise;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            UpdateChunkQueue();
            HandleChunkChanges();
        }

        private void Init()
        {
            _chunks = new Dictionary<Vector3, Chunk>();

            _chunkPool = new ObjectPool(_chunkPrefab, SettingsManager.ViewRange / SettingsManager.ChunkSize * 2, transform);
 
            _chunksToCreate = new Queue<Vector3>();
            _chunksToDestroy = new Queue<Vector3>();
            _lastChunkCheckPos = Vector3.back;
            _checkingChunks = false;
            _handlingChunks = false;

            _extantNoise = new ExtantNoise(1337);
        }

        /// <summary>
        /// Marks which chunks needs to be instantiated and which needs to be destroyed
        /// </summary>
        private void UpdateChunkQueue()
        {
            if (_checkingChunks ||
                _lastChunkCheckPos.Equals(ExtantMath.RoundToNearestChunk(_playerTransform.position))) return;

            _checkingChunks = true;

            List<Vector3> chunkPositions = new List<Vector3>();

            for (int x = ExtantMath.RoundToNearestChunk(_playerTransform.position.x) - SettingsManager.ViewRange;
                x <= ExtantMath.RoundToNearestChunk(_playerTransform.position.x) + SettingsManager.ViewRange;
                x += SettingsManager.ChunkSize)
            {
                for (int z = ExtantMath.RoundToNearestChunk(_playerTransform.position.z) - SettingsManager.ViewRange;
                    z < ExtantMath.RoundToNearestChunk(_playerTransform.position.z) + SettingsManager.ViewRange;
                    z += SettingsManager.ChunkSize)
                {
                    Vector3 newPos = new Vector3(x, 0, z);

                    chunkPositions.Add(newPos);

                    if (!_chunks.ContainsKey(newPos))
                        _chunksToCreate.Enqueue(newPos);
                }
            }

            foreach (KeyValuePair<Vector3, Chunk> kv in _chunks)
            {
                if (!chunkPositions.Contains(kv.Key))
                    _chunksToDestroy.Enqueue(kv.Key);
            }

            chunkPositions.Clear();
            _checkingChunks = false;
            _lastChunkCheckPos = ExtantMath.RoundToNearestChunk(_playerTransform.position);
        }

        /// <summary>
        /// Checks queues and handles chunk instantiate/destroy function
        /// </summary>
        private void HandleChunkChanges()
        {
            if (_handlingChunks) return;

            _handlingChunks = true;

            for (int i = 0; i < SettingsManager.ChunkOperationsPerFrame; i++)
            {
                if (_chunksToDestroy.Count > 0)
                {
                    DestroyChunk(_chunksToDestroy.Dequeue());
                }

            }

            for (int i = 0; i < SettingsManager.ChunkOperationsPerFrame; i++)
            {
                if (_chunksToCreate.Count > 0)
                {
                    CreateChunk(_chunksToCreate.Dequeue());
                }
            }

            _handlingChunks = false;
        }

        private void CreateChunk(Vector3 pos)
        {
            Chunk chunk = _chunkPool.GetObject().GetComponent<Chunk>();
            chunk.gameObject.transform.position = pos;

            chunk.Init(pos, _extantNoise);
            chunk.QueueCalculation();

            _chunks.Add(pos, chunk);
        }

        private void DestroyChunk(Vector3 pos)
        {
            _chunks[pos].Clear();
            _chunkPool.AddObject(_chunks[pos].gameObject);
            _chunks.Remove(pos);
        }
    }
}