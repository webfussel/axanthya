using UnityEngine;

public class WorldGeneration_Lakeside : WorldGeneration_Base
{
    [SerializeField] private float scale = 40, persistence = 4, lacunarity = 1;
    [SerializeField] private int octaveCount = 3;
    [SerializeField] private Vector2 offset;

    void Awake()
    {
        HexMap.Init(width, height);
        Generate();
    }

    override public void Generate() {
        base.Generate();

        float[,] noiseMap = new float[width, height];

        Vector2[] octaves = new Vector2[octaveCount];
        for (int o = 0; o < octaveCount; o++) {
            float offsetX = Random.Range(-100000, 100000) + offset.x;
            float offsetY = Random.Range(-100000, 100000) + offset.y;
            octaves[o] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int o = 0; o < octaveCount; o++) {
                    float heightRow = (row - halfHeight) / scale * frequency + octaves[o].y;
                    float heightCol = (col - halfWidth) / scale * frequency + octaves[o].x;

                    float perlinValue = Mathf.PerlinNoise (heightRow, heightCol) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                maxNoiseHeight = Mathf.Max(maxNoiseHeight, noiseHeight);
                minNoiseHeight = Mathf.Min(minNoiseHeight, noiseHeight);

                noiseMap[row, col] = noiseHeight;
            }
        }

        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                HexTile hex = HexMap.GetHex(new Vector3Int(row, col, 0));
                float normalizedNoiseValue = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap[row, col]);
                hex.Height = normalizedNoiseValue;
            }
        }

        base.UpdateTerrain();
    }


    override protected void OnValidate() {
        base.OnValidate();
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaveCount < 0) {
            octaveCount = 0;
        }
    }
}
