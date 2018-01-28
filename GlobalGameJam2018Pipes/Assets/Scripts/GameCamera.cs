using UnityEngine;

public class GameCamera : MonoBehaviour
{

    public float smoothing = 0.3f;        // The speed with which the camera will be following.

    Vector3 offset;                     // The initial offset from the target.

    private float xPosition = 0;
    private float zPosition = 0;

    private float yPosition = 60; //Zoom

    void Start()
    {
        // Calculate the initial offset.
        offset = transform.position - new Vector3(Input.mousePosition.x, yPosition, Input.mousePosition.y);
    }

    void FixedUpdate()
    {
        // Create a postion the camera is aiming for based on the offset from the target.
        Vector3 targetCamPos = new Vector3(xPosition, yPosition, zPosition) + offset;

        // Smoothly interpolate between the camera's current position and it's target position.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

    private void Update()
    {
        Input.GetAxis("Left");

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            yPosition += scroll * -10;
        }
    }
}