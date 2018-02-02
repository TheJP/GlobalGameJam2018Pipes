using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoard : MonoBehaviour
{
    public int boardSize;
    public GameObject tilePrefab;
    public List<ItemSink> itemSinks;

    private GameObject[,] tiles;
    private int tileSize;

    private void Awake()
    {
        tileSize = tilePrefab.GetComponentInChildren<TileDisplay>().tileSize;
    }

    void Start()
    {
        //Init Board
        tiles = new GameObject[boardSize, boardSize];
        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 0; column < boardSize; column++)
            {
                GameObject newTile = Instantiate(tilePrefab, this.transform);
                newTile.name = "Tile (" + column + ", " + row + ")";

                newTile.GetComponent<Tile>().Row = row;
                newTile.GetComponent<Tile>().Column = column;

                float positionX = GetXPosition(column);
                float positionZ = GetZPosition(row);
                newTile.transform.position = new Vector3(positionX, 0, positionZ);

                tiles[column, row] = newTile;
            }
        }
    }

    public Tile GetTileForPosition(int column, int row)
    {
        if(column < 0 || column >= boardSize || row < 0 || row >= boardSize)
        {
            return null;
        }
        
        return tiles[column, row].GetComponent<Tile>();
    }

    public float GetXPosition(int column)
    {
        return tileSize * column - ((boardSize - 1) * tileSize / 2);
    }

    public float GetZPosition(int row)
    {
        return tileSize * row - ((boardSize - 1) * tileSize / 2);
    }
}
