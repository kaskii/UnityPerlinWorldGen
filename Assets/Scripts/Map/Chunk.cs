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

        private Vector3 _chunkPos;
        private ExtantNoise _extantNoise;
        private Thread _chunkCalculationThread;
        private MeshData _meshData;

        private Texture _tilemap;

        public void Init(Vector3 pos, ExtantNoise extantNoise)
        {
            _chunkPos = pos;

            _extantNoise = extantNoise;

            _chunkCalculationThread = new Thread(CalculateGeometry);
            _meshData = new MeshData();
        }

        public void SetTilemap(Texture tilemap)
        {
            _tilemap = tilemap;
        }

        public void QueueCalculation()
        {
            StartCoroutine(ActualQueueCalculation());
        }

        public void Clear()
        {
            // TODO: MeshData clear
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
                        _meshData.CreateFace(
                            new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z), Vector3.up,
                            Vector3.right, new Texturing.TileTexture(
                                Texturing.TileTexture.BiomeType.Normal,
                                Texturing.TileTexture.TextureType.Grass,
                                Texturing.TileTexture.TextureSide.Side));
                    }

                    // Forward
                    if (!IsBlockAt(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1))
                    {
                        _meshData.CreateFace(
                            new Vector3(x + 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1),
                            Vector3.up, Vector3.left, new Texturing.TileTexture(
                                Texturing.TileTexture.BiomeType.Normal,
                                Texturing.TileTexture.TextureType.Grass,
                                Texturing.TileTexture.TextureSide.Side));
                    }

                    // Top
                    if (!IsBlockAt(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z) + 1, z))
                    {
                        _meshData.CreateFace(
                            new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z) + 1, z),
                            Vector3.forward, Vector3.right, new Texturing.TileTexture(
                                Texturing.TileTexture.BiomeType.Normal,
                                Texturing.TileTexture.TextureType.Grass,
                                Texturing.TileTexture.TextureSide.Top));
                    }

                    // Bottom
                    if (!IsBlockAt(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z) - 1, z))
                    {
                        _meshData.CreateFace(
                            new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1),
                            Vector3.back, Vector3.right, new Texturing.TileTexture(
                                Texturing.TileTexture.BiomeType.Normal,
                                Texturing.TileTexture.TextureType.Grass,
                                Texturing.TileTexture.TextureSide.Bottom));
                    }

                    // Left
                    if (!IsBlockAt(x - 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z))
                    {
                        _meshData.CreateFace(
                            new Vector3(x, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z + 1), Vector3.up,
                            Vector3.back, new Texturing.TileTexture(
                                Texturing.TileTexture.BiomeType.Normal,
                                Texturing.TileTexture.TextureType.Grass,
                                Texturing.TileTexture.TextureSide.Side));
                    }

                    // Right
                    if (!IsBlockAt(x + 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z))
                    {
                        _meshData.CreateFace(
                            new Vector3(x + 1, _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z), z), Vector3.up,
                            Vector3.forward, new Texturing.TileTexture(
                                Texturing.TileTexture.BiomeType.Normal,
                                Texturing.TileTexture.TextureType.Grass,
                                Texturing.TileTexture.TextureSide.Side));
                    }
                }
            }
        }



        private bool IsBlockAt(float x, float y, float z)
        {
            return y <= _extantNoise.GetHeight(x + _chunkPos.x, z + _chunkPos.z);
        }

        private void ApplyMesh()
        {
            _meshFilter.sharedMesh = _meshData.CurrentMesh;
            _meshCollider.sharedMesh = _meshData.CurrentMesh;

            _meshRenderer.material.mainTexture = _tilemap;
        }
    }
}