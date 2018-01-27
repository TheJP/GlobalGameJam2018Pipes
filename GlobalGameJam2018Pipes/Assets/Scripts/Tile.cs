using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Column;

    public GameObject pipeStraight;
    public GameObject pipeTurn;
    public GameObject pipeLeftRight;
    public GameObject pipeOverUnder;
    public GameObject pipeXIntersection;
    public GameObject pipeTIntersection;

    public bool hasPipe;
    
    public bool BuildPipe(PipeType pipeType)
    {
        if(hasPipe)
        {
            return false;
        }

        switch(pipeType)
        {
        case PipeType.Straight:
            Instantiate(pipeStraight, transform);
            return hasPipe = true;
        case PipeType.Turn:
            Instantiate(pipeTurn, transform);
            return hasPipe = true;
        case PipeType.LeftRight:
            Instantiate(pipeLeftRight, transform);
            return hasPipe = true;
        case PipeType.UnderOver:
            Instantiate(pipeOverUnder, transform);
            return hasPipe = true;
        case PipeType.TIntersection:
            Instantiate(pipeTIntersection, transform);
            return hasPipe = true;
        case PipeType.XIntersection:
            Instantiate(pipeXIntersection, transform);
            return hasPipe = true;
        default:
            return false;
        }
    }

    public void RemovePipe()
    {
        hasPipe = false;
    }

    public void Test()
    {
        Debug.Log("Test - Row: " + Row + ", Column: " + Column);
    }
}
