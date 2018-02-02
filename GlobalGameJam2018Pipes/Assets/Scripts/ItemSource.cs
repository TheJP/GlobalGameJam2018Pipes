using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
    [SerializeField] private bool spawnPositionFree = true;
    private bool itemReleased = false;

    [SerializeField] private List<Material> availableMaterials;
    [SerializeField] private List<MaterialColor> availableColors;

    // Use this for initialization
    void Start()
    {
        //random = new System.Random();
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
            newItem.GetComponent<ItemBehaviour>().StartMoving(FlowDirection.ToRight);
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
            case Material.Powder:
                return Instantiate(powderContainerPrefab);
            case Material.Vapor:
                return Instantiate(vaporContainerPrefab);
            case Material.Paste:
                return Instantiate(pasteContainerPrefab);
            default:
                Debug.Log("Can't Create Container. Unknown Material: " + material);
                return null;
        }
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            if (spawnPositionFree)
            {
                var material = availableMaterials[UnityEngine.Random.Range(0, availableMaterials.Count)];
                var materialColor = availableColors[UnityEngine.Random.Range(0, availableColors.Count)];

                newItem = CreateItem(new ColoredMaterial(material, materialColor), transform.position, Row, Column).gameObject;

                spawnPositionFree = false;
                itemReleased = false;
            }
            yield return new WaitForSecondsRealtime(spawnTime);
        }
    }

    public ItemBehaviour CreateItem(ColoredMaterial coloredMaterial, Vector3 position, int row, int column)
    {
        var item = CreateContainer(coloredMaterial.Material);
        item.transform.position = new Vector3(position.x, 5, position.z);
        var itemBehaviour = item.GetComponent<ItemBehaviour>();
        itemBehaviour.Row = row;
        itemBehaviour.Column = column;
        itemBehaviour.material = coloredMaterial;
        return itemBehaviour;
    }
}
