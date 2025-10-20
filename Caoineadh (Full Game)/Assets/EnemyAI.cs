using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform waypoints;
    private int currentWaypoint;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= 0.2f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.childCount)
            {
                currentWaypoint = 0;
            }

            agent.SetDestination(waypoints.GetChild(currentWaypoint).position);

        }
    }
}
