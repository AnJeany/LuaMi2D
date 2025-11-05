using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpTime = 5f;
    [SerializeField] bool canJump = true;
    private bool isJumping = false;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public PlayerInput playerInput;
    private bool isDashing = false;
    private bool canDash = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    private void OnEnable()
    {
        var actions = playerInput.actions;
        actions["Move"].performed += Move;
        actions["Move"].canceled += OnMoveCanceled;
        actions["Dash"].performed += Dash;
    }

    private void OnDisable()
    {
        var actions = playerInput.actions;
        actions["Move"].performed -= Move;
        actions["Move"].canceled -= OnMoveCanceled;
        actions["Dash"].performed -= Dash;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void Dash(InputAction.CallbackContext context)
    {
        if (canDash && moveInput != Vector2.zero)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            StartCoroutine(Jump());
        }
    }
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        Vector2 dashDirection = moveInput.normalized;
        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Jump()
    {
        canJump = false;
        isJumping = true;
        spriteRenderer.enabled = false;
        
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
        canJump = true;
        spriteRenderer.enabled = true;
    }
}
