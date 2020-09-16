using UnityEngine;

public class WorldGeneration_Continents : WorldGeneration_Base
{
    
    [SerializeField] int countContinents = 3;
    [SerializeField] float heightNoiseResolution = 0.05f, heightNoiseScale = 1.8f; // scale bigger => wilder islands, resolution bigger => bigger chunks of land
    [SerializeField] Vector2 heightNoiseOffset = new Vector2(0f, 0f); // shifts perlin noise map by given values

    void Awake()
    {
        HexMap.Init(width, height);
        Generate();
    }

    override public void Generate() {
        base.Generate();
        int continentSpacing = (width / countContinents);
        for (int c = 0; c < countContinents; c++) {
            int min = (width * height) / 100 / 30;
            int max = (width * height) / 100 / 15;
            int countSplat = Random.Range(min, max);
            bool horizontal = Random.Range(0,100) > 33;

            for (int i = 0; i < countSplat; i++) {
                int range = Random.Range(15, 25);

                int x, y;

                if (horizontal) {
                    x = Random.Range(Random.Range(0, 19), Random.Range(20, 30)) + range + (c * continentSpacing);
                    y = Random.Range(Random.Range(range, width/2), Random.Range(width/4, width));
                } else {
                    x = Random.Range(Random.Range(range, height/2), Random.Range(height/4, height));
                    y = Random.Range(Random.Range(0, 19), Random.Range(20, 30)) + range + (c * continentSpacing);
                }

                HexMap.ElevateArea(x, y, range, 0.2f);
            }
        }

        for (int x = 0; x < height; x++) {
            for (int y = 0; y < width; y++) {
                HexTile hex = HexMap.GetHex(new Vector3Int(x, y, 0));
                float noise =
                    Mathf.PerlinNoise(
                        ((float) y/width/heightNoiseResolution) + heightNoiseOffset.y,
                        ((float) x/height/heightNoiseResolution) + heightNoiseOffset.x
                    ) - 0.5f;
                
                if (hex.Height > elevationMountain) {
                    hex.Height -= 1f;
                }
                hex.Height += noise * heightNoiseScale;
            }
        }

        base.UpdateTerrain();
    }
}
