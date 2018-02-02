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
    
    public GameObject ActivePipe { get; private set; }

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
        if(this.ActivePipe != null)
        {
            this.ActivePipe.SetActive(false);
        }

        switch(pipeType)
        {
        case PipeType.Straight:
            this.ActivePipe = pipeStraight;
            break;
        case PipeType.Turn:
            this.ActivePipe = pipeTurn;
            break;
        case PipeType.LeftRight:
            this.ActivePipe = pipeLeftRight;
            break;
        case PipeType.UnderOver:
            this.ActivePipe = pipeOverUnder;
            break;
        case PipeType.Mixer:
            this.ActivePipe = pipeMixer;
            break;
        case PipeType.Trash:
            this.ActivePipe = pipeTrash;
            break;
        default:
            this.ActivePipe = null;
            break;
        }

        if(this.ActivePipe != null)
        {
            UpdateRotation();
            this.ActivePipe.SetActive(true);
        }
    }

    public void Hide()
    {
        if(this.ActivePipe != null)
        {
            this.ActivePipe.SetActive(false);
        }
    }

    public void Show()
    {
        if(this.ActivePipe != null)
        {
            this.ActivePipe.SetActive(true);
        }
    }

    private void UpdateRotation()
    {
        if(this.ActivePipe != null)
        {
            var pipeRotation = Quaternion.AngleAxis(90 * (rotation % 4), Vector3.up);
            this.ActivePipe.transform.rotation = pipeRotation;
        }
    }
}