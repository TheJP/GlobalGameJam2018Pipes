using System.Collections;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    private static Color ConvertMaterialColor(MaterialColor materialColor)
    {
        switch (materialColor)
        {
        case MaterialColor.Red:
            return Color.red;
        case MaterialColor.Yellow:
            return Color.yellow;
        case MaterialColor.Blue:
            return Color.blue;
        case MaterialColor.Green:
            return Color.green;
        case MaterialColor.Orange:
            return new Color(1, 0xa0 / 255.0f, 0);
        case MaterialColor.Violet:
            return new Color(1, 0, 1);
        case MaterialColor.Black:
            return Color.black;
        default:
            return Color.magenta;
        }
    }

    public int floatSpeed;
    [SerializeField] public int Row;
    [SerializeField] public int Column;
    private LastStep lastStep;
    private PlayBoard playBoard;
    private bool isMoving;

    private AudioSource audioSource;
    private AudioClip audioClip;

    public ColoredMaterial material;

    // Use this for initialization
    void Start()
    {
        SetMoving(false);

        GameObject playBoardObject = GameObject.Find("PlayBoard(Clone)");
        playBoard = playBoardObject.GetComponent<PlayBoard>();

        lastStep = LastStep.RIGHT;

        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = ConvertMaterialColor(material.Color);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
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
                Debug.Log("stop sound");
                audioSource.Pause();
            }
        }
        else
        {
            if (isMoving)
            {
                // we start moving - start to play sound
                Debug.Log("start sound");
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

    public bool StepRight()
    {
        transform.position = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
        Column++;
        lastStep = LastStep.RIGHT;

        return ContinueMoving();
    }

    public bool StepLeft()
    {
        transform.position = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z);
        Column--;
        lastStep = LastStep.LEFT;

        return ContinueMoving();
    }

    public bool StepUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        Row++;
        lastStep = LastStep.UP;
        
        return ContinueMoving();
    }

    public bool StepDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
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
