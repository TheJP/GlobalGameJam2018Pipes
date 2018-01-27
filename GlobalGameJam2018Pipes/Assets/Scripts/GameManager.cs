using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private PipeType buildNext;

    public void Start()
    {
    }

    public bool SetBuildNext(PipeType pipeType)
    {
        if (inventory.HasInventory(pipeType))
        {
            buildNext = pipeType;
            return true;
        }

        return false;
    }

    void Update()
    {
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

                    if (inventory.HasInventory(buildNext) && target.GetComponentInParent<Tile>().BuildPipe(buildNext))
                    {
                        inventory.Reduce(buildNext);
                    }
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
