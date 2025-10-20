using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 pathPoint;
    bool pathPointSet;
    public float pathPointRange;

    public float sightRange;
    public bool seesPlayer;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        seesPlayer = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if (!seesPlayer)
            Patroling();
        else if (seesPlayer)
            Chase();
    }

    private void Patroling()
    {
        if (!pathPointSet)
            SearchPathPoint();

        if (pathPointSet)
            agent.SetDestination(pathPoint);

        Vector3 distToPathPoint = transform.position - pathPoint;

        if (distToPathPoint.magnitude < 1f)
            pathPointSet = false;
    }

    private void SearchPathPoint()
    {
        float randomZ = Random.Range(-pathPointRange, pathPointRange);
        float randomX = Random.Range(-pathPointRange, pathPointRange);

        pathPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(pathPoint, -transform.up, 2f, whatIsGround))
            pathPointSet = true;
    }


    private void Chase()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);
    }
}
