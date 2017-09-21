using UnityEngine;

namespace Map
{
    public static class TileTexture 
    {
        public enum BiomeType
        {
            Normal
        }

        public enum TextureSide
        {
            Top,
            Side,
            Bottom
        }

        public enum TextureType
        {
            Grass,
            Gravel,
            Stone
        }

        private static float TileRatio => SettingsManager.TileRatio;

        public static class Coordinates
        {
            public static class NormalBiome
            {
                public static class GrassBlock
                {
                    public static Vector2[] Top =
                    {
                        new Vector2(0, 0),
                        new Vector2(0, TileRatio),
                        new Vector2(TileRatio, TileRatio),
                        new Vector2(TileRatio, 0),
                    };

                    public static Vector2[] Side =
                    {
                        new Vector2(TileRatio, 0),
                        new Vector2(TileRatio, TileRatio),
                        new Vector2(TileRatio * 2, TileRatio),
                        new Vector2(TileRatio * 2, 0),
                    };

                    public static Vector2[] Bottom =
                    {
                        new Vector2(TileRatio * 2, 0),
                        new Vector2(TileRatio * 2, TileRatio),
                        new Vector2(TileRatio * 3, TileRatio),
                        new Vector2(TileRatio * 3, 0),
                    };
                }
            }
        }
    }
}