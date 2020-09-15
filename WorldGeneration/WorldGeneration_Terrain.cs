using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGeneration_Terrain : MonoBehaviour
{

    [SerializeField] protected int width = 300, height = 300, seed = 375;
    [SerializeField] protected Tilemap map;
    [SerializeField] protected float elevationMountain = 2f, elevationHill = 1.7f, elevationFlat = 0.4f, elevationCoast = 0.2f;
    [SerializeField] protected Biome biomeDefault;
    [SerializeField] protected Biome[] biome;
    
    // Start is called before the first frame update
    void Awake() {
        HexMap.Init(width, height);
        Generate();
    }

    virtual public void Generate() {
        //Random.InitState(seed);

        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                HexCoordinates hexCoords = HexCoordinates.FromTilemapCoordinates(row, col);
                Vector3Int tilemapCoords = new Vector3Int(row, col, 0);
                TileBase tileToSet = biomeDefault.water;
                HexTile hex = new HexTile(hexCoords, tilemapCoords, tileToSet);
                HexMap.SetHex(hex);
                map.SetTile(hex.TilemapCoords, tileToSet);
            }
        }

        for (int i = 0; i < biome.Length; i++) {
            Biome currentBiome = biome[i];
            for (int biomeCount = 0; biomeCount < currentBiome.count; biomeCount++) {
                HexTile biomeCenter = HexMap.GetHex(new Vector3Int(Random.Range(0, width), Random.Range(0, height), 0));
                HexMap.PlaceBiome(biomeCenter, Random.Range(currentBiome.minRange, currentBiome.maxRange), i);
            }
        }        
    }

    public void UpdateTerrain() {
        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                HexTile hex = HexMap.GetHex(new Vector3Int(row, col, 0));

                // Elevation and whole map
                if (hex.Height >= elevationMountain) {
                    hex.MapTile = hex.Biome != -1 ? biome[hex.Biome].mountain : biomeDefault.mountain;
                    hex.Elevation = ElevationType.MOUNTAIN;
                } else if (hex.Height >= elevationHill) {
                    hex.MapTile = hex.Biome != -1 ? biome[hex.Biome].hill : biomeDefault.hill;
                    hex.Elevation = ElevationType.HILL;
                } else if (hex.Height >= elevationFlat) {
                    hex.MapTile = hex.Biome != -1 ? biome[hex.Biome].flat : biomeDefault.flat;
                    hex.Elevation = ElevationType.FLAT;
                } else if (hex.Height >= elevationCoast) {
                    hex.MapTile = hex.Biome != -1 ? biome[hex.Biome].coast : biomeDefault.coast;
                    hex.Elevation = ElevationType.COAST;
                } else {
                    hex.MapTile = hex.Biome != -1 ? biome[hex.Biome].water : biomeDefault.water;
                    hex.Elevation = ElevationType.WATER;
                }

                map.SetTile(hex.TilemapCoords, hex.MapTile);
            }
        }
    }
}
