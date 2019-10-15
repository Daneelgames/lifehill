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
        //NEED TO GO TO THE CLOSEST POINT TO ME
       agent.SetDestination(go.transform.position);
    }
}