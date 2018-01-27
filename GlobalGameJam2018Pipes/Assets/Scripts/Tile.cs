using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Column;
    public int tileSize;

    private List<GameObject> pipes = new List<GameObject>(2);

    // Use this for initialization
    void Start()
    {
        gameObject.transform.localScale = new Vector3(tileSize, 1, tileSize);
    }

    public void Test()
    {
        Debug.Log("Test - Row: " + Row + ", Column: " + Column);
    }
}
