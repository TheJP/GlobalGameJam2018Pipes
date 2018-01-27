using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject inventoryPrefab;
    public GameObject cursorPrefab;
    public GameObject cameraPrefab;
    public GameObject playBoardPrefab;
    public GameObject assetTablePrefab;
    public GameObject itemSourcePrefab;
    public GameObject itemSinkPrefab;

    [SerializeField] private PipeType buildNext;
    private Inventory inventory;
    private Cursor cursor;
    private ItemSource itemSource;
    private TableScript tableScript;

    public int numGold;
    public int numPipeStraight;
    public int numPipeCurves;

    // TODO previousBuildNext can be removed when we select pipes via the table
    private PipeType previousBuildNext;

    private void Awake()
    {
        inventory = Instantiate(inventoryPrefab).GetComponent<Inventory>();
        cursor = Instantiate(cursorPrefab).GetComponent<Cursor>();
        Instantiate(cameraPrefab);
        Instantiate(playBoardPrefab);
        GameObject table = Instantiate(assetTablePrefab);
        table.transform.position = new Vector3(50, 0, 0);
        //table.transform.rotation = Quaternion.Euler(0, 90, 0);

        GameObject itemSourceObject = Instantiate(itemSourcePrefab);
        itemSourceObject.transform.position = new Vector3(-45, 0, 25);
        itemSource = itemSourceObject.GetComponent<ItemSource>();

        tableScript = table.GetComponent<TableScript>();
        tableScript.SetInventory(inventory);

    }

    public void Start()
    {
        SetBuildNext(PipeType.Straight);
        
        //tableScript.InitInventoryPlaces();
    }


    // TODO just for testing, needs different implementation
    private void InitInventory()
    {
        Debug.Log("in initInventory");

        //inventory.Gold = this.numGold;
        //inventory.PipeStraightCount = this.numPipeStraight;
        //inventory.PipeTurnCount = this.numPipeCurves;

        // Zuteilung welcher Typ wo dargestellt wird, ist in TableScript.InitInventoryPlaces
    }


    public bool SetBuildNext(PipeType pipeType)
    {
        cursor.SetPipeDisplay(pipeType);

        if (inventory.HasInventory(pipeType))
        {
            buildNext = pipeType;
            return true;
        }

        return false;
    }

    void Update()
    {
        // TODO This entire if block can be removed later, when we can select the pipe on the table
        if (buildNext != previousBuildNext)
        {
            previousBuildNext = buildNext;
            SetBuildNext(buildNext);
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float range = 1000.0F;

            Debug.DrawRay(transform.position, (Input.mousePosition), Color.green);

            if (Physics.Raycast(ray, out hit, range))
            {
                GameObject target = hit.collider.gameObject;

                if (target.tag == "Pipe")
                {
                    Debug.Log("Hit a Pipe");
                    target.GetComponent<DestroyPipe>().ReduceLifetime();
                }

                if (target.name.Contains("Tile"))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);

                    if (inventory.HasInventory(buildNext) && target.GetComponentInParent<Tile>().BuildPipe(buildNext, cursor.currentRotation))
                    {
                        inventory.Reduce(buildNext);
                    }
                }

                if (target.name.Contains("Asset"))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                }

                if (target.tag == "ItemSource")
                {
                    Debug.Log("ItemSource clicked");
                    itemSource.ReleasItem();
                }

            }
        }
    }

}
