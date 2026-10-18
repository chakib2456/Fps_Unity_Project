using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    private float xRotation = 0f;
    
    private PlayerInput playerInput;
    private InputAction lookAction;
    private Vector2 lookInput;

    void Awake()
    {
        // Get or add PlayerInput component
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }
        
        // Setup look action
        lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
        lookAction.Enable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        lookAction?.Enable();
    }

    void OnDisable()
    {
        lookAction?.Disable();
    }

    void Update()
    {
        // Read the look input
        lookInput = lookAction.ReadValue<Vector2>();
        
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // Rotate the parent (player) horizontally
        if (transform.parent != null)
        {
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

    void OnDestroy()
    {
        lookAction?.Dispose();
    }
}