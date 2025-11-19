using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Conveyor Settings")]
    [Tooltip("Hướng đẩy, tính bằng độ. 0 = phải, 90 = lên, 180 = trái, 270 = xuống")]
    public float directionAngle = 0f;

    [Tooltip("Lực đẩy của băng chuyền")]
    public float pushSpeed = 2f;

    private Vector2 PushDirection
    {
        get
        {
            float rad = directionAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.SetConveyor(PushDirection * pushSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.ClearConveyor();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)(PushDirection * 1.5f);
        Gizmos.DrawLine(start, end);
        Gizmos.DrawSphere(end, 0.1f);
    }
}
