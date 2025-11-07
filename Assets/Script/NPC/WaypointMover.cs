using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform waypointParent; // Reference to the parent object containing waypoints
    public float speed = 2f; // Speed of movement
    public float waitTime = 2f; // Time to wait at each waypoint
    public bool loopWaitPoint = true; // Should the movement loop

    private Transform[] waypoints; // Array to hold waypoint transforms
    private int currentWaypointIndex = 0; // Current waypoint index
    private bool isWaiting; 
   
    void Start()
    {
       waypoints = new Transform[waypointParent.childCount];

        for (int i = 0; i < waypointParent.childCount; i++)
         {
              waypoints[i] = waypointParent.GetChild(i);
        }   
    }

    // Update is called once per frame
    void Update()
    {


        MoveToWayPoint();
    }

    void MoveToWayPoint()
    {
        Transform target = waypoints[currentWaypointIndex];

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (!isWaiting && Vector2.Distance(transform.position, target.position) < 0.1f )
        {
            StartCoroutine(WaitAtWayPoint());
        }

    }

    IEnumerator WaitAtWayPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        currentWaypointIndex = loopWaitPoint ? (currentWaypointIndex + 1) % waypoints.Length : Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);

        isWaiting = false;

    }

}
