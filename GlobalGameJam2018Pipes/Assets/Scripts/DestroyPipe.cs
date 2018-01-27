﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPipe : MonoBehaviour
{
    public float timeToDestroy;
    private float timeSpentToDestroy;

    private Tile tile;

    // Use this for initialization
    void Start()
    {
        tile = GetComponentInParent<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ReduceLifetime()
    {
        Debug.Log("Reduce Lifetime called");
        timeSpentToDestroy += Time.deltaTime;

        if (timeSpentToDestroy >= timeToDestroy)
        {
            tile.RemovePipe();
            Destroy(this.transform.parent.gameObject);
        }
    }
}