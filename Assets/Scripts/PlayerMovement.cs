using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
            moveAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
            moveAction.action.Disable();
    }

    // Physics-driven movement (4-direction, normalized so diagonal isn't faster)
    private void FixedUpdate()
    {
        Vector2 input = Vector2.zero;
        if (moveAction != null && moveAction.action != null)
            input = moveAction.action.ReadValue<Vector2>();

        Vector2 direction = input.normalized;

        // use linearVelocity (project uses this API)
        rb.linearVelocity = direction * speed;
    }
}
