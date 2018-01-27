using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] private FlowDirection fromLeft;

    [SerializeField] private FlowDirection fromTop;

    [SerializeField] private FlowDirection fromRight;

    [SerializeField] private FlowDirection fromBottom;

    [SerializeField] private int rotation;

    public int Rotation
    {
        get { return rotation; }
        set { rotation = value % 4; }
    }

    public FlowDirection FromLeft => GetRotatedFlowDirection(fromLeft, fromTop, fromRight, fromBottom);

    public FlowDirection FromTop => GetRotatedFlowDirection(fromTop, fromRight, fromBottom, fromLeft);

    public FlowDirection FromRight => GetRotatedFlowDirection(fromRight, fromBottom, fromLeft, fromTop);

    public FlowDirection FromBottom => GetRotatedFlowDirection(fromBottom, fromLeft, fromTop, fromRight);
    
    private FlowDirection GetRotatedFlowDirection(
        FlowDirection one,
        FlowDirection two,
        FlowDirection three,
        FlowDirection four)
    {
        switch (rotation)
        {
        default:
        case 0:
            return one;
        case 1:
            return two;
        case 2:
            return three;
        case 3:
            return four;
        }
    }
}
