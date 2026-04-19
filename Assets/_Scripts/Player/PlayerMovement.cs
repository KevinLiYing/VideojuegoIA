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

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>() * -1f;
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

        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        Vector3 move = transform.forward * moveInput.y * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        float turn = moveInput.x * turnSpeed * Mathf.Deg2Rad * -1;
        rb.angularVelocity = new Vector3(0f, turn, 0f);
    }
}