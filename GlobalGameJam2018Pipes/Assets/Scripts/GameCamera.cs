using UnityEngine;

public class GameCamera : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float range = 1000.0F;

            Debug.DrawRay(transform.position, (Input.mousePosition), Color.green);

            if (Physics.Raycast(ray, out hit, range))
            {

                if (hit.collider.gameObject.name.Contains("Tile"))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }

            }
        }
    }
}
