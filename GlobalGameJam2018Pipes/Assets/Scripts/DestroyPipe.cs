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

    public bool ReduceLifetime()
    {
        //Debug.Log("Reduce Lifetime called");
        timeSpentToDestroy += Time.deltaTime;

        if (timeSpentToDestroy >= timeToDestroy)
        {
            timeSpentToDestroy = 0;
            tile.RemovePipe();
            return true;
        }

        return false;
    }
}
