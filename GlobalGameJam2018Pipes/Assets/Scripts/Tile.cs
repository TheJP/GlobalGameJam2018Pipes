using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Column;

    private List<GameObject> pipes = new List<GameObject>(2);

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Test()
    {
        Debug.Log("Test - Row: " + Row + ", Column: " + Column);
    }
}
