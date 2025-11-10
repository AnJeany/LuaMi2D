using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement1 : MonoBehaviour
{   private PlayerMovement1 playerMovement;

    [Header("Movement Settings")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;
    private Vector2 currentVelocity;


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
        if (isDashing)
            return;

        if (!isDashing)
        {
            Vector2 targetVelocity = moveInput * moveSpeed;

            //Nếu đang có input → tăng tốc
            if (moveInput.magnitude > 0.1f)
            {
                currentVelocity = Vector2.MoveTowards(
                    currentVelocity,
                    targetVelocity,
                    acceleration * Time.fixedDeltaTime
                );
            }
            else // Nếu thả phím → giảm tốc dần về 0
            {
                currentVelocity = Vector2.MoveTowards(
                    currentVelocity,
                    Vector2.zero,
                    deceleration * Time.fixedDeltaTime
                );
            }

            rb.linearVelocity = currentVelocity;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerMovement || !other.CompareTag("NPC")) return;
        if (!playerMovement.IsDashing()) return;

        Vector2 fromDir = (transform.position - other.transform.position).normalized;

        NPCBehaviour npc = other.GetComponent<NPCBehaviour>();
        if (npc != null)
        {
            npc.OnPlayerDashHit(fromDir);
        }

        SkinChanger skinChanger = other.GetComponent<SkinChanger>();
        if (skinChanger != null)
        {
            // Đổi sprite của player
            spriteRenderer.sprite = skinChanger.skinSprite;
        }
    }

    private bool IsDashing()
    {
        return isDashing;
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
        float elapsed = 0f;

        // Giai đoạn 1: Lướt nhanh
        while (elapsed < dashDuration)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        //Giai đoạn 2: Giảm tốc dần
        while (rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 2f);
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;

        // reset trạng thái
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


   
