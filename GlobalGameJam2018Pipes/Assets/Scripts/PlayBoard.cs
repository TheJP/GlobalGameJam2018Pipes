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
                newTile.GetComponent<Tile>().Row = row;
                newTile.GetComponent<Tile>().Column = column;
                newTile.name = "Tile (" + row + ", " + column + ")";
                tiles[column, row] = newTile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
