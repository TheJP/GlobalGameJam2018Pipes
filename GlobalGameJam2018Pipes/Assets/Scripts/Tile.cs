using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Column;
    public int tileSize;

    public GameObject pipeStraight;
    public GameObject pipeTurn;
    public GameObject pipeLeftRight;
    public GameObject pipeOverUnder;
    public GameObject pipeMixer;
    public GameObject pipeTrash;

    public float maxBlockTime;
    private float blockTime;

    public Pipe pipe;

    private ItemBehaviour blockingItem;

    public bool BuildPipe(PipeType pipeType, int rotation)
    {
        if(pipe != null || blockingItem != null)
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
        case PipeType.Trash:
            pipeGameObject = Instantiate(pipeTrash, transform.position, pipeRotation, transform);
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

    public void Block(ItemBehaviour item)
    {
        if (item == null)
        {
            return;
        }

        if (pipe != null)
        {
            Destroy(pipe.gameObject);
            pipe = null;
        }

        blockTime = maxBlockTime;
        blockingItem = item;
    }

    private void Update()
    {
        if (blockingItem == null)
        {
            return;
        }

        blockTime -= Time.deltaTime;

        if (blockTime <= 0)
        {
            Destroy(blockingItem.gameObject);
            blockingItem = null;
        }
    }
}
