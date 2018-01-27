using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPipe : MonoBehaviour
{

    public float timeToDestroy;
    private float timeSpentToDestroy;

    // Use this for initialization
    void Start()
    {

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
            Destroy(this.transform.parent.gameObject);
        }
    }
}
