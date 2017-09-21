using UnityEngine;

public static class SettingsManager
{
    // World bounds
    public const int ChunkSize = 16;
    
    // World performance
    public const int ChunkOperationsPerFrame = 999;
    public const int MaxChunkCalculationThread = 4;
    public const int ViewRange = 128;

    // Noise
    public const float NoiseHeightmapScale = 90.3f;
    public const float NoiseHeightmapMultiplier = 12.2f;
    public const float NoiseBiomeScale = 398.333f;
    public const float NoiseBiomeMultiplier = 1f;

    // Tilemap
    public const int TilemapSize = 512;
    public const int TileSize = 16;
    public const float TileRatio = 0.03125F; // 1f / (TilemapSize / TileSize)
}