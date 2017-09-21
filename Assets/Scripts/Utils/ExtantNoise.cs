using Map.Texturing;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public class ExtantNoise
    {
        

        private Vector3 _noiseOffset;

        public ExtantNoise(int seed)
        {
            Random.InitState(seed);

            _noiseOffset = new Vector3(Random.Range(-10000, 10000), 0, Random.Range(-10000, 10000));
        }

        public ushort GetHeight(float x, float z)
        {
            return (ushort) (Mathf.PerlinNoise((x + _noiseOffset.x) / SettingsManager.NoiseHeightmapScale,
                                 (z + _noiseOffset.z) / SettingsManager.NoiseHeightmapScale) * SettingsManager.NoiseHeightmapMultiplier +
                             5);
        }

        public TileTexture.BiomeType GetBiome(float x, float z)
        {
            return TileTexture.BiomeType.Normal;
        }
    }
}