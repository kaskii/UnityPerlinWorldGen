using UnityEngine;

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
            return (ushort) (Mathf.PerlinNoise((x + _noiseOffset.x) / SettingsManager.NoiseScale,
                                 (z + _noiseOffset.z) / SettingsManager.NoiseScale) * SettingsManager.NoiseMultiplier +
                             5);
        }
    }
}