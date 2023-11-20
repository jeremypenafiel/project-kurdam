using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TIleManager : MonoBehaviour

{

    [SerializeField] public List<TileData> tileDatas;

    public Dictionary<TileBase, TileData> dataFromTiles;

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
