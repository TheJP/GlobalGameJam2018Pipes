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

    public GameObject pipeStraight;
    public GameObject pipeTurn;
    public GameObject pipeLeftRight;
    public GameObject pipeOverUnder;
    public GameObject pipeMixer;
    public GameObject pipeTrash;
    public GameObject goldPrefab;


    private GameObject assetObj;
    private Inventory inventory;
        
    /*[HideInInspector]*/
    public int itemCount;  // number of assets stored here
    public PipeType pipeType;
    public bool displaysGold;

    private Text textObj;


    // Use this for initialization
    void Start ()
    {
        gameObject.transform.localScale = new Vector3 (tileSizeX, 1, tileSizeZ);

        textObj = GetComponentInChildren<Text>();

        inventory = GetComponentInParent<Inventory>();

        // TODO only display text if there is something on this field
        if (pipeType != PipeType.None || displaysGold)
            UpdateText();
        
    }

    public void SetPipeType(PipeType type)
    {
        //Debug.Log("setPipeType");
        pipeType = type;
        UpdateItemDisplay();     // pipeDisplay is an instance of the pipe type, to display on asset-tile
    }

    public void SetGoldDisplay()
    {
        displaysGold = true;
        SetPipeType(PipeType.None);
    }


    public void SetCount (int count)
    {
        this.itemCount = count;
        UpdateText();
    }
    private void UpdateText()
    {
        if (textObj != null)
        {
            textObj.text = itemCount + "x";
        }
    }

    private void UpdateItemDisplay()
    {
        if (assetObj != null)
        {
            Destroy(assetObj);
        }

        if (this.pipeType != PipeType.None)
        {
            assetObj = CreatePipe(this.pipeType);
        }
        else if (displaysGold)
        {
            assetObj = CreateGold();
        }
        //Debug.Log("in UpdateItemDisplay end");
    }


    public void Test ()
    {
        Debug.Log ("Test - Row: " + Row + ", Column: " + Column);
    }

    /*
    public static Object Instantiate(Object original);
public static Object Instantiate(Object original, Transform parent);
public static Object Instantiate(Object original, Transform parent, bool instantiateInWorldSpace);
public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent); 
 *      */

    public GameObject CreatePipe(PipeType pipeType)
    {
        var pipeRotation = Quaternion.AngleAxis(90, Vector3.up);

        //Debug.Log("in CreatePipe");

        GameObject pipeGameObject;
        switch (pipeType)
        {
            case PipeType.Straight:
                pipeGameObject = Instantiate(pipeStraight, transform.position, pipeRotation, transform);
                break;
            case PipeType.Turn:
                pipeGameObject = Instantiate(pipeTurn, transform.position, pipeRotation, transform);
                break;
            case PipeType.LeftRight:
                pipeGameObject = Instantiate(pipeLeftRight, transform.position, pipeRotation, transform);
                break;
            case PipeType.UnderOver:
                pipeGameObject = Instantiate(pipeOverUnder, transform.position, pipeRotation, transform);
                break;
            case PipeType.Mixer:
                pipeGameObject = Instantiate(pipeMixer, transform.position, pipeRotation, transform);
                break;
            case PipeType.Trash:
                pipeGameObject = Instantiate(pipeTrash, transform.position, pipeRotation, transform);
                break;
            default:
                pipeGameObject = null;
                break;
        }

        if(pipeGameObject != null)
        {
            // make smaller to display on table
            pipeGameObject.transform.localScale = new Vector3(0.05f, 0.05f * 8, 0.05f);     // parent is scaled 8 in x and z, but only 1 in y, that's why * 8 ...
            pipeGameObject.transform.Translate(new Vector3(0, .35f, 0));

            var boxCollider = pipeGameObject.GetComponentInChildren<BoxCollider>();
            if(boxCollider != null)
            {
                boxCollider.enabled = false;
            }

            //Debug.Log("in CreatePipe end, have pipeObj: " + pipeGameObject.name);
        }

        return pipeGameObject;

        //if (pipeGameObject != null)
        //{
        //    pipe = pipeGameObject.GetComponent<Pipe>();
        //    pipe.Rotation = rotation;
        //    return true;
        //}
    }

    public GameObject CreateGold()
    {
        var goldObject = Instantiate(goldPrefab, transform.position, Quaternion.identity, transform);

        goldObject.transform.localScale = new Vector3(0.1f, 1.5f, 0.1f);
        goldObject.transform.Translate(new Vector3(0.0f, 1.0f, 0.15f));

        return goldObject;
    }


    // Update is called once per frame
    void Update ()
    {
        
    }
}
