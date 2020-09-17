using UnityEngine;
using UnityEngine.Tilemaps;
public class HexTile {
	public HexCoordinates HexCoords {get; set;}
	public Vector3Int TilemapCoords {get; set;}
	public TileBase MapTile {get; set;}
	public float Height {get; set;}
	public string Biome {get; set;}
	public int BiomeIndex {get; set;}
	public ElevationType Elevation {get; set;}
	public float MoistureLevel {get; set;}
	public MoistureType Moisture {get; set;}
	public TileBase InfrastructureTile {get; set;}
	public InfrastructureType Infrastructure {get; set;}

	public static HexTile NULL = new HexTile(new HexCoordinates(999999999, 999999999), new Vector3Int(0, 0, -10), null);

	public HexTile(HexCoordinates hexCoords, Vector3Int tilemapCoords, TileBase mapTile) {
		HexCoords = hexCoords;
		TilemapCoords = tilemapCoords;
		MapTile = mapTile;
		Height = 0f;
		Biome = "DEFAULT";
		BiomeIndex = -1;
		Elevation = ElevationType.FLAT;
		MoistureLevel = 0f;
		Moisture = MoistureType.NORMAL;
		Infrastructure = InfrastructureType.NONE;
	}

	public void SetTile(Tilemap map, TileBase mapTile) {
		MapTile = mapTile;
		map.SetTile(TilemapCoords, mapTile);
	}

}