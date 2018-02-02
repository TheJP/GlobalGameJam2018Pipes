using System.Collections;
using UnityEngine;

public class MixerPipe : MonoBehaviour
{
    [SerializeField] private ItemSource itemSourcePrefab;

    public int row;
    public int column;

    private MixerScript mixer;
    private Pipe pipe;
    private ParticleSystem steam;

    private ItemBehaviour storedItem;

	// Use this for initialization
	void Awake()
	{
        mixer = new MixerScript();
	}

    void Start()
    {
        pipe = GetComponent<Pipe>();
        steam = GetComponentInChildren<ParticleSystem>();
    }
	
    public void ProcessItem(ItemBehaviour itemBehaviour)
    {
        if (storedItem == null)
        {
            storedItem = itemBehaviour;
            return;
        }
        steam.Play();
        StartCoroutine(DelayedDisplayMix(0.7f, itemBehaviour));
    }

    private IEnumerator DelayedDisplayMix(float delay, ItemBehaviour behaviour)
    {
        yield return new WaitForSeconds(delay);

        var newMaterial = mixer.Mix(storedItem.material, behaviour.material);
        Destroy(storedItem.gameObject);
        Destroy(behaviour.gameObject);
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
