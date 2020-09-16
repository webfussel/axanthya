using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGeneration_Base : MonoBehaviour
{

    [SerializeField] protected int width = 300, height = 300, seed = 375;
    [SerializeField] protected int forests = 50, flowerPatches = 150;
    [SerializeField] protected Tilemap terrain, flora;
    [SerializeField] protected float elevationMountain = 2f, elevationHill = 1.7f, elevationFlat = 0.4f, elevationCoast = 0.2f;
    [SerializeField] protected Biome biomeDefault;
    [SerializeField] protected Biome[] biome;

    public bool autoGenerate = false;
    
    // Start is called before the first frame update
    void Awake() {
        HexMap.Init(width, height);
        Generate();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = terrain.WorldToCell(mouseWorldPos);
            HexTile tile = HexMap.GetHex(new Vector3Int(coordinate.y, coordinate.x, 0));
            string display = "Q: {0} | R: {1} | S: {2} | Elevation: {3} | Moisture: {4}";
            Debug.Log(string.Format(display, tile.HexCoords.Q, tile.HexCoords.R, tile.HexCoords.S, tile.Elevation, tile.Moisture));
        }
    }

    virtual public void Generate() {
        Random.InitState(seed);

        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                HexCoordinates hexCoords = HexCoordinates.FromTilemapCoordinates(row, col);
                Vector3Int tilemapCoords = new Vector3Int(row, col, 0);
                TileBase tileToSet = biomeDefault.water;
                HexTile hex = new HexTile(hexCoords, tilemapCoords, tileToSet);
                HexMap.SetHex(hex);
                terrain.SetTile(hex.TilemapCoords, tileToSet);
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

                terrain.SetTile(hex.TilemapCoords, hex.MapTile);
            }
        }

        SetFlora();
    }

    public void SetFlora() {
        for (int trees = 0; trees < forests; trees++) {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            HexMap.MoistureArea(x, y, Random.Range(width / 40, width / 24), 2f);
        }

        for (int flowers = 0; flowers < flowerPatches; flowers++) {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            HexMap.MoistureArea(x, y, Random.Range(width / 40, width / 24), 0.4f);
        }


        for (int row = 0; row < height; row++) {
            for (int col = 0; col < width; col++) {
                HexTile hex = HexMap.GetHex(new Vector3Int(row, col, 0));
                if (hex.Elevation == ElevationType.FLAT || hex.Elevation == ElevationType.HILL) {
                    if (hex.Moisture > 0.7f) {
                        TileBase tile = hex.Biome != -1 ? biome[hex.Biome].trees : biomeDefault.trees;
                        flora.SetTile(hex.TilemapCoords, tile);
                    } else if (hex.Moisture > 0.2f && hex.Moisture < 0.4f) {
                        TileBase tile = hex.Biome != -1 ? biome[hex.Biome].flowers : biomeDefault.flowers;
                        flora.SetTile(hex.TilemapCoords, tile);
                    }
                }
            }
        }
    }

    public void ResetTiles() {
        HexMap.Init(width, height);
        terrain.ClearAllTiles();
        flora.ClearAllTiles();
    }
}
