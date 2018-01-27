
/// <summary>
/// Direction an ingredient will flow.
/// Left is -x and right is +x.
/// Up is +z and down is -z.
/// </summary>
public enum FlowDirection
{
    ToLeft,
    ToDown,
    ToRight,
    ToTop,
    Stop,
    Trash,
    Drop
}