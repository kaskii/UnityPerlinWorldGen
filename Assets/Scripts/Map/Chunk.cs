using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Utils;

namespace Map
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {

        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshCollider _meshCollider;

        private static int _currentChunkThreadsActive = 0;

        private Mesh _mesh;

        private List<Vector3> _verts;
        private List<int> _tris;
        private List<Vector2> _uvs;

        private Vector3 _chunkPos;

        private ExtantNoise _extantNoise;

        private Thread _chunkCalculationThread;

        public void Init(Vector3 pos, ExtantNoise extantNoise)
        {
            _chunkPos = pos;

            _mesh = new Mesh();
            _mesh.MarkDynamic();

            _verts = new List<Vector3>();
            _tris = new List<int>();
            _uvs = new List<Vector2>();

            _extantNoise = extantNoise;

            _chunkCalculationThread = new Thread(CalculateGeometry);
        }

        public void QueueCalculation()
        {
            StartCoroutine(ActualQueueCalculation());
        }

        public void Clear()
        {
            _mesh.Clear(false);

            _verts.Clear();
            _tris.Clear();
            _uvs.Clear();
        }

        private IEnumerator ActualQueueCalculation()
        {
            while (_currentChunkThreadsActive >= SettingsManager.MaxChunkCalculationThread)
            {
                yield return null;
            }

            _currentChunkThreadsActive++;

            _chunkCalculationThread.Start();

            while (_chunkCalculationThread.IsAlive)
            {
                yield return null;
            }

            ApplyMesh();

            _currentChunkThreadsActive--;
        }

        private void CalculateGeometry()
        {
            for (int x = 0; x < SettingsManager.ChunkSize; x++)
            {
                for (int z = 0; z < SettingsManager.ChunkSize; z++)
                {
                    // Back
                    if (!IsBlockAt(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z - 1))
                    {
                        CreateFace(new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z), Vector3.up, Vector3.right);
                    }

                    // Forward
                    if (!IsBlockAt(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1))
                    {
                        CreateFace(new Vector3(x + 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1), Vector3.up, Vector3.left);
                    }

                    // Top
                    if (!IsBlockAt(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z) + 1, z))
                    {
                        CreateFace(new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z) + 1, z), Vector3.forward, Vector3.right);
                    }

                    // Bottom
                    if (!IsBlockAt(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z) - 1, z))
                    {
                        CreateFace(new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1), Vector3.back, Vector3.right);
                    }

                    // Left
                    if (!IsBlockAt(x - 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z))
                    {
                        CreateFace(new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1), Vector3.up, Vector3.back);
                    }

                    // Right
                    if (!IsBlockAt(x + 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z))
                    {
                        CreateFace(new Vector3(x + 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z), Vector3.up, Vector3.forward);
                    }
                }
            }
        }

        private void CreateFace(Vector3 corner, Vector3 up, Vector3 right)
        {
            int tIndex = _verts.Count;

            _verts.Add(corner);
            _verts.Add(corner + up);
            _verts.Add(corner + up + right);
            _verts.Add(corner + right);

            _tris.Add(tIndex + 0);
            _tris.Add(tIndex + 1);
            _tris.Add(tIndex + 2);
            _tris.Add(tIndex + 2);
            _tris.Add(tIndex + 3);
            _tris.Add(tIndex + 0);

            _uvs.Add(new Vector2(0, 0));
            _uvs.Add(new Vector2(0, 1));
            _uvs.Add(new Vector2(1, 1));
            _uvs.Add(new Vector2(1, 0));
        }

        private bool IsBlockAt(float x, float y, float z)
        {
            return y <= _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z);
        }

        private void ApplyMesh()
        {
            _mesh.vertices = _verts.ToArray();
            _mesh.triangles = _tris.ToArray();
            _mesh.uv = _uvs.ToArray();

            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();

            _meshFilter.sharedMesh = _mesh;
            _meshCollider.sharedMesh = _mesh;

            _meshRenderer.material.color = Color.gray;
        }
    }
}