using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] Transform[] _waypoints;

    [SerializeField] [Range( 0f, 10f )] private float _waitTimer;
    private float _waitTime = 0f;
    [SerializeField] private int _currWaypont;

    [SerializeField] private float _acceptableRadius;

    [SerializeField] bool _waiting;
    [SerializeField] bool _loop;

    void Update()
    {
        Wait();
        PathPatrol();
    }

    void Wait()
    {
        if( _waiting )
        {
            _waitTime += Time.deltaTime;

            if ( _waitTime < _waitTimer )
                return;

            _waiting = false;
        }
    }

    void PathPatrol()
    {
        Transform wp = _waypoints[_currWaypont];

        if ( Vector3.Distance( transform.position, wp.position ) < _acceptableRadius )
        {
            _waitTime = 0f;
            _waiting = true;

            if ( _loop )
            {
                if ( _currWaypont > _waypoints.Length )
                    _currWaypont--;

                _currWaypont = ( _currWaypont + 1 ) % _waypoints.Length;
            } 
            else
            {
                if ( _currWaypont >= _waypoints.Length - 1 )
                    return;

                _currWaypont++;
            }
        }
        else
        {
            if( !_waiting )
            {
                _navAgent.SetDestination( wp.position );
                Quaternion lookRotation = Quaternion.LookRotation( _navAgent.velocity );
                transform.rotation = Quaternion.RotateTowards( transform.rotation, lookRotation, _navAgent.angularSpeed * Time.deltaTime );
            }
        }
    }
}
