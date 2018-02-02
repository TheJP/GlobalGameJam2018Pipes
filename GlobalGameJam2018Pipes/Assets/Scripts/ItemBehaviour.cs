using System;
using System.Collections;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public int floatSpeed;
    [SerializeField] public int Row;
    [SerializeField] public int Column;
    private LastStep lastStep;
    private PlayBoard playBoard;
    private bool isMoving;

    private AudioSource audioSource;

    public ColoredMaterial material;
    public AudioClip trashClip;
    public AudioClip dropClip;


    // Use this for initialization
    void Start()
    {
        SetMoving(false);

        GameObject playBoardObject = GameObject.Find("PlayBoard(Clone)");
        playBoard = playBoardObject.GetComponent<PlayBoard>();

        lastStep = LastStep.RIGHT;

        var particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            var main = particleSystem.main;
            main.startColor = MixerScript.ConvertMaterialColor(material.Color);
        }
        else
        {
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color = MixerScript.ConvertMaterialColor(material.Color);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetMoving(bool isMoving)
    {
        if (this.isMoving)
        {
            if (!isMoving)
            {
                // we stop moving - stop playing sound
                audioSource.Pause();
            }
        }
        else
        {
            if (isMoving)
            {
                // we start moving - start to play sound
                audioSource.Play();
            }
        }
        this.isMoving = isMoving;
    }

    private IEnumerator MoveItem()
    {
        yield return new WaitForSecondsRealtime(floatSpeed);

        while (true)
        {
            FlowDirection nextDirection = FlowDirection.Drop;
            Tile nextTile = playBoard.GetTileForPosition(Column, Row);
            //Debug.Log(this.name.ToString() + ": Next Tile " + nextTile + ", Row/Column " + Row + "/" + Column);
            if (nextTile != null)
            {
                if (nextTile.pipe == null)
                {
                    nextDirection = FlowDirection.Drop;
                    SwitchSound(dropClip);
                }
                else
                {
                    switch (lastStep)
                    {
                        case LastStep.DOWN:
                            nextDirection = nextTile.pipe.FromTop;
                            break;
                        case LastStep.LEFT:
                            nextDirection = nextTile.pipe.FromRight;
                            break;
                        case LastStep.RIGHT:
                            nextDirection = nextTile.pipe.FromLeft;
                            break;
                        case LastStep.UP:
                            nextDirection = nextTile.pipe.FromBottom;
                            break;
                        default:
                            break;
                    }
                    if (nextDirection == FlowDirection.Trash)
                    {
                        AudioSource audioSource = nextTile.pipe.gameObject.GetComponent<AudioSource>();
                        audioSource.clip = trashClip;
                        audioSource.volume = 0.8f;
                        audioSource.Play();
                        // switchsound cannot be done later, because we destroy the gameobject then
                    }
                }
            }

            //Debug.Log(this.name.ToString() + ": Next Direction " + nextDirection);
            var foundSink = false;
            switch (nextDirection)
            {
                case FlowDirection.ToTop:
                    foundSink = StepUp();
                    break;
                case FlowDirection.ToDown:
                    foundSink = StepDown();
                    break;
                case FlowDirection.ToLeft:
                    foundSink = StepLeft();
                    break;
                case FlowDirection.ToRight:
                    foundSink = StepRight();
                    break;
                case FlowDirection.Stop:
                    //Debug.Log("Item stopping");
                    var mixerPipe = nextTile?.GetComponentInChildren<MixerPipe>();
                    if (mixerPipe != null)
                    {
                        mixerPipe.ProcessItem(this);
                        SetMoving(false);
                        yield break;
                    }
                    break;
                case FlowDirection.Trash:
                    Destroy(gameObject);
                    yield break;
                case FlowDirection.Drop:
                    if (nextTile != null)
                    {
                        nextTile.Block(this);
                        // SwitchSound(dropClip);
                        // sound setting has been done when setting nextDirection = FlowDirection.Drop;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    yield break;
                default:
                    Debug.Log("No Direction to Move");
                    break;
            }

            if (foundSink)
            {
                SetMoving(false);
                yield break;
            }

            yield return new WaitForSecondsRealtime(floatSpeed);
        }
    }

    private void SwitchSound(AudioClip clip)
    {
        audioSource.loop = false;
        audioSource.Stop();
        Debug.Log("switch sound to " + clip.name);
        audioSource.clip = clip;
        audioSource.volume = 0.8f;
        audioSource.Play();
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        float timePassed = 0;
        var origin = transform.position;

        do
        {
            timePassed += Time.deltaTime;
            transform.position = Vector3.Lerp(origin, target, timePassed / floatSpeed);
            
            yield return null;
        }
        while(timePassed < floatSpeed);
    }

    public bool StepRight()
    {
        StartCoroutine(MoveTo(new Vector3(transform.position.x + 10, transform.position.y, transform.position.z)));
        Column++;
        lastStep = LastStep.RIGHT;

        return ContinueMoving();
    }

    public bool StepLeft()
    {
        StartCoroutine(MoveTo(new Vector3(transform.position.x - 10, transform.position.y, transform.position.z)));
        Column--;
        lastStep = LastStep.LEFT;

        return ContinueMoving();
    }

    public bool StepUp()
    {
        StartCoroutine(MoveTo(new Vector3(transform.position.x, transform.position.y, transform.position.z + 10)));
        Row++;
        lastStep = LastStep.UP;
        
        return ContinueMoving();
    }

    public bool StepDown()
    {
        StartCoroutine(MoveTo(new Vector3(transform.position.x, transform.position.y, transform.position.z - 10)));
        Row--;
        lastStep = LastStep.DOWN;

        return ContinueMoving();
    }

    private bool ContinueMoving()
    {
        if (isMoving)
        {
            return FindSink();
        }

        if (!FindSink())
        {
            if (!isMoving)
            {
                SetMoving(true);
                StartCoroutine(MoveItem());
            }

            return false;
        }

        return true;
    }

    private bool FindSink()
    {
        foreach (var itemSink in playBoard.itemSinks)
        {
            if (itemSink.row == Row && itemSink.column == Column)
            {
                itemSink.ProcessSinkItem(this);
                return true;
            }
        }

        return false;
    }

    private enum LastStep { LEFT, RIGHT, UP, DOWN }
}
