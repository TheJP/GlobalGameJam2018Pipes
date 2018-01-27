using UnityEngine;

public class Cursor
    : MonoBehaviour
{
    public GameObject pipeStraight;
    public GameObject pipeTurn;
    public GameObject pipeLeftRight;
    public GameObject pipeOverUnder;
    public GameObject pipeXIntersection;
    public GameObject pipeTIntersection;

    private GameObject currrentDisplay;
    private PipeType currentPipeType;
    private Plane cursorPlane;

    public void SetPipeDisplay(PipeType pipeType)
    {
        if(currrentDisplay != null)
        {
            Destroy(currrentDisplay);
        }

        switch(pipeType)
        {
        case PipeType.Straight:
            currrentDisplay = Instantiate(pipeStraight, transform);
            break;
        case PipeType.Turn:
            currrentDisplay = Instantiate(pipeTurn, transform);
            break;
        case PipeType.LeftRight:
            currrentDisplay = Instantiate(pipeLeftRight, transform);
            break;
        case PipeType.UnderOver:
            currrentDisplay = Instantiate(pipeOverUnder, transform);
            break;
        case PipeType.TIntersection:
            currrentDisplay = Instantiate(pipeTIntersection, transform);
            break;
        case PipeType.XIntersection:
            currrentDisplay = Instantiate(pipeXIntersection, transform);
            break;
        default:
            currrentDisplay = null;
            break;
        }

        if(currrentDisplay != null)
        {
            var boxCollider = currrentDisplay.GetComponentInChildren<BoxCollider>();
            if(boxCollider != null)
            {
                boxCollider.enabled = false;
            }
        }

        currentPipeType = pipeType;
    }

    // Use this for initialization
    void Start()
    {
        cursorPlane = new Plane(Vector3.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if(cursorPlane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }
}
