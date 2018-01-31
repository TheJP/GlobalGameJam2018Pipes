using UnityEngine;

public class Cursor
    : MonoBehaviour
{
    public GameObject pipeStraight;
    public GameObject pipeTurn;
    public GameObject pipeLeftRight;
    public GameObject pipeOverUnder;
    public GameObject pipeMixer;
    public GameObject pipeTrash;

    public GameObject hammer;

    private GameObject currrentDisplay;
    private PipeType currentPipeType;
    private Plane cursorPlane;
    public int currentRotation;

    private bool displaysHammer;

    public void SetPipeDisplay(PipeType pipeType)
    {
        if(currrentDisplay != null)
        {
            currrentDisplay.SetActive(false);
        }

        switch(pipeType)
        {
        case PipeType.Straight:
            currrentDisplay = pipeStraight;
            break;
        case PipeType.Turn:
            currrentDisplay = pipeTurn;
            break;
        case PipeType.LeftRight:
            currrentDisplay = pipeLeftRight;
            break;
        case PipeType.UnderOver:
            currrentDisplay = pipeOverUnder;
            break;
        case PipeType.Mixer:
            currrentDisplay = pipeMixer;
            break;
        case PipeType.Trash:
            currrentDisplay = pipeTrash;
            break;
        default:
            currrentDisplay = null;
            break;
        }

        if(currrentDisplay != null)
        {
            var pipeRotation = Quaternion.AngleAxis(90 * (currentRotation % 4), Vector3.up);
            currrentDisplay.transform.rotation = pipeRotation;
            currrentDisplay.SetActive(true);
        }

        currentPipeType = pipeType;
    }

    public void SetHammerDisplay()
    {
        SetPipeDisplay(PipeType.None);

        displaysHammer = true;
        currrentDisplay = hammer;
        currrentDisplay.SetActive(true);
    }

    private void Start()
    {
        cursorPlane = new Plane(Vector3.up, this.transform.position);
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(1) && currrentDisplay != null && !displaysHammer)
        {
            currrentDisplay.transform.Rotate(Vector3.up, 90);
            ++currentRotation;
            currentRotation %= 4;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        float distance;
        if(cursorPlane.Raycast(ray, out distance))
        {
            this.transform.position = ray.GetPoint(distance);
        }
        if(currrentDisplay != null && !displaysHammer)
        {
            currrentDisplay.SetActive(false);
        }
        
        RaycastHit hit;
        var range = 1000.0f;
        if (Physics.Raycast(ray, out hit, range))
        {
            var target = hit.collider.gameObject;
            if (target.name.Contains("Tile"))
            {
                if (currrentDisplay != null)
                {
                    currrentDisplay.SetActive(true);
                }

                var tile = target.GetComponentInParent<Tile>();

                if (tile.pipe == null)
                {
                    var targetPos = tile.transform.position;
                    this.transform.position = targetPos;
                }
            }
        }
    }
}
