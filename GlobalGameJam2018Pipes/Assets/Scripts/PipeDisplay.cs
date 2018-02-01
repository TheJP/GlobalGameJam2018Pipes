using System;
using UnityEngine;


[Serializable]
public class PipeDisplay
{
    [SerializeField]
    private GameObject pipeStraight;
    
    [SerializeField]
    private GameObject pipeTurn;
    
    [SerializeField]
    private GameObject pipeLeftRight;
    
    [SerializeField]
    private GameObject pipeOverUnder;
    
    [SerializeField]
    private GameObject pipeMixer;
    
    [SerializeField]
    private GameObject pipeTrash;

    private int rotation;
    private GameObject activePipe;

    public int Rotation
    {
        get { return rotation; }
        set
        {
            rotation = value % 4;
            UpdateRotation();
        }
    }

    public void ShowPipe(PipeType pipeType)
    {
        if(activePipe != null)
        {
            activePipe.SetActive(false);
        }

        switch(pipeType)
        {
        case PipeType.Straight:
            activePipe = pipeStraight;
            break;
        case PipeType.Turn:
            activePipe = pipeTurn;
            break;
        case PipeType.LeftRight:
            activePipe = pipeLeftRight;
            break;
        case PipeType.UnderOver:
            activePipe = pipeOverUnder;
            break;
        case PipeType.Mixer:
            activePipe = pipeMixer;
            break;
        case PipeType.Trash:
            activePipe = pipeTrash;
            break;
        default:
            activePipe = null;
            break;
        }

        if(activePipe != null)
        {
            UpdateRotation();
            activePipe.SetActive(true);
        }
    }

    public void Hide()
    {
        if(activePipe != null)
        {
            activePipe.SetActive(false);
        }
    }

    public void Show()
    {
        if(activePipe != null)
        {
            activePipe.SetActive(true);
        }
    }

    private void UpdateRotation()
    {
        if(activePipe != null)
        {
            var pipeRotation = Quaternion.AngleAxis(90 * (rotation % 4), Vector3.up);
            activePipe.transform.rotation = pipeRotation;
        }
    }
}