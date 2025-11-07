using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPatroAI : MonoBehaviour
{
    [Header("Waypoints Settings")]
    public Transform waypointParent;        // Parent chứa các waypoint
    public float speed = 2f;                // Tốc độ di chuyển
    public float waitTime = 2f;             // Thời gian chờ mỗi điểm
    public bool loopWaypoints = true;       // Lặp lại sau khi hết đường

    [Header("Random Move Settings")]
    public bool enableRandomBehavior = true; // Cho phép hành vi ngẫu nhiên
    [Range(0f, 1f)] public float randomChance = 0.3f; // Tỉ lệ 0-1 (ví dụ: 0.3 = 30%)
    public float randomMoveRadius = 2f;     // Bán kính di chuyển ngẫu nhiên quanh waypoint

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool isWaiting;

    private Vector2 randomTarget;
    private bool isMovingRandom;

    void Start()
    {
        // Lấy các waypoint con
        int count = waypointParent.childCount;
        waypoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
    }

    void Update()
    {
        if (isWaiting) return;

        if (isMovingRandom)
            MoveToRandomPoint();
        else
            MoveToWaypoint();
    }

    //di chuyen theo waypoint
    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAndDecideNextAction());
        }
    }

    //di chuyen ngau nhien
    void MoveToRandomPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, randomTarget, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, randomTarget) < 0.1f)
        {
            isMovingRandom = false;
            StartCoroutine(WaitAndDecideNextAction());
        }
    }

    //logic cho viec cho va quyet dinh hanh dong tiep theo
    IEnumerator WaitAndDecideNextAction()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        // Nếu có bật chế độ random
        if (enableRandomBehavior && Random.value < randomChance)
        {
            PickRandomNearbyPoint();
        }
        else
        {
            // Tiếp tục waypoint bình thường
            if (loopWaypoints)
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            else
                currentWaypointIndex = Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);
        }

        isWaiting = false;
    }

    //chon diem ngau nhien gan do
    void PickRandomNearbyPoint()
    {
        isMovingRandom = true;

        Vector2 basePos = waypoints[currentWaypointIndex].position;
        float randX = Random.Range(-randomMoveRadius, randomMoveRadius);
        float randY = Random.Range(-randomMoveRadius, randomMoveRadius);

        randomTarget = basePos + new Vector2(randX, randY);
    }
}
