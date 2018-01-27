using UnityEngine;

public class Inventory : MonoBehaviour
{
  [SerializeField] private int pipeStraigthCount;

  [SerializeField] private int pipeTurnCount;

  [SerializeField] private int pipeLeftRightCount;

  [SerializeField] private int pipeOverUnderCount;

  [SerializeField] private int pipeTIntersectionCount;

  [SerializeField] private int pipeXIntersectionCount;

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
}
