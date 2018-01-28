﻿using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Column;
    public int tileSize;

    public GameObject pipeStraight;
    public GameObject pipeTurn;
    public GameObject pipeLeftRight;
    public GameObject pipeOverUnder;
    public GameObject pipeXIntersection;
    public GameObject pipeMixer;

    public Pipe pipe;

    public bool BuildPipe(PipeType pipeType, int rotation)
    {
        if(pipe != null)
        {
            return false;
        }

        var pipeRotation = Quaternion.AngleAxis(90 * (rotation % 4), Vector3.up);

        GameObject pipeGameObject;
        switch(pipeType)
        {
        case PipeType.Straight:
            pipeGameObject = Instantiate(pipeStraight, transform.position, pipeRotation, transform);
            break;
        case PipeType.Turn:
            pipeGameObject = Instantiate(pipeTurn, transform.position, pipeRotation, transform);
            break;
        case PipeType.LeftRight:
            pipeGameObject = Instantiate(pipeLeftRight, transform.position, pipeRotation, transform);
            break;
        case PipeType.UnderOver:
            pipeGameObject = Instantiate(pipeOverUnder, transform.position, pipeRotation, transform);
            break;
        case PipeType.Mixer:
            pipeGameObject = Instantiate(pipeMixer, transform.position, pipeRotation, transform);
            break;
        case PipeType.XIntersection:
            pipeGameObject = Instantiate(pipeXIntersection, transform.position, pipeRotation, transform);
            break;
        default:
            pipeGameObject = null;
            break;
        }

        if (pipeGameObject != null)
        {
            pipe = pipeGameObject.GetComponent<Pipe>();
            pipe.Rotation = rotation;

            var mixerPipe = pipeGameObject.GetComponent<MixerPipe>();
            if (mixerPipe != null)
            {
                mixerPipe.row = Row;
                mixerPipe.column = Column;
            }

            return true;
        }

        return false;
    }

    public void RemovePipe()
    {
        pipe = null;
    }
}
