using System;
using UnityEngine;

namespace Map.Texturing
{
    public class TileTexture 
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

        public BiomeType Biome { get; }
        public TextureType Block { get; }
        public TextureSide Side { get; }

        public TileTexture(BiomeType biomeType, TextureType textureType, TextureSide textureSide)
        {
            Biome = biomeType;
            Block = textureType;
            Side = textureSide;
        }

        public static Vector2[] GetUVs(BiomeType biomeType, TextureType textureType, TextureSide textureSide)
        {
            return Coordinates.GetTexture(biomeType, textureType, textureSide);
        }

        public Vector2[] GetUVs()
        {
            return Coordinates.GetTexture(Biome, Block, Side);
        }

        private static class Coordinates
        {
            public static Vector2[] GetTexture(BiomeType biomeType, TextureType textureType, TextureSide textureSide)
            {
                switch (biomeType)
                {
                    case BiomeType.Normal:
                        return NormalBiome.GetTexture(textureType, textureSide);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(biomeType), biomeType, null);
                }
            }

            private static class NormalBiome
            {

                public static Vector2[] GetTexture(TextureType textureType, TextureSide textureSide)
                {
                    switch (textureType)
                    {
                        case TextureType.Grass:
                            return GrassBlock.GetTexture(textureSide);

                        case TextureType.Gravel:
                            return GravelBlock.GetTexture(textureSide);

                        case TextureType.Stone:
                            return StoneBlock.GetTexture(textureSide);

                        default:
                            throw new ArgumentOutOfRangeException(nameof(textureType), textureType, null);
                    }
                }

                private static class GrassBlock
                {
                    public static Vector2[] GetTexture(TextureSide textureSide)
                    {
                        switch (textureSide)
                        {
                            case TextureSide.Top:
                                return Top;

                            case TextureSide.Side:
                                return Side;

                            case TextureSide.Bottom:
                                return Bottom;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(textureSide), textureSide, null);
                        }
                    }

                    private static readonly Vector2[] Top =
                    {
                        new Vector2(0, 0),
                        new Vector2(0, TileRatio),
                        new Vector2(TileRatio, TileRatio),
                        new Vector2(TileRatio, 0),
                    };

                    private static readonly Vector2[] Side =
                    {
                        new Vector2(TileRatio, 0),
                        new Vector2(TileRatio, TileRatio),
                        new Vector2(TileRatio * 2, TileRatio),
                        new Vector2(TileRatio * 2, 0),
                    };

                    private static readonly Vector2[] Bottom =
                    {
                        new Vector2(TileRatio * 2, 0),
                        new Vector2(TileRatio * 2, TileRatio),
                        new Vector2(TileRatio * 3, TileRatio),
                        new Vector2(TileRatio * 3, 0),
                    };
                }

                private static class GravelBlock
                {
                    public static Vector2[] GetTexture(TextureSide textureSide)
                    {
                        return new []
                        {
                            new Vector2(0, TileRatio),
                            new Vector2(0, TileRatio * 2),
                            new Vector2(TileRatio, TileRatio * 2),
                            new Vector2(TileRatio, TileRatio),
                        };
                    }
                }

                private static class StoneBlock
                {
                    public static Vector2[] GetTexture(TextureSide textureSide)
                    {
                        return new[]
                        {
                            new Vector2(0, TileRatio * 2),
                            new Vector2(0, TileRatio * 3),
                            new Vector2(TileRatio, TileRatio * 3),
                            new Vector2(TileRatio, TileRatio * 2),
                        };
                    }
                }
            }
        }
    }
}