using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform[] Waypoints;
    [SerializeField] private int _currWaypont;
    [SerializeField] NavMeshAgent navAgent;
    [SerializeField] private float _acceptableRadius;

    void Update()
    {
        Transform wp = Waypoints[_currWaypont];

        if ( Vector3.Distance( transform.position, wp.position ) < _acceptableRadius )
            _currWaypont = ( _currWaypont + 1 ) % Waypoints.Length;
        else
        {
            navAgent.SetDestination( wp.position );
            Quaternion lookRotation = Quaternion.LookRotation( navAgent.velocity );
            transform.rotation = Quaternion.RotateTowards( transform.rotation, lookRotation, navAgent.angularSpeed * Time.deltaTime );
        }
    }

    void Wait()
    {

    }
}
