using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Column;

    // Use this for initialization
    void Start()
    {
    }

    public void Test()
    {
        Debug.Log("Test - Row: " + Row + ", Column: " + Column);
    }
}
