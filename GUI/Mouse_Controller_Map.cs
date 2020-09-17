using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Mouse_Controller_Map : MonoBehaviour
{
    [SerializeField] protected Texture2D mouseCursor;
    [SerializeField] protected Tilemap terrain, UIGrid;
    [SerializeField] protected TileBase hover;
    [SerializeField] protected Text hexCoords, tilemapCoords, elevation, moisture, infrastructure, biome;
    
    private Vector3Int lastCoords;

    // Start is called before the first frame update
    void Awake() {
        Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = terrain.WorldToCell(mouseWorldPos);

        if(coordinate != lastCoords) {
            UIGrid.SetTile(lastCoords, null);
            UIGrid.SetTile(coordinate, hover);

            HexTile tile = HexMap.GetHex(new Vector3Int(coordinate.y, coordinate.x, 0));
            hexCoords.text = string.Format("Q: {0} | R: {1} | S: {2}", tile.HexCoords.Q, tile.HexCoords.R, tile.HexCoords.S);
            tilemapCoords.text = string.Format("X: {0} | Y: {1}", coordinate.x, coordinate.y);
            elevation.text = string.Format("{0} ({1})", tile.Elevation, tile.Height);
            moisture.text = string.Format("{0} ({1})", tile.Moisture, tile.MoistureLevel);
            infrastructure.text = string.Format("{0}", tile.Infrastructure);
            biome.text = string.Format("{0} ({1})", tile.Biome, tile.BiomeIndex);

            lastCoords = coordinate;
        }
    }
}
