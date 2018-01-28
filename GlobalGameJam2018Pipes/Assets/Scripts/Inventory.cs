using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action InventoryChanged;

    [SerializeField] private int gold;

    [SerializeField] private int pipeStraigthCount;

    [SerializeField] private int pipeTurnCount;

    [SerializeField] private int pipeLeftRightCount;

    [SerializeField] private int pipeOverUnderCount;

    [SerializeField] private int pipeMixerCount;

    [SerializeField] private int pipeTrashCount;
    
    public int Gold
    {
        get { return gold; }
        set { gold = value;
            InventoryChanged?.Invoke();
        }
    }

    public int PipeStraightCount
    {
        get { return pipeStraigthCount; }
        set { pipeStraigthCount = value;
            InventoryChanged?.Invoke();
        }
    }

    public int PipeTurnCount
    {
        get { return pipeTurnCount; }
        set { pipeTurnCount = value;
            InventoryChanged?.Invoke();
        }
    }

    public int PipeLeftRightCount
    {
        get { return pipeLeftRightCount; }
        set { pipeLeftRightCount = value;
            InventoryChanged?.Invoke();
        }
    }

    public int PipeOverUnderCount
    {
        get { return pipeOverUnderCount; }
        set { pipeOverUnderCount = value;
            InventoryChanged?.Invoke();
        }
    }

    public int PipeMixerCount
    {
        get { return pipeMixerCount; }
        set { pipeMixerCount = value;
            InventoryChanged?.Invoke();
        }
    }

    public int PipeTrashCount
    {
        get { return pipeTrashCount; }
        set
        {
            pipeTrashCount = value;
            InventoryChanged?.Invoke();
        }
    }

    public bool HasInventory(PipeType pipeType)
    {
        switch(pipeType)
        {
        case PipeType.Straight:
            return pipeStraigthCount > 0;
        case PipeType.Turn:
            return pipeTurnCount > 0;
        case PipeType.LeftRight:
            return pipeLeftRightCount > 0;
        case PipeType.UnderOver:
            return pipeOverUnderCount > 0;
        case PipeType.Mixer:
            return pipeMixerCount > 0;
        case PipeType.Trash:
            return pipeTrashCount > 0;
        default:
            return false;
        }
    }

    public int getNumber(PipeType pipeType)
    {
        switch (pipeType)
        {
            case PipeType.Straight:
                return pipeStraigthCount;
            case PipeType.Turn:
                return pipeTurnCount;
            case PipeType.LeftRight:
                return pipeLeftRightCount;
            case PipeType.UnderOver:
                return pipeOverUnderCount;
            case PipeType.Mixer:
                return pipeMixerCount;
            case PipeType.Trash:
                return pipeTrashCount;
            default:
                return 0;
        }
    }


    public void Reduce(PipeType pipeType)
    {
        switch(pipeType)
        {
        case PipeType.Straight:
            --pipeStraigthCount;
            break;
        case PipeType.Turn:
            --pipeTurnCount;
            break;
        case PipeType.LeftRight:
            --pipeLeftRightCount;
            break;
        case PipeType.UnderOver:
            --pipeOverUnderCount;
            break;
        case PipeType.Mixer:
            --pipeMixerCount;
            break;
        case PipeType.Trash:
            --pipeTrashCount;
            break;
        }

        InventoryChanged?.Invoke();
    }

    public void Increase(PipeType pipeType)
    {
        switch(pipeType)
        {
        case PipeType.Straight:
            ++pipeStraigthCount;
            break;
        case PipeType.Turn:
            ++pipeTurnCount;
            break;
        case PipeType.LeftRight:
            ++pipeLeftRightCount;
            break;
        case PipeType.UnderOver:
            ++pipeOverUnderCount;
            break;
        case PipeType.Mixer:
            ++pipeMixerCount;
            break;
        case PipeType.Trash:
            ++pipeTrashCount;
            break;
        }

        InventoryChanged?.Invoke();
    }
}
