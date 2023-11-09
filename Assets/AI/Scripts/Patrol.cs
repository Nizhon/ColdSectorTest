using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform[] Waypoints;
    [SerializeField] private int _currWaypont;
    [SerializeField] NavMeshAgent navAgent;

    void Update()
    {
        navAgent.SetDestination(Waypoints[_currWaypont].position);
    }
}
