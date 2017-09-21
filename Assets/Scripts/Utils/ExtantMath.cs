using UnityEngine;

namespace Utils
{
    public static class ExtantMath 
    {
        public static int RoundToNearestChunk(float value)
        {
            return (int)(value / SettingsManager.ChunkSize) * SettingsManager.ChunkSize;
        }

        public static Vector3 RoundToNearestChunk(Vector3 value)
        {
            return new Vector3(
                RoundToNearestChunk(value.x),
                RoundToNearestChunk(value.y),
                RoundToNearestChunk(value.z));
        }
    }
}