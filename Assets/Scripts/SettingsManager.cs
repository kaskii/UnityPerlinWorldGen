using UnityEngine;

public static class SettingsManager
{
    // World bounds
    public const int ChunkSize = 16;
    
    // World performance
    public const int ChunkOperationsPerFrame = 999;
    public const int MaxChunkCalculationThread = 2;
    public const int ViewRange = 128;

    // Noise
    public const float NoiseScale = 90.3f;
    public const float NoiseMultiplier = 12.2f;
}