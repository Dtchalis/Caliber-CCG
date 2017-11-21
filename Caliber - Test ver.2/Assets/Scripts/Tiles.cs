using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tiles {

    public GameObject tile { get; set; }
    public enum terrainType { none, normal, impass };
    public terrainType? terrain;

    public Tiles()
    {
        tile = new GameObject();
        terrain = terrainType.normal;
    }

    public Tiles(GameObject _tile, terrainType? _terrain, bool _isActive, bool _isOccupied, bool _isWeakspot)
    {
        tile = _tile;
        //terrain = _terrain;
        //isActive = _isActive;
        //isOccupied = _isOccupied;
        //isWeakspot = _isWeakspot;

        tile.GetComponent<TileManager>().terrain = _terrain;

        tile.GetComponent<TileManager>().isActive = _isActive;
        tile.GetComponent<TileManager>().isOccupied = _isOccupied;
        tile.GetComponent<TileManager>().isWeakspot = _isWeakspot;
    }
}
