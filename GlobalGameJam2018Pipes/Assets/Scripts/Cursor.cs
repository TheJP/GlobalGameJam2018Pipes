﻿using UnityEngine;

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
        displaysHammer = false;

        if(currrentDisplay != null)
        {
            Destroy(currrentDisplay);
        }

        var pipeRotation = Quaternion.AngleAxis(90 * (currentRotation % 4), Vector3.up);

        switch(pipeType)
        {
        case PipeType.Straight:
            currrentDisplay = Instantiate(pipeStraight, transform.position, pipeRotation, transform);
            break;
        case PipeType.Turn:
            currrentDisplay = Instantiate(pipeTurn, transform.position, pipeRotation, transform);
            break;
        case PipeType.LeftRight:
            currrentDisplay = Instantiate(pipeLeftRight, transform.position, pipeRotation, transform);
            break;
        case PipeType.UnderOver:
            currrentDisplay = Instantiate(pipeOverUnder, transform.position, pipeRotation, transform);
            break;
        case PipeType.Mixer:
            currrentDisplay = Instantiate(pipeMixer, transform.position, pipeRotation, transform);
            break;
        case PipeType.Trash:
            currrentDisplay = Instantiate(pipeTrash, transform.position, pipeRotation, transform);
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

    public void SetHammerDisplay()
    {
        SetPipeDisplay(PipeType.None);

        displaysHammer = true;
        currrentDisplay = Instantiate(hammer, transform);
    }

    // Use this for initialization
    void Start()
    {
        cursorPlane = new Plane(Vector3.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(1) && currrentDisplay != null)
        {
            currrentDisplay.transform.Rotate(Vector3.up, 90);
            ++currentRotation;
            currentRotation %= 4;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        float distance;
        if(cursorPlane.Raycast(ray, out distance))
        {
            transform.position = ray.GetPoint(distance);
        }
        if(currrentDisplay != null && !displaysHammer)
        {
            currrentDisplay.SetActive(false);
        }
        
        RaycastHit hit;
        var range = 1000.0f;
        if (Physics.Raycast(ray, out hit, range))
        {
            GameObject target = hit.collider.gameObject;
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
                    transform.position = targetPos;
                }
            }
        }
    }
}
