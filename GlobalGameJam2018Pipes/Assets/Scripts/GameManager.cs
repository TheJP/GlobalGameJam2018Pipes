using GlobalGameJam2018Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Multiplayer Multiplayer;
    public static Options options;

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
    public List<GameObject> itemSources = new List<GameObject>();
    private AudioSource audioSource;
    private bool deletingPipe = false;
    private float coolDownDeletingPipe = 0;
    private float thresholdDeletingPipe = 0;

    private bool holdsHammer;

    private TableScript tableScript;
   

    private void Awake()
    {
        if (options == null)
            options = new Options();
        
        Instantiate(roomPrefab);

        inventory = Instantiate(inventoryPrefab).GetComponent<Inventory>();
        cursor = Instantiate(cursorPrefab).GetComponent<Cursor>();
        Instantiate(cameraPrefab);
        var playBoard = Instantiate(playBoardPrefab).GetComponent<PlayBoard>();
        GameObject table = Instantiate(assetTablePrefab);
        table.transform.rotation = Quaternion.Euler(0, 90, 0);
        table.transform.position = new Vector3(50, 6, 0);

        for (int sourceCount = 0; sourceCount < 3; sourceCount++)
        {
            GameObject itemSourceObject = Instantiate(itemSourcePrefab);
            itemSourceObject.transform.position = new Vector3(-45, 2.35F, 25 - sourceCount * 10);
            itemSourceObject.GetComponent<ItemSource>().Row = 6 - sourceCount;
            itemSources.Add(itemSourceObject);
        }

        audioSource = GetComponent<AudioSource>();

        var sinkX = playBoard.GetXPosition(playBoard.boardSize);
        var sinkTopZ = playBoard.GetZPosition(0);
        var sinkBottomZ = playBoard.GetZPosition(playBoard.boardSize - 1);

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
        else
        {
            foreach (ItemSink sink in playBoard.itemSinks)
            {
                sink.SinglePlayer = true;
            }
        }

        tableScript = table.GetComponent<TableScript>();
        tableScript.SetInventory(inventory);

    }

    public void Start()
    {
        SetBuildNext(PipeType.None);

        // uncomment this to create files about probabilites of mixed materials
        // MixCounter.WriteMixOccurrenceTables();

        //tableScript.InitInventoryPlaces();
    }


    public bool SetBuildNext(PipeType pipeType)
    {
        cursor.ShowPipe(pipeType);

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
                    tableScript.ResetSelectionColors();
                    var asset = target.GetComponent<Asset>();
                    SetBuildNext(asset.PipeType);
                    target.GetComponent<Renderer>().material.color = Color.blue;

                    if (asset.DisplaysHammer)
                    {
                        cursor.ShowHammer();
                        holdsHammer = true;
                    }
                    else
                    {
                        holdsHammer = false;
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        {
                            BuyPipe(asset.PipeType);
                        }
                    }
                }

                if (target.tag == "ItemSource")
                {
                    //Debug.Log("ItemSource clicked");
                    target.gameObject.GetComponentInParent<ItemSource>().ReleasItem();
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

                    if (holdsHammer)
                    {
                        thresholdDeletingPipe++;
                        if (thresholdDeletingPipe >= 50)
                        {
                            if (!deletingPipe)
                            {
                                deletingPipe = true;
                                SoundStart();
                            }
                        }

                        var pipe = target.GetComponentInParent<Pipe>();
                        if (target.GetComponent<DestroyPipe>().ReduceLifetime())
                        {
                            audioSource.Stop();
                            inventory.Increase(pipe.Type);
                        }
                    }
                }

                if (target.name.Contains("Tile") && !deletingPipe)
                {
                    //Debug.Log("Hit: " + hit.collider.gameObject.name);

                    if (inventory.HasInventory(buildNext) && target.GetComponentInParent<Tile>()
                            .BuildPipe(buildNext, cursor.PipeRotation))
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
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            string sceneName = "Options";
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.IsValid())
            {
                Debug.Log($"will set scene active ({sceneName})");
                SceneManager.SetActiveScene(scene);
            }                
            else
            {
                //Debug.Log($"will load scene in coroutine ({sceneName})");
                StartCoroutine(LoadScene(sceneName));
            }
        }

        if (GameManager.Multiplayer != null)
        {
            GameManager.Multiplayer.DispatchEvents();
        }
    }

    
    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
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
