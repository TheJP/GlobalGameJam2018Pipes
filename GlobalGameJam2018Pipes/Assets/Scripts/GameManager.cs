using GlobalGameJam2018Networking;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Multiplayer Multiplayer;

    public GameObject inventoryPrefab;
    public GameObject cursorPrefab;
    public GameObject cameraPrefab;
    public GameObject playBoardPrefab;
    public GameObject assetTablePrefab;
    public GameObject itemSourcePrefab;
    public GameObject itemSinkPrefab;
    public GameObject roomPrefab;


    [SerializeField] private PipeType buildNext;

    public int priceStraightPipe;
    public int priceTurnPipe;
    public int priceLeftRightPipe;
    public int priceOverUnderPipe;
    public int priceMixerPipe;
    public int priceTrashPipe;


    private Inventory inventory;
    private Cursor cursor;
    private ItemSource itemSource;
    private AudioSource audioSource;
    private bool deletingPipe = false;
    private float coolDownDeletingPipe = 0;
    private float thresholdDeletingPipe = 0;

    private TableScript tableScript;

    private void Awake()
    {
        Instantiate(roomPrefab);

        inventory = Instantiate(inventoryPrefab).GetComponent<Inventory>();
        cursor = Instantiate(cursorPrefab).GetComponent<Cursor>();
        Instantiate(cameraPrefab);
        var playBoard = Instantiate(playBoardPrefab).GetComponent<PlayBoard>();
        GameObject table = Instantiate(assetTablePrefab);
        table.transform.rotation = Quaternion.Euler(0, 90, 0);
        table.transform.position = new Vector3(50, 6, 0);

        GameObject itemSourceObject = Instantiate(itemSourcePrefab);
        itemSourceObject.transform.position = new Vector3(-45, 2.35F, 25);
        itemSource = itemSourceObject.GetComponent<ItemSource>();
        audioSource = GetComponent<AudioSource>();

        var tileSize = playBoard.tilePrefab.GetComponentInChildren<TileDisplay>().tileSize;
        var sinkX = playBoard.GetXPosition(playBoard.boardSize, tileSize);
        var sinkTopZ = playBoard.GetZPosition(0, tileSize);
        var sinkBottomZ = playBoard.GetZPosition(playBoard.boardSize - 1, tileSize);

        playBoard.itemSinks = new List<ItemSink>()
        {
            Instantiate(itemSinkPrefab, new Vector3(sinkX, 0, sinkTopZ), Quaternion.identity).GetComponent<ItemSink>(),
            Instantiate(itemSinkPrefab, new Vector3(sinkX, 0, sinkBottomZ), Quaternion.identity).GetComponent<ItemSink>()
        };

        playBoard.itemSinks[0].row = 0;
        playBoard.itemSinks[0].column = playBoard.boardSize;
        playBoard.itemSinks[1].row = playBoard.boardSize - 1;
        playBoard.itemSinks[1].column = playBoard.boardSize;

        if (Multiplayer != null)
        {
            var levelConfig = LevelConfig.Builder("Main")
                .AddPipe(PipeDirection.ToAlchemist, 0)
                .AddPipe(PipeDirection.ToAlchemist, 1)
                .AddPipe(PipeDirection.ToPipes, 200)
                .Create();

            foreach (var remotePipe in levelConfig.Pipes)
            {
                if (remotePipe.Direction == PipeDirection.ToAlchemist)
                {
                    var sink = playBoard.itemSinks[remotePipe.Order];
                    sink.remotePipe = remotePipe;
                    sink.pipesNetwork = GameManager.Multiplayer.Network;
                }
            }

            Multiplayer.Network.ReceivedMoneyMaker += (maker, pipe) => { inventory.Gold += maker.GoldValue; };

            // Exit or continue game:
            Multiplayer.Network.AlchemistDisconnected += this.OnAlchemistDisconnected;
            Multiplayer.Network.GameOver += this.OnGameOver;

            // Display chat message:
            //Multiplayer.Network.ReceivedMessage

            Multiplayer.Network.StartLevel(levelConfig);
        }

        tableScript = table.GetComponent<TableScript>();
        tableScript.SetInventory(inventory);

    }

    public void Start()
    {
        SetBuildNext(PipeType.None);

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float range = 1000.0F;

            Debug.DrawRay(transform.position, (Input.mousePosition), Color.green);

            if (Physics.Raycast(ray, out hit, range))
            {
                GameObject target = hit.collider.gameObject;

                if (target.name.Contains("Asset"))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    tableScript.ResetSelectionColors();
                    var pipeType = target.GetComponent<Asset>().pipeType;
                    SetBuildNext(pipeType);
                    target.GetComponent<Renderer>().material.color = Color.blue;

                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        BuyPipe(pipeType);
                    }
                }

                if (target.tag == "ItemSource")
                {
                    //Debug.Log("ItemSource clicked");
                    itemSource.ReleasItem();
                }

            }
        }
        else if (Input.GetMouseButton(0))
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
                    //Debug.Log("Hit a Pipe");
                    var mixerPipe = target.GetComponentInParent<MixerPipe>();
                    if (mixerPipe != null)
                    {
                        mixerPipe.ReleaseItem();
                    }

                    thresholdDeletingPipe++;
                    if (thresholdDeletingPipe >= 50)
                    {
                        if (!deletingPipe)
                        {
                            deletingPipe = true;
                            SoundStart();
                        }
                    }

                    if (target.GetComponent<DestroyPipe>().ReduceLifetime())
                    {
                        audioSource.Stop();
                        inventory.Increase(target.GetComponentInParent<Pipe>().Type);
                    }
                }

                if (target.name.Contains("Tile") && !deletingPipe)
                {
                    //Debug.Log("Hit: " + hit.collider.gameObject.name);

                    if (inventory.HasInventory(buildNext) && target.GetComponentInParent<Tile>()
                            .BuildPipe(buildNext, cursor.currentRotation))
                    {
                        inventory.Reduce(buildNext);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            deletingPipe = false;
            thresholdDeletingPipe = 0;
        }

        if (GameManager.Multiplayer != null)
        {
            GameManager.Multiplayer.DispatchEvents();
        }
    }

    private void SoundStart()
    {
        audioSource.loop = true;
        audioSource.volume = 0.8f;
        audioSource.Play();
    }

    private void BuyPipe(PipeType pipeType)
    {
        int price;
        switch (pipeType)
        {
            case PipeType.Straight:
                price = priceStraightPipe;
                break;
            case PipeType.Turn:
                price = priceTurnPipe;
                break;
            case PipeType.LeftRight:
                price = priceLeftRightPipe;
                break;
            case PipeType.UnderOver:
                price = priceOverUnderPipe;
                break;
            case PipeType.Mixer:
                price = priceMixerPipe;
                break;
            case PipeType.Trash:
                price = priceTrashPipe;
                break;
            default:
                return;
        }

        if (inventory.Gold >= price)
        {
            inventory.Gold -= price;
            inventory.Increase(pipeType);
        }
    }

    private void OnAlchemistDisconnected()
    {
        OnGameOver(false);
    }

    private void OnGameOver(bool success)
    {
        if (GameManager.Multiplayer == null)
        {
            return;
        }

        Multiplayer.Network.AlchemistDisconnected -= this.OnAlchemistDisconnected;
        Multiplayer.Network.GameOver -= this.OnGameOver;
        Multiplayer.Network.Stop();

        Multiplayer = null;

        SceneManager.LoadScene("MainMenu");
    }
}
