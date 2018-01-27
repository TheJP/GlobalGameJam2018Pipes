using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSink : MonoBehaviour
{

    public bool singlePlayer = true;

    public void ProcessSinkItem(GameObject item)
    {
        if (singlePlayer)
        {
            Destroy(item);
        }
        else
        {
            //Do Network Stuff
        }

    }
}
