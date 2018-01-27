using UnityEngine;

public class TileDisplay : MonoBehaviour
{
    public int tileSize;

    // Use this for initialization
    void Start ()
	{
	    gameObject.transform.localScale = new Vector3(tileSize, 1, tileSize);
    }
}
