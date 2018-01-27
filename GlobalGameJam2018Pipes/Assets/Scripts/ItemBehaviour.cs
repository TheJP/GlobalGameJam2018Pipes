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
        //Debug.Log(this.name.ToString() + ": LastStep " + lastStep + ", Row/Column " + Row + "/" + Column);
    }

    private IEnumerator MoveItem()
    {
        while (true)
        {
            FlowDirection nextDirection = FlowDirection.Stop;
            Tile nextTile = playBoard.GetTileForPosition(Column, Row);
            Debug.Log(this.name.ToString() + ": Next Tile " + nextTile + ", Row/Column " + Row + "/" + Column);
            if (nextTile != null)
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

            Debug.Log(this.name.ToString() + ": Next Direction " + nextDirection);
            if (nextDirection != null)
            {
                switch (nextDirection)
                {
                    case FlowDirection.ToTop:
                        StepUp();
                        break;
                    case FlowDirection.ToDown:
                        StepDown();
                        break;
                    case FlowDirection.ToLeft:
                        StepLeft();
                        break;
                    case FlowDirection.ToRight:
                        StepRight();
                        break;
                    case FlowDirection.Stop:
                        Debug.Log("Item stopping");
                        break;
                    case FlowDirection.Trash:
                        //TODO
                        break;
                    case FlowDirection.Drop:
                        //TODO
                        break;
                    default:
                        Debug.Log("No Direction to Move");
                        break;
                }
            }
            yield return new WaitForSecondsRealtime(floatSpeed);
        }
    }

    public void StepRight()
    {
        transform.position = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
        Column++;
        lastStep = LastStep.RIGHT;
    }

    public void StepLeft()
    {
        transform.position = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z);
        Column--;
        lastStep = LastStep.LEFT;
    }

    public void StepUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        Row++;
        lastStep = LastStep.UP;
    }

    public void StepDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
        Row--;
        lastStep = LastStep.DOWN;
    }

    private enum LastStep { LEFT, RIGHT, UP, DOWN }
}
