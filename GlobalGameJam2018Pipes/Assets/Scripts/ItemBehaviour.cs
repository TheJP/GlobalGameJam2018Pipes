using System.Collections;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    private static Color ConvertMaterialColor(MaterialColor materialColor)
    {
        switch (materialColor)
        {
        case MaterialColor.Red:
            return new Color(1, 0, 0);
        case MaterialColor.Yellow:
            return new Color(0, 1, 1);
        case MaterialColor.Blue:
            return new Color(0, 0, 1);
        case MaterialColor.Green:
            return new Color(0, 1, 0);
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

    public ColoredMaterial material;

    // Use this for initialization
    void Start()
    {
        GameObject playBoardObject = GameObject.Find("PlayBoard(Clone)");
        playBoard = playBoardObject.GetComponent<PlayBoard>();

        lastStep = LastStep.RIGHT;

        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = ConvertMaterialColor(material.Color);

        StartCoroutine(MoveItem());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator MoveItem()
    {
        while (true)
        {
            FlowDirection nextDirection = FlowDirection.Stop;
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
            if (nextDirection != null)
            {
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
                        break;
                    case FlowDirection.Trash:
                        //TODO
                        break;
                    case FlowDirection.Drop:
                        //TODO
                        //Debug.Log("Item dropped");
                        break;
                    default:
                        Debug.Log("No Direction to Move");
                        break;
                }

                if (foundSink)
                {
                    yield break;
                }
            }

            yield return new WaitForSecondsRealtime(floatSpeed);
        }
    }

    public bool StepRight()
    {
        transform.position = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
        Column++;
        lastStep = LastStep.RIGHT;
        return FindSink();
    }

    public bool StepLeft()
    {
        transform.position = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z);
        Column--;
        lastStep = LastStep.LEFT;
        return FindSink();
    }

    public bool StepUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        Row++;
        lastStep = LastStep.UP;
        return FindSink();
    }

    public bool StepDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
        Row--;
        lastStep = LastStep.DOWN;
        return FindSink();
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
