using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoard : MonoBehaviour
{
    public int boardSize;
    public GameObject tilePrefab;
    public List<ItemSink> itemSinks;

    private GameObject[,] tiles;

    // Use this for initialization
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
                int tileSize = newTile.GetComponentInChildren<TileDisplay>().tileSize;

                newTile.GetComponent<Tile>().Row = row;
                newTile.GetComponent<Tile>().Column = column;

                float positionX = GetXPosition(column, tileSize);
                float positionZ = GetZPosition(row, tileSize);
                newTile.transform.position = new Vector3(positionX, 0, positionZ);

                tiles[column, row] = newTile;
            }
        }
    }

    public int GetStepSize()
    {
        return tiles[0, 0].GetComponent<Tile>().tileSize;
    }

    public Tile GetTileForPosition(int column, int row)
    {
        try
        {
            return tiles[column, row].GetComponent<Tile>();
        }
        catch (IndexOutOfRangeException/* ioofException*/)
        {
            //Debug.Log(ioofException.ToString());
            return null;
        }
    }

    public float GetXPosition(int column, int tileSize)
    {
        return tileSize * column - ((boardSize - 1) * tileSize / 2);
    }

    public float GetZPosition(int row, int tileSize)
    {
        return tileSize * row - ((boardSize - 1) * tileSize / 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
