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
	
	// Update is called once per frame
	void Update ()
	{
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
            storedItem.StepRight();
            break;
        case 1:
            storedItem.StepDown();
            break;
        case 2:
            storedItem.StepLeft();
            break;
        case 3:
            storedItem.StepUp();
            break;
        default:
            return;
        }

        storedItem = null;
    }
}
