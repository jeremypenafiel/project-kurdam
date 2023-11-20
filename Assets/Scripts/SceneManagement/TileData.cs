using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile", menuName = "TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public TileData GetTileData(Vector3Int tileposition)
    {
        TileBase tile = map.GetTile(tileposition);
        if (tile == null)
        {
            return null;
        }
        else
        {
            return dataFromTiles[tile];
        }
    }

}
