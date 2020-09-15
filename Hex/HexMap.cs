using System.Collections.Generic;
using UnityEngine;
public class HexMap {
	private static HexTile[,] HexTiles;
	public static int HEIGHT {get; private set;}
	public static int WIDTH {get; private set;}

	public static void Init(int width, int height) {
		HexTiles = new HexTile[width, height];
		HexMap.HEIGHT = height;
		HexMap.WIDTH = width;
	}

	public static HexTile GetHex(Vector3Int tilemapCoords) {
		if (tilemapCoords.x >= 0 && tilemapCoords.x < HEIGHT && tilemapCoords.y >= 0 && tilemapCoords.y < WIDTH) {
			return HexMap.HexTiles[tilemapCoords.y, tilemapCoords.x];
		} else {
			return HexTile.NULL;
		}
	}

	public static void SetHex(HexTile hexTile) {
		HexMap.HexTiles[hexTile.TilemapCoords.x, hexTile.TilemapCoords.y] = hexTile;
	}

	public static HexTile[] GetTilesWithinRange(HexTile center, int range) {
		List<HexTile> results = new List<HexTile>();
		for (int offsetCol = -range; offsetCol <= range; offsetCol++) {
			for (int offsetRow = Mathf.Max(-range, -offsetCol-range); offsetRow <= Mathf.Min(range, -offsetCol+range); offsetRow++) {
				HexTile tile = HexMap.GetHex(new HexCoordinates(center.HexCoords.Q + offsetCol, center.HexCoords.R + offsetRow));
				if (tile.TilemapCoords.z > -1) {
					results.Add(tile);
				}
			}
		}
		return results.ToArray();
	}

	public static void PlaceBiome(HexTile center, int range, int biome) {;
		HexTile[] area = GetTilesWithinRange(center, range);
		
		foreach(HexTile tile in area) {
			float percentage = (int)(Mathf.Lerp(1f, 0.05f, GetDistance(center, tile) / range) * 100);
			int random = Random.Range(1, 50);
			if (random < percentage) {
				tile.Biome = biome;
			}
		}
	}

	public static void ElevateArea(int x, int y, int range, float centerHeight = 0.5f) {
		HexTile center = GetHex(new Vector3Int(x, y, 0));
		HexTile[] area = GetTilesWithinRange(center, range);
		

		foreach(HexTile tile in area) {
			if (tile.Height < 0) tile.Height = 0;
			tile.Height += centerHeight * Mathf.Lerp(1f, 0.25f, GetDistance(center, tile) / range);
		}
	}

	public static float GetDistance(HexTile from, HexTile to) {
		return
			Mathf.Max(
				Mathf.Abs(from.HexCoords.Q - to.HexCoords.Q),
				Mathf.Abs(from.HexCoords.R - to.HexCoords.R),
				Mathf.Abs(from.HexCoords.S - to.HexCoords.S)
			);
	}

}