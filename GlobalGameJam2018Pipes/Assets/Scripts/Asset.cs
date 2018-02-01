using UnityEngine;
using UnityEngine.UI;


public class Asset : MonoBehaviour
{
    public int Row;
    public int Column;

    [SerializeField]
    private PipeDisplay pipeDisplay;
    
    [SerializeField]
    private GameObject gold;
    
    [SerializeField]
    private GameObject hammer;

    public PipeType PipeType { get; private set; }

    public bool DisplaysGold => gold.activeSelf;
    public bool DisplaysHammer => hammer.activeSelf;

    private Text itemCountText;

    private void Awake()
    {
        itemCountText = GetComponentInChildren<Text>();
    }

    public void SetPipeDisplay(PipeType type)
    {
        SetDisplay(type, false, false);
    }

    public void SetGoldDisplay()
    {
        SetDisplay(PipeType.None, false, true);
    }

    public void SetHammerDisplay()
    {
        SetDisplay(PipeType.None, true, false);
    }

    public void SetCount(int count)
    {
        UpdateText(count);
    }

    private void SetDisplay(PipeType pipeType, bool displayHammer, bool displayGold)
    {
        gold.SetActive(displayGold);
        hammer.SetActive(displayHammer);
        
        this.PipeType = pipeType;
        pipeDisplay.ShowPipe(pipeType);
    }
    
    private void UpdateText(int count)
    {
        if(this.DisplaysHammer)
        {
            itemCountText.text = "";
        }
        else
        {
            itemCountText.text = count + "x";
        }
    }
}
