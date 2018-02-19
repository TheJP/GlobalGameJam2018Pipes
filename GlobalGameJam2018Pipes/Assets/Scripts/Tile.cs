using UnityEngine;

public class Tile : MonoBehaviour
{
    public int Row;
    public int Column;
    public int tileSize;

    [SerializeField]
    private PipeDisplay pipeDisplay;

    public float maxBlockTime;
    private float blockTime;

    [HideInInspector]
    public Pipe pipe;

    private ItemBehaviour blockingItem;

    public bool BuildPipe(PipeType pipeType, int rotation)
    {
        if (pipeDisplay.ActivePipe == null)
        {
            pipeDisplay.ShowPipe(pipeType);
            pipeDisplay.Rotation = rotation;

            pipe = pipeDisplay.ActivePipe.GetComponent<Pipe>();
            pipe.Rotation = rotation;

            var mixerPipe = pipeDisplay.ActivePipe.GetComponent<MixerPipe>();
            if (mixerPipe != null)
            {
                mixerPipe.row = Row;
                mixerPipe.column = Column;
            }

            return true;
        }

        pipe = null;
        return false;
    }

    public void RemovePipe()
    {
        pipe = null;
        pipeDisplay.ShowPipe(PipeType.None);
    }

    public void Block(ItemBehaviour item)
    {
        if (item == null)
        {
            return;
        }

        RemovePipe();

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
