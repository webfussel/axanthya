using System;
using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
	public int Q { get; private set; }
	public int R { get; private set; }
    public int S => -R-Q;

	public HexCoordinates (int q, int r) {
		Q = q;
		R = r;
	}

    public static HexCoordinates FromTilemapCoordinates (int row, int col) {
		int colNew = col < 0 ? col - 1 : col;
		return new HexCoordinates(col, row - colNew/2);
	}
    public static HexCoordinates FromTilemapCoordinates (Vector3Int tilemapCoordinates) {
		return FromTilemapCoordinates(tilemapCoordinates.x, tilemapCoordinates.y);
	}

	public static implicit operator HexCoordinates(Vector3Int tilemapCoordinates)
	{
		return FromTilemapCoordinates(tilemapCoordinates.x, tilemapCoordinates.y);
	}

    public static Vector3Int FromHexCoordinates (int q, int r) {
		int qNew = q < 0 ? q - 1 : q;
		return new Vector3Int(q, r + qNew/2, 0);
	}

    public static Vector3Int FromHexCoordinates (HexCoordinates hexCoordinates) {
		return FromHexCoordinates(hexCoordinates.Q, hexCoordinates.R);
	}

	public static implicit operator Vector3Int(HexCoordinates hexCoordinates)
	{
		return FromHexCoordinates(hexCoordinates.Q, hexCoordinates.R);
	}
}