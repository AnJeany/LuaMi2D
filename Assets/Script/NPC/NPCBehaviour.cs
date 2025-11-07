using UnityEngine;
using System.Collections;

public class NPCBehaviour : MonoBehaviour
{
    [Header("References")]
    public Transform[] waypoints;
    public Transform player;

    [Header("Movement Settings")]
    public float patrolSpeed = 2f;
    public float fleeSpeed = 4f;
    public float detectionRadius = 5f;
    public float calmDelay = 2f;

    private int currentWaypoint = 0;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isFleeing = false;
    private Vector2 fleeTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isFleeing)
        {
            MoveToTarget(fleeTarget, fleeSpeed);
        }
        else if (distanceToPlayer < detectionRadius)
        {
            // Nếu player đang ở gần nhưng không dash, NPC vẫn bình thường
            Patrol();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        Vector2 target = waypoints[currentWaypoint].position;
        MoveToTarget(target, patrolSpeed);

        if (Vector2.Distance(transform.position, target) < 0.1f)
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    private void MoveToTarget(Vector2 target, float speed)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

    public void OnPlayerDashHit(Vector2 fromDirection)
    {
        if (isFleeing) return;

        // sinh hướng ngẫu nhiên dựa theo hướng dash
        Vector2 randomDir = (Random.insideUnitCircle + fromDirection * -1).normalized;
        fleeTarget = (Vector2)transform.position + randomDir * 3f; // chạy ngẫu nhiên 3 đơn vị
        isFleeing = true;

        animator.SetTrigger("Flee");

        StopAllCoroutines();
        StartCoroutine(CalmDownAfterDelay());
    }

    private IEnumerator CalmDownAfterDelay()
    {
        yield return new WaitForSeconds(calmDelay);
        isFleeing = false;
        animator.SetTrigger("Calm");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
