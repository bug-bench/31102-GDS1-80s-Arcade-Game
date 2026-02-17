using UnityEngine;
using UnityEngine.InputSystem;

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

    private void FixedUpdate()
    {
        Vector2 input = Vector2.zero;
        if (moveAction != null && moveAction.action != null)
            input = moveAction.action.ReadValue<Vector2>();

        Vector2 direction = input.normalized;

        rb.linearVelocity = direction * speed;
    }
}
