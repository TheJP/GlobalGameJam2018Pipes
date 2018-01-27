using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSource : MonoBehaviour
{
    public int spawnTime;
    public List<GameObject> itemPrefabs;

    private System.Random random;
    private bool spawnPositionFree = true;

    // Use this for initialization
    void Start()
    {
        random = new System.Random();
        StartCoroutine(SpawnItem());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            if (spawnPositionFree)
            {
                Instantiate(itemPrefabs[random.Next(0, itemPrefabs.Count)]);
            }
            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }
}
