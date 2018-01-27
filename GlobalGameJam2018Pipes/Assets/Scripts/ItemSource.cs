using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSource : MonoBehaviour
{
    public int spawnTime;
    public List<GameObject> itemPrefabs;
    public int Column = -1;
    public int Row;

    private GameObject newItem;
    private System.Random random;
    [SerializeField] private bool spawnPositionFree = true;
    private bool itemReleased = false;

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

    public void ReleasItem()
    {
        if (!itemReleased)
        {
            itemReleased = true;
            spawnPositionFree = true;
            newItem.GetComponent<ItemBehaviour>().StepRight();
        }
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            if (spawnPositionFree)
            {
                spawnPositionFree = false;
                newItem = Instantiate(itemPrefabs[random.Next(0, itemPrefabs.Count)]);
                itemReleased = false;
                newItem.transform.position = transform.position;
                newItem.GetComponent<ItemBehaviour>().Row = Row;
                newItem.GetComponent<ItemBehaviour>().Column = Column;
            }
            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }
}
