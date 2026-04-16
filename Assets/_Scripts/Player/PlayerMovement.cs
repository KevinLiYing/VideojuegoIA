using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    private Vector2 moveInput;
    private Rigidbody rb;

    private InputSystem controls;

    private void Awake()
    {
        controls = new InputSystem();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>() * -1;
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveAmount = moveInput.y;
        Vector3 move = transform.forward * moveAmount * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        float turnAmount = moveInput.x;
        float turn = turnAmount * turnSpeed * Time.fixedDeltaTime * -1;
        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }
}