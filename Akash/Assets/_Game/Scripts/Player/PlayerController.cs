using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Variables
    [Header("Settings")]
    public float speed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public Transform playerCamera;
    public float gravity = -9.81f;  // Gravity value
    public float jumpHeight = 1.0f; // Jump height

    // Customizable Key Bindings
    [Header("Key Bindings")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;

    // Private Variables
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
      
    }

    void Update()
    {

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movement
        Vector3 move = Vector3.zero;
        if (Input.GetKey(forwardKey)) move += transform.forward;
        if (Input.GetKey(backwardKey)) move -= transform.forward;
        if (Input.GetKey(leftKey)) move -= transform.right;
        if (Input.GetKey(rightKey)) move += transform.right;

        controller.Move(move * speed * Time.deltaTime);

        // Gravity
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Small negative value to keep player grounded
        }

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
