using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public int floatSpeed;
    [SerializeField] public int Row;
    [SerializeField] public int Column;
    private LastStep lastStep;
    private PlayBoard playBoard;

    // Use this for initialization
    void Start()
    {
        GameObject playBoardObject = GameObject.Find("PlayBoard(Clone)");
        playBoard = playBoardObject.GetComponent<PlayBoard>();

        StartCoroutine(MoveItem());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.name.ToString() + ": LastStep " + lastStep + ", Row/Column " + Row + "/" + Column);
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
                        lastStep = LastStep.UP;
                        StepUp();
                        break;
                    case FlowDirection.ToDown:
                        lastStep = LastStep.DOWN;
                        StepDown();
                        break;
                    case FlowDirection.ToLeft:
                        lastStep = LastStep.LEFT;
                        StepLeft();
                        break;
                    case FlowDirection.ToRight:
                        lastStep = LastStep.RIGHT;
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
    }

    public void StepLeft()
    {
        transform.position = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z);
        Column--;
    }

    public void StepUp()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 10);
        Row++;
    }

    public void StepDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 10);
        Row--;
    }

    private enum LastStep { LEFT, RIGHT, UP, DOWN }
}
