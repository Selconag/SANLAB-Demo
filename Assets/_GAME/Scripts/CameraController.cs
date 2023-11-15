using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeedHorizontal = 20f;
    public float panSpeedVertical = 20f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 10f;

    private bool isPanMode = false;
    private bool isZoomMode = false;
    private bool isRotateMode = false;

    [SerializeReference]private Transform targetPoint; // The point to rotate around

    void Update()
    {
        // Toggle Pan Mode
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPanMode = !isPanMode;
            isZoomMode = false;
            isRotateMode = false;
        }

        // Toggle Zoom Mode
        if (Input.GetKeyDown(KeyCode.W))
        {
            isZoomMode = !isZoomMode;
            isPanMode = false;
            isRotateMode = false;
        }

        // Toggle Rotate Mode
        if (Input.GetKeyDown(KeyCode.E))
        {
            isRotateMode = !isRotateMode;
            isPanMode = false;
            isZoomMode = false;
        }

        // Perform selected mode
        if (isPanMode)
        {
            PanCamera();
        }
        else if (isZoomMode)
        {
            ZoomCamera();
        }
        else if (isRotateMode)
        {
            RotateCamera();
        }
    }

    void PanCamera()
    {
        float horizontalZ = Input.GetAxis("Mouse X");
        float horizontalX = Input.GetAxis("Mouse Y");
        float vertical = Input.GetAxis("Mouse ScrollWheel") * panSpeedVertical * Time.deltaTime;

        Vector3 panDirection = new Vector3(horizontalZ, vertical, horizontalX).normalized;
        Vector3 pan = panDirection * panSpeedHorizontal * Time.deltaTime;

        transform.Translate(pan, Space.Self);
    }

    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = scroll * zoomSpeed * transform.forward * Time.deltaTime;

        transform.Translate(zoom, Space.World);
    }

    void RotateCamera()
    {
        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Rotate around the targetPoint while keeping the distance constant
        transform.RotateAround(targetPoint.position, Vector3.up, -rotationX);
        transform.RotateAround(targetPoint.position, transform.right, rotationY);
    }
}

