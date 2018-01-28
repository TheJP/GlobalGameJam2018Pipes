using UnityEngine;

public class Pipe : MonoBehaviour
{
    private static FlowDirection GetRotatedFlowDirection(FlowDirection direction, int rotation)
    {
        while (rotation > 0)
        {
            switch (direction)
            {
            case FlowDirection.ToLeft:
                direction = FlowDirection.ToTop;
                break;
            case FlowDirection.ToTop:
                direction = FlowDirection.ToRight;
                break;
            case FlowDirection.ToRight:
                direction = FlowDirection.ToDown;
                break;
            case FlowDirection.ToDown:
                direction = FlowDirection.ToLeft;
                break;
            default:
                return direction;
            }

            --rotation;
        }

        return direction;
    }

    [SerializeField] private bool isAffectedByRotation;

    [SerializeField] private FlowDirection fromLeft;

    [SerializeField] private FlowDirection fromTop;

    [SerializeField] private FlowDirection fromRight;

    [SerializeField] private FlowDirection fromBottom;

    [SerializeField] private int rotation;

    [SerializeField] private PipeType pipeType;

    public int Rotation
    {
        get { return rotation; }
        set { rotation = value % 4; }
    }

    public PipeType Type => pipeType;

    public FlowDirection FromLeft => GetRotatedFlowDirection(fromLeft, fromBottom, fromRight, fromTop);

    public FlowDirection FromTop => GetRotatedFlowDirection(fromTop, fromLeft, fromBottom, fromRight);

    public FlowDirection FromRight => GetRotatedFlowDirection(fromRight, fromTop, fromLeft, fromBottom);

    public FlowDirection FromBottom => GetRotatedFlowDirection(fromBottom, fromRight, fromTop, fromLeft);
    
    private FlowDirection GetRotatedFlowDirection(
        FlowDirection one,
        FlowDirection two,
        FlowDirection three,
        FlowDirection four)
    {
        if (!isAffectedByRotation)
        {
            return one;
        }

        switch (rotation)
        {
        default:
        case 0:
            return GetRotatedFlowDirection(one, rotation);
        case 1:
            return GetRotatedFlowDirection(two, rotation);
        case 2:
            return GetRotatedFlowDirection(three, rotation);
        case 3:
            return GetRotatedFlowDirection(four, rotation);
        }
    }
}
