using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public int floatSpeed;
    public int Row { get; set; }
    public int Column { get; set; }
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

    }

    private IEnumerator MoveItem()
    {
        while (true)
        {
            FlowDirection nextDirection = FlowDirection.Stop;
            switch (lastStep)
            {
                case LastStep.DOWN:
                    nextDirection = playBoard.GetTileForPosition(Row, Column).pipe.FromTop;
                    break;
                case LastStep.LEFT:
                    nextDirection = playBoard.GetTileForPosition(Row, Column).pipe.FromRight;
                    break;
                case LastStep.RIGHT:
                    nextDirection = playBoard.GetTileForPosition(Row, Column).pipe.FromLeft;
                    break;
                case LastStep.UP:
                    nextDirection = playBoard.GetTileForPosition(Row, Column).pipe.FromBottom;
                    break;
                default:
                    break;
            }

            if(nextDirection != null)
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
        transform.position = new Vector2(transform.position.x + 10, transform.position.y);
    }

    public void StepLeft()
    {
        transform.position = new Vector2(transform.position.x - 10, transform.position.y);
    }

    public void StepUp()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 10);
    }

    public void StepDown()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y - 10);
    }

    private enum LastStep { LEFT, RIGHT, UP, DOWN }
}
