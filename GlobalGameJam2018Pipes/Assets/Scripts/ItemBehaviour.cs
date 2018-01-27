using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{

    public int floatSpeed;
    public int Row { get; set; }
    public int Column { get; set; }
    private PlayBoard playBoard;

    // Use this for initialization
    void Start()
    {
        GameObject playBoardObject = GameObject.Find("PlayBoard");
        playBoard = playBoardObject.GetComponent<PlayBoard>();

        Row = 0;
        Column = 0;

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
            switch (playBoard.GetTileForPosition(Row, Column).FlowDirection)
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
}
