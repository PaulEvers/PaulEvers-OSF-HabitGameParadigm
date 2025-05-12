using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [HideInInspector] public Vector2 moveVelocity;
    private Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // Prevents player rotating

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get movement input from WASD or arrow keys
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize(); // Ensure consistent movement speed when moving diagonally
        moveVelocity = moveInput * moveSpeed;

        animator.SetFloat("Horizontal", moveVelocity.x);
        animator.SetFloat("Vertical", moveVelocity.y);
        animator.SetFloat("Speed", moveVelocity.sqrMagnitude);
    }

    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
