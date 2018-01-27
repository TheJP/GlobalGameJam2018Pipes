using UnityEngine;

public class PlayBoard : MonoBehaviour
{
    public int boardSize;
    public GameObject tilePrefab;

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
                int tileSize = newTile.GetComponent<Tile>().tileSize;

                newTile.GetComponent<Tile>().Row = row;
                newTile.GetComponent<Tile>().Column = column;

                float positionX = GetXPosition(column, tileSize);
                float positionZ = GetZPosition(row, tileSize);
                newTile.transform.position = new Vector3(positionX, 0, positionZ);

                tiles[column, row] = newTile;
            }
        }
    }

    private float GetXPosition(int column, int tileSize)
    {
        return tileSize * column - ((boardSize - 1) * tileSize / 2);
    }

    private float GetZPosition(int row, int tileSize)
    {
        return tileSize * row - ((boardSize - 1) * tileSize / 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
