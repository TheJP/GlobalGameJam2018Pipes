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
    public GameObject pipeXIntersection;
    public GameObject pipeTIntersection;


    private GameObject assetObj;
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
        Debug.Log("setPipeType");
        pipeType = type;
        UpdatePipeDisplay();     // pipeDisplay is an instance of the pipe type, to display on asset-tile
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

    private void UpdatePipeDisplay()
    {
        if (this.pipeType != null)
        {
            assetObj = CreatePipe(this.pipeType);
        }
        Debug.Log("in UpdatePipeDisplay end");
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

    public GameObject CreatePipe(PipeType? pipeType)
    {
        var pipeRotation = Quaternion.AngleAxis(90, Vector3.up);

        Debug.Log("in CreatePipe");

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
            case PipeType.TIntersection:
                pipeGameObject = Instantiate(pipeTIntersection, transform.position, pipeRotation, transform);
                break;
            case PipeType.XIntersection:
                pipeGameObject = Instantiate(pipeXIntersection, transform.position, pipeRotation, transform);
                break;
            default:
                pipeGameObject = null;
                break;
        }
        // make smaller to display on table
        pipeGameObject.transform.localScale = new Vector3(0.05f, 0.05f * 8, 0.05f);     // parent is scaled 8 in x and z, but only 1 in y, that's why * 8 ...
        pipeGameObject.transform.Translate(new Vector3(0, .35f, 0));
        
        Debug.Log("in CreatePipe end, have pipeObj: " + pipeGameObject.name);
        return pipeGameObject;

        //if (pipeGameObject != null)
        //{
        //    pipe = pipeGameObject.GetComponent<Pipe>();
        //    pipe.Rotation = rotation;
        //    return true;
        //}
    }


    // Update is called once per frame
    void Update ()
    {
        
    }
}
