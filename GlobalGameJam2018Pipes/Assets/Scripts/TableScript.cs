using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{

    public int tableSizeRows;
    public int tableSizeCols;
    public GameObject assetPrefab;
    public GameObject table;
    public float tableThickness;

    private GameObject[,] assets;

    private float tileSizeZ;    // row height - we think landscape format table as portrait
    private float tileSizeX;    // column width - we think landscape format table as portrait

    private Vector3 tableCenter;
//    private Vector3 localTableDirection;

    // Use this for initialization
    void Start ()
    {

        // table position from global space to local
//        localTablePos = transform.InverseTransformVector (table.transform.position);
//        Debug.Log ("localTablePos x=" + localTablePos.x + ", z=" + localTablePos.z + ", y=" + localTablePos.y);

        // my rotation like the table rotation 
//        localTableDirection = new Vector3(transform.eulerAngles.x, table.transform.eulerAngles.y, transform.eulerAngles.z);
//        transform.rotation = Quaternion.Euler(localTableDirection);

        // init assets
        float tableSizeX = table.GetComponent<Renderer> ().bounds.size.x;
        float tableSizeZ = table.GetComponent<Renderer> ().bounds.size.z;
        float tableHeight = table.GetComponent<Renderer> ().bounds.size.y;
        Debug.Log ("table size: x=" + tableSizeX + ", z=" + tableSizeZ + ", h=" + tableHeight);

        tileSizeX = tableSizeX / tableSizeRows;
        tileSizeZ = tableSizeZ / tableSizeCols;
        Debug.Log ("tileW: " + tileSizeZ + ", tileH: " + tileSizeX);

        tableCenter = table.GetComponent<Renderer>().bounds.center;
        Debug.Log("table center: " + tableCenter);

        assets = new GameObject[tableSizeCols, tableSizeRows];

        for (int row = 0; row < tableSizeRows; row++) {
            for (int column = 0; column < tableSizeCols; column++) {
                GameObject newAsset = Instantiate (assetPrefab, this.transform);
                newAsset.name = "Asset (" + column + ", " + row + ")";

                newAsset.GetComponent<Asset> ().Row = row;
                newAsset.GetComponent<Asset> ().Column = column;

                float positionX = GetXPosition (row, tileSizeX, tableSizeX);
                float positionZ = GetZPosition (column, tileSizeZ, tableSizeZ);
                newAsset.transform.position = new Vector3 (positionX, tableThickness, positionZ);
                Debug.Log ("asset pos: " + positionX + "/" + positionZ);

                assets [column, row] = newAsset;
            }
        }


    }

    private float GetXPosition (int column, float tileSizeX, float tableSizeX)
    {
        Debug.Log ("getX: col=" + column + ", tileSizeX=" + tileSizeX);
        return tileSizeX * column + tableCenter.x + tileSizeX / 2 - tableSizeX / 2;
    }

    private float GetZPosition (int row, float tileSizeZ, float tableSizeZ)
    {
        return tileSizeZ * row + tableCenter.z + tileSizeZ / 2 - tableSizeZ / 2;
    }


    // Update is called once per frame
    void Update ()
    {
        
    }
}
