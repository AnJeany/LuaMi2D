using UnityEngine;
using System.Collections;

public class KnockBackHandler : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isKnockedBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (isKnockedBack) return;
        StartCoroutine(KnockbackCoroutine(direction));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        isKnockedBack = true;

        float elapsed = 0f;
        Vector2 startPos = rb.position;
        Vector2 targetPos = startPos + direction * knockbackForce;

        while (elapsed < knockbackDuration)
        {
            elapsed += Time.fixedDeltaTime;
            float t = elapsed / knockbackDuration;

            // di chuyển giảm dần (ease-out)
            rb.MovePosition(Vector2.Lerp(targetPos, startPos, t * t));

            yield return new WaitForFixedUpdate();
        }

        isKnockedBack = false;
    }
}
