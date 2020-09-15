using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Biome", menuName = "Biome", order = 1)]
public class Biome : ScriptableObject
{
  [SerializeField] public TileBase mountain, hill, flat, coast, water;
  [SerializeField] public int count, minRange, maxRange;
}