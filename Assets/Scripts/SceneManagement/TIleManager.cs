using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TIleManager : MonoBehaviour

{
    [SerializeField] public Tilemap map;
    [SerializeField] public List<TileData> tileDatas;

    public Dictionary<TileBase, TileData> dataFromTiles;

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

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }
}
