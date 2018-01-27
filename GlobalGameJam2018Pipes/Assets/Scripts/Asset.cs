using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset : MonoBehaviour
{
    public int Row;
    public int Column;
    public int tileSizeX;
    public int tileSizeZ;

    private GameObject assetObj;
    private int count;


    // Use this for initialization
    void Start ()
    {
        gameObject.transform.localScale = new Vector3 (tileSizeX, 1, tileSizeZ);
    }

    public void Test ()
    {
        Debug.Log ("Test - Row: " + Row + ", Column: " + Column);
    }

    // Update is called once per frame
    void Update ()
    {
        
    }
}
