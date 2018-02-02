using UnityEngine;

public class MixerPipe : MonoBehaviour
{
    [SerializeField] private ItemSource itemSourcePrefab;

    public int row;
    public int column;

    private MixerScript mixer;
    private Pipe pipe;

    private ItemBehaviour storedItem;

	// Use this for initialization
	void Awake()
	{
        mixer = new MixerScript();
	}

    void Start()
    {
        pipe = GetComponent<Pipe>();
    }
	
    public void ProcessItem(ItemBehaviour itemBehaviour)
    {
        if (storedItem == null)
        {
            storedItem = itemBehaviour;
            return;
        }

        var newMaterial = mixer.Mix(storedItem.material, itemBehaviour.material);
        Destroy(storedItem.gameObject);
        Destroy(itemBehaviour.gameObject);

        storedItem = itemSourcePrefab.CreateItem(newMaterial, transform.position, row, column);
    }

    public void ReleaseItem()
    {
        if (storedItem == null)
        {
            return;
        }

        switch (pipe.Rotation)
        {
        case 0:
            storedItem.StartMoving(FlowDirection.ToRight);
            break;
        case 1:
            storedItem.StartMoving(FlowDirection.ToDown);
            break;
        case 2:
            storedItem.StartMoving(FlowDirection.ToLeft);
            break;
        case 3:
            storedItem.StartMoving(FlowDirection.ToTop);
            break;
        default:
            return;
        }

        storedItem = null;
    }
}
