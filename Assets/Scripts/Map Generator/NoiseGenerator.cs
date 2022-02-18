using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, int octaves, float scale, float lacunarity, Vector2 offset, float persistance)
	{
		float[,] noiseMap = new float[mapWidth, mapHeight];
		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) - offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= persistance;
		}
		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;
		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				amplitude = 1;
				frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++)
				{
					float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
					float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxLocalNoiseHeight)
				{
					maxLocalNoiseHeight = noiseHeight;
				}
				if (noiseHeight < minLocalNoiseHeight)
				{
					minLocalNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;

			}
		}
		return noiseMap;
	}
}

