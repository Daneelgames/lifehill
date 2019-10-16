using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;

    GameManager gm;
    HealthController hc;

    private void Start()
    {
        hc = GetComponent<HealthController>();
        hc.movement = this;
        gm = hc.gm;

        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(GameObject go)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, go.transform.position + (transform.position - go.transform.position).normalized / 3, NavMesh.AllAreas, path);

        agent.SetPath(path); //Agent is the NavMeshAgent attached to gameObject
        print(agent.pathStatus);
        agent.isStopped = false;

        /*
        Vector3 targetPoint = go.transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(targetPoint, out hit, 3f, NavMesh.AllAreas);

        agent.SetDestination(hit.position + (transform.position - hit.position).normalized / 3);
        */
     
        //agent.SetDestination(go.transform.position + (transform.position - go.transform.position).normalized / 3);
    }
}