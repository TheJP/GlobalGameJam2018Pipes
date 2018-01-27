using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private PipeType buildNext;

    [SerializeField] private Cursor cursor;

    // TODO previousBuildNext can be removed when we select pipes via the table
    private PipeType previousBuildNext;

    public void Start()
    {
        SetBuildNext(PipeType.Straight);
    }

    public bool SetBuildNext(PipeType pipeType)
    {
        cursor.SetPipeDisplay(pipeType);

        if (inventory.HasInventory(pipeType))
        {
            buildNext = pipeType;
            return true;
        }

        return false;
    }

    void Update()
    {
        // TODO This entire if block can be removed later, when we can select the pipe on the table
        if (buildNext != previousBuildNext)
        {
            previousBuildNext = buildNext;
            SetBuildNext(buildNext);
        }

        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float range = 1000.0F;

            Debug.DrawRay(transform.position, (Input.mousePosition), Color.green);

            if(Physics.Raycast(ray, out hit, range))
            {
                GameObject target = hit.collider.gameObject;
                if(target.name.Contains("Tile"))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);

                    if (inventory.HasInventory(buildNext) && target.GetComponentInParent<Tile>().BuildPipe(buildNext, cursor.currentRotation))
                    {
                        inventory.Reduce(buildNext);
                    }
                }

                if (target.name.Contains("Asset"))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                }

                if(target.tag == "Pipe")
                {
                    Debug.Log("Hit a Pipe");
                    target.GetComponent<DestroyPipe>().ReduceLifetime();
                }

            }
        }
    }
}
