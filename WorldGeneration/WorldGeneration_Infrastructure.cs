using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGeneration_Infrastructure : MonoBehaviour {
    [SerializeField] TileBase[] fortress;
    [SerializeField] protected Tilemap map;

    void Start()
    {
        Generate();
    }

    public void Generate() {
        HexTile tile = HexMap.GetHex(new Vector3Int(132, 159, 0));
        tile.InfrastructureTile = fortress[0];
        UpdateInfrastructure();
    }

    public void UpdateInfrastructure() {
        for (int row = 0; row < HexMap.HEIGHT; row++) {
            for (int col = 0; col < HexMap.WIDTH; col++) {
                HexTile tile = HexMap.GetHex(new Vector3Int(row, col, 0));
                map.SetTile(tile.TilemapCoords, tile.InfrastructureTile);
            }
        }
    }
}
