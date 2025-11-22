using UnityEngine;

public class PlayerStateChanger : MonoBehaviour
{
    [Header("State Management")]
    [SerializeField] private PlayerStateData[] states;  // Assign 4 SO theo thứ tự: [0]=Normal, [1]=Ball, [2]=Thin, [3]=SuperThin
    [SerializeField] private PlayerState currentState = PlayerState.Normal;
    private PlayerStateData currentStateData;
    [SerializeField] SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private bool crushed = false;
    private BoxCollider2D boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        SetState(PlayerState.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        SkinChanger skinChanger = other.GetComponent<SkinChanger>();
        if (skinChanger != null)
        {
            // Đổi sprite của player
            spriteRenderer.sprite = skinChanger.skinSprite;
        }
        if (other.TryGetComponent<Kneader>(out var kneading) && crushed == false)
        {
            SetState(PlayerState.Ball);
        }
        else if (other.TryGetComponent<Crusher>(out var crushing))
        {
            if (crushed == false)
            {
                SetState(PlayerState.Thin);
                crushed = true;
            }
            else if (crushed == true)
                SetState(PlayerState.SuperThin);
        }

    }
    public void SetState(PlayerState newState)
    {
        if ((int)newState >= states.Length)
        {
            Debug.LogWarning($"Invalid state index: {newState}");
            return;
        }

        currentState = newState;
        currentStateData = states[(int)newState];

        // Visual
        if (spriteRenderer != null)
            spriteRenderer.sprite = currentStateData.sprite;
        // Movement
        playerMovement.SetMoveSpeed(currentStateData.moveSpeed);

        // Collider (hitbox) - disable/enable để tránh glitch resize
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
            boxCollider.size = currentStateData.hitboxSize;
            boxCollider.offset = new Vector2(0f, currentStateData.colliderOffsetY);
            boxCollider.enabled = true;
            Physics2D.SyncTransforms();  // Sync với physics world
        }
    }
}
