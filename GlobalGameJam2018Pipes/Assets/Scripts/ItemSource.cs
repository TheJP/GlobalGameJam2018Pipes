using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSource : MonoBehaviour
{
    public int spawnTime;

    public GameObject fluidContainerPrefab;
    public GameObject vaporContainerPrefab;
    public GameObject powderContainerPrefab;
    public GameObject herbsContainerPrefab;
    public GameObject pasteContainerPrefab;
    
    public int Column = -1;
    public int Row;

    private GameObject newItem;
    private System.Random random;
    [SerializeField] private bool spawnPositionFree = true;
    private bool itemReleased = false;

    [SerializeField] private List<Material> availableMaterials;
    [SerializeField] private List<MaterialColor> availableColors;

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

    private GameObject CreateContainer(Material material)
    {
        switch (material)
        {
        case Material.Fluid:
            return Instantiate(fluidContainerPrefab);
        case Material.Herbs:
            return Instantiate(herbsContainerPrefab);
        default:
        case Material.Paste:
            return Instantiate(pasteContainerPrefab);
        case Material.Powder:
            return Instantiate(powderContainerPrefab);
        case Material.Vapor:
            return Instantiate(vaporContainerPrefab);
        }
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            if (spawnPositionFree)
            {
                var material = availableMaterials[random.Next(0, availableMaterials.Count)];
                var materialColor = availableColors[random.Next(0, availableColors.Count)];

                spawnPositionFree = false;
                newItem = CreateContainer(material);
                itemReleased = false;
                newItem.transform.position = transform.position;
                var itemBehaviour = newItem.GetComponent<ItemBehaviour>();
                itemBehaviour.Row = Row;
                itemBehaviour.Column = Column;
                itemBehaviour.material = new ColoredMaterial(material, materialColor);
            }
            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }
}
