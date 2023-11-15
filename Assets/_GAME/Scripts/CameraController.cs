using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera System Variables")]
    [SerializeField] private float panSpeedHorizontal = 20f;
    [SerializeField] private float panSpeedVertical = 20f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Camera System Switches")]
    [Tooltip("Pan mode is the movement of Camera using mouse.\n" +
        "Forward and Backward movement affects Z Dimension of Camera.\n" +
        "Left and Right movement affects X Dimension of Camera.\n" +
        "MouseWheel affects Y dimension of Camera.\n" +
        "Press Q Key to activate and deactivate.")]
    [SerializeField] private bool isPanMode = false;
    [Tooltip("Zoom mode is the closeness of Camera to subject using only mousewheel.\n" +
        "MouseWheel affects zoom level.\n" +
        "Press W Key to activate and deactivate.")]
    [SerializeField] private bool isZoomMode = false;
    [Tooltip("Rotate mode is the angle of Camera for each dimension using mouse.\n" +
        "Left and Right movement horizontally turns the camera around the subject.\n" +
        "Forward and Backward movement vertically turns the camera around the subject.\n" +
        "Press E Key to activate and deactivate.")]
    [SerializeField] private bool isRotateMode = false;
    [Tooltip("The subject we need to reference.")]
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

