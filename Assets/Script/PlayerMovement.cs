using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 0.01f;
    public float jumpForce = 5f;

    public Transform cameraTransform;

    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float cameraPitch = 0f;
    private bool jumpPressed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpPressed = true;
        }
    }

    void FixedUpdate()
    {
        // Déplacement
        Vector3 movement =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        Vector3 newVelocity = movement * speed;

        rb.linearVelocity = new Vector3(
            newVelocity.x,
            rb.linearVelocity.y,
            newVelocity.z
        );

        // Saut
        if (jumpPressed && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        jumpPressed = false;
    }

    void Update()
    {
        // Rotation du joueur gauche/droite
        //float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseX = lookInput.x;
        

        transform.Rotate(Vector3.up * mouseX);

        // Rotation de la caméra haut/bas
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        cameraTransform.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            1.1f
        );
    }
}