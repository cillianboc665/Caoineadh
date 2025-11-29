using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    enum States
    {
        Idle, Patrolling, Chasing
    }

    [SerializeField] private Transform waypoints;
    private int currentWaypoint;
    NavMeshAgent agent;

    [SerializeField] private States currentState;
    [SerializeField] private float sightRange;
    [SerializeField] private float susTime;
    private float timeSinceLastSaw;
    public GameObject player;
    [SerializeField] private float waitAtPoint = 2f;
    private float waitCounter;

    [SerializeField] private float visionAngle = 45;

    public float eyeLevel;

    //[SerializeField] private LayerMask obstructionLayers = ~0;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        timeSinceLastSaw = susTime;
    }

    void FixedUpdate()
    {
        VisionCheck();
    }

    private void VisionCheck()
    {
        if (PlayerSeen() && currentState != States.Chasing)
        {
            currentState = States.Chasing;
            timeSinceLastSaw = susTime;
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case States.Idle:

                agent.speed = 2.5f;

                if (waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else
                {
                    currentState = States.Patrolling;
                    agent.SetDestination(waypoints.GetChild(currentWaypoint).position);
                }

                if (PlayerSeen())
                {
                    currentState = States.Chasing;
                }

                break;

            case States.Patrolling:

                agent.speed = 2.5f;

                if (agent.remainingDistance <= 0.2f)
                {
                    currentWaypoint++;
                    if (currentWaypoint >= waypoints.childCount)
                    {
                        currentWaypoint = 0;
                    }

                    if ((currentWaypoint == 1 || currentWaypoint == 5 || currentWaypoint == 8))
                    {
                        currentState = States.Idle;
                        waitCounter = waitAtPoint;
                    }
                    else
                    {
                        agent.SetDestination(waypoints.GetChild(currentWaypoint).position);
                    }
                }

                break;

            case States.Chasing:

                agent.SetDestination(player.transform.position);
                agent.speed = 6.5f;

                float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

                if (distToPlayer > sightRange && !PlayerSeen())
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    timeSinceLastSaw -= Time.deltaTime;

                    if (timeSinceLastSaw < 0)
                    {
                        currentState = States.Idle;
                        timeSinceLastSaw = susTime;
                        agent.isStopped = false;
                    }
                }

                else
                {
                    timeSinceLastSaw = susTime;
                    agent.isStopped = false;
                }

                break;
        }
    }

    private bool PlayerSeen()
    {
        Vector3 eyePos = transform.position + Vector3.up * eyeLevel;

        Vector3 playerEyePos = player.transform.position + Vector3.up * eyeLevel;

        Vector3 directionToPlayer = (player.transform.position - eyePos).normalized;  // may need to change transform.position to transform.position + eyeOffset

        float distanceToPlayer = Vector3.Distance(eyePos, playerEyePos);


        if (distanceToPlayer > sightRange)
        {
            return false;
        }

        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(forward, directionToPlayer);

        if (angle > visionAngle)
        {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(eyePos, directionToPlayer, out hit, sightRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Sees Player");
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }



    private void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        Vector3 eyePos = transform.position + Vector3.up * eyeLevel;

        // Draw eye position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(eyePos, 0.05f);

        // Draw cone lines
        Gizmos.color = Color.yellow;
        Quaternion leftRot = Quaternion.AngleAxis(-visionAngle, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(visionAngle, Vector3.up);

        Vector3 leftDir = leftRot * transform.forward;
        Vector3 rightDir = rightRot * transform.forward;

        Gizmos.DrawLine(eyePos, eyePos + leftDir * sightRange);
        Gizmos.DrawLine(eyePos, eyePos + rightDir * sightRange);

        // Draw arc
        int segments = 30;
        Vector3 prevPoint = eyePos + leftDir * sightRange;

        for (int i = 1; i <= segments; i++)
        {
            float stepAngle = -visionAngle + (visionAngle * 2f) * (i / (float)segments);
            Vector3 nextPoint = eyePos + Quaternion.AngleAxis(stepAngle, Vector3.up) * transform.forward * sightRange;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }

        // Draw line to player if in sight
        if (PlayerSeen())
        {
            Gizmos.color = Color.green;
            Vector3 playerEyePos = player.transform.position + Vector3.up * eyeLevel;
            Gizmos.DrawLine(eyePos, playerEyePos);
        }
    }
}
