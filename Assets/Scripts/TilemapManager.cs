using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public List<Tilemap> grassTilemaps;
    public List<Tilemap> waterTilemaps;

    public bool IsPositionOnGrass(Vector3 worldPosition)
    {
        foreach (Tilemap tilemap in grassTilemaps)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
            if (tilemap.HasTile(cellPosition))
            {
             //   Debug.Log("On grass tile");
                return true;
            }
        }
        return false;
    }

    public bool IsPositionOnWater(Vector3 worldPosition)
    {
        foreach (Tilemap tilemap in waterTilemaps)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
            if (tilemap.HasTile(cellPosition))
            {
              //  Debug.Log("On water tile");
                return true;
            }
        }
        return false;
    }
}
