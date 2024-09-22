using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform player; 
    public float mouseSensitivity = 5f;
    public float distanceFromPlayer = 2.0f;
    public float cameraHeight = 1.5f; 
    public float rotationSmoothTime = 0.1f;

    private float yaw = 0f;   
    private float pitch = 0f;   
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -35f, 60f);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = player.position - transform.forward * distanceFromPlayer + Vector3.up * cameraHeight;
    }
}

