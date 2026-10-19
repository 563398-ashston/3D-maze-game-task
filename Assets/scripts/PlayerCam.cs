

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float sensX = 100f;
    public float sensY = 100f;

    [Header("Controller Settings")]
    public float controllerSensX = 150f;
    public float controllerSensY = 150f;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Mouse input
        if (Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float mouseX = mouseDelta.x * Time.deltaTime * sensX;
            float mouseY = mouseDelta.y * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
        }

        // Controller right stick input
        if (Gamepad.current != null)
        {
            Vector2 stickInput = Gamepad.current.rightStick.ReadValue();

            float stickX = stickInput.x * controllerSensX * Time.deltaTime;
            float stickY = stickInput.y * controllerSensY * Time.deltaTime;

            yRotation += stickX;
            xRotation -= stickY;
        }

        // Clamp vertical camera rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate camera and player orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}

