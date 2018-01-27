using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int gold;

    [SerializeField] private int pipeStraigthCount;

    [SerializeField] private int pipeTurnCount;

    [SerializeField] private int pipeLeftRightCount;

    [SerializeField] private int pipeOverUnderCount;

    [SerializeField] private int pipeTIntersectionCount;

    [SerializeField] private int pipeXIntersectionCount;
    
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public int PipeStraightCount
    {
        get { return pipeStraigthCount; }
        set { pipeStraigthCount = value; }
    }

    public int PipeTurnCount
    {
        get { return pipeTurnCount; }
        set { pipeTurnCount = value; }
    }

    public int PipeLeftRightCount
    {
        get { return pipeLeftRightCount; }
        set { pipeLeftRightCount = value; }
    }

    public int PipeOverUnderCount
    {
        get { return pipeOverUnderCount; }
        set { pipeOverUnderCount = value; }
    }

    public int PipeTIntersectionCount
    {
        get { return pipeTIntersectionCount; }
        set { pipeTIntersectionCount = value; }
    }

    public int PipeXIntersectionCount
    {
        get { return pipeXIntersectionCount; }
        set { pipeXIntersectionCount = value; }
    }

    public bool HasInventory(PipeType pipeType)
    {
        switch(pipeType)
        {
        case PipeType.Straight:
            return pipeStraigthCount > 0;
        case PipeType.Turn:
            return pipeTurnCount > 0;
        case PipeType.LeftRight:
            return pipeLeftRightCount > 0;
        case PipeType.UnderOver:
            return pipeOverUnderCount > 0;
        case PipeType.TIntersection:
            return pipeTIntersectionCount > 0;
        case PipeType.XIntersection:
            return pipeXIntersectionCount > 0;
        default:
            return false;
        }
    }

    public void Reduce(PipeType pipeType)
    {
        switch(pipeType)
        {
        case PipeType.Straight:
            --pipeStraigthCount;
            break;
        case PipeType.Turn:
            --pipeTurnCount;
            break;
        case PipeType.LeftRight:
            --pipeLeftRightCount;
            break;
        case PipeType.UnderOver:
            --pipeOverUnderCount;
            break;
        case PipeType.TIntersection:
            --pipeTIntersectionCount;
            break;
        case PipeType.XIntersection:
            --pipeXIntersectionCount;
            break;
        }
    }

    public void Increase(PipeType pipeType)
    {
        switch(pipeType)
        {
        case PipeType.Straight:
            ++pipeStraigthCount;
            break;
        case PipeType.Turn:
            ++pipeTurnCount;
            break;
        case PipeType.LeftRight:
            ++pipeLeftRightCount;
            break;
        case PipeType.UnderOver:
            ++pipeOverUnderCount;
            break;
        case PipeType.TIntersection:
            ++pipeTIntersectionCount;
            break;
        case PipeType.XIntersection:
            ++pipeXIntersectionCount;
            break;
        }
    }
}
