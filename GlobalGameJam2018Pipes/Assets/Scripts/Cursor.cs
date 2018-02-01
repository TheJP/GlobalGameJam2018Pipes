using UnityEngine;


public class Cursor
    : MonoBehaviour
{
    [SerializeField]
    private PipeDisplay pipeDisplay;
    
    [SerializeField]
    private GameObject hammer;

    private Plane cursorPlane;

    public int PipeRotation => pipeDisplay.Rotation;

    public void ShowPipe(PipeType pipeType)
    {
        pipeDisplay.ShowPipe(pipeType);
        hammer.SetActive(false);
    }

    public void ShowHammer()
    {
        pipeDisplay.ShowPipe(PipeType.None);
        hammer.SetActive(true);
    }

    private void Start()
    {
        cursorPlane = new Plane(Vector3.up, this.transform.position);
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(1))
        {
            ++pipeDisplay.Rotation;
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        float distance;
        if(cursorPlane.Raycast(ray, out distance))
        {
            this.transform.position = ray.GetPoint(distance);
        }

        if(!hammer.activeSelf)
        {
            pipeDisplay.Hide();

            RaycastHit hit;
            var range = 1000.0f;
            if(Physics.Raycast(ray, out hit, range))
            {
                var target = hit.collider.gameObject;
                if(target.name.Contains("Tile"))
                {
                    var tile = target.GetComponentInParent<Tile>();

                    if(tile.pipe == null)
                    {
                        pipeDisplay.Show();
                        var targetPos = tile.transform.position;
                        this.transform.position = targetPos;
                    }
                }
            }
        }
    }
}
