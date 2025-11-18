using UnityEngine;
using System.Collections;

public class NPCBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFleeing = false;
    private Vector2 fleeDirection;
    private float fleeSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isFleeing)
        {
            rb.linearVelocity = fleeDirection * fleeSpeed;
        }
    }

    public void OnPlayerDashHit(Vector2 fromDir)
    {
        // Khi bị va chạm dash
        isFleeing = true;
        fleeDirection = fromDir.normalized;
        fleeSpeed = Random.Range(4f, 7f); // tốc độ chạy đi
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi chạm tường, vật thể hoặc vật cản
        if (isFleeing && !collision.collider.CompareTag("Player"))
        {
            // Hiệu ứng nhỏ trước khi biến mất (tuỳ chọn)
            Destroy(gameObject);
            
        }
    }
}
