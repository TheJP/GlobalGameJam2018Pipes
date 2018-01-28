using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asset : MonoBehaviour
{
    public int Row;
    public int Column;
    public int tileSizeX;
    public int tileSizeZ;
    
    //private GameObject assetObj;
    private Inventory inventory;
        
    /*[HideInInspector]*/
    public int pipeCount;  // number of assets stored here
    public PipeType? pipeType;

    private Text textObj;


    // Use this for initialization
    void Start ()
    {
        gameObject.transform.localScale = new Vector3 (tileSizeX, 1, tileSizeZ);

        textObj = GetComponentInChildren<Text>();

        inventory = GetComponentInParent<Inventory>();

        // TODO only display text if there is something on this field
        if (pipeType != null)
            UpdateText();
        
    }

    public void SetPipeType(PipeType type)
    {
        pipeType = type;
        //TODO UpdatePipeDisplay();     // pipeDisplay is an instance of the pipe type, to display on asset-tile
    }

    public void SetCount (int count)
    {
        this.pipeCount = count;
        UpdateText();
    }


    private void UpdateText()
    {
        if (textObj != null)
        {
            textObj.text = pipeCount + "x";
        }
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
