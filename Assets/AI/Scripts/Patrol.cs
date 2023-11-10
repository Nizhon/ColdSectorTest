using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu( "AI/AIPatrol" )]
public class Patrol : MonoBehaviour
{
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] Transform[] _waypoints;

    [SerializeField] private int _currWaypoint;
    private int _index;
    [SerializeField] private float _acceptableRadius;

    [SerializeField] bool _loop;
    [SerializeField] bool _sorted;
    [SerializeField] bool _Arrived;

    [SerializeField] bool _waiting;
    private float _waitTime = 0f;
    [SerializeField] private float _waitTimer;

    [SerializeField] private float _minValue;
    private float _minLimit = 0;
    [SerializeField] private float _maxValue;
    private float _maxLimit = 10;

    void Update()
    {
        Wait();
        PathPatrol();
    }

    public void Slider()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField( "Min 0", GUILayout.Width( 40f ) );
        EditorGUILayout.MinMaxSlider( ref _minValue, ref _maxValue, _minLimit, _maxLimit );
        EditorGUILayout.LabelField( "10 Max", GUILayout.Width( 40f ) );
        EditorGUILayout.EndHorizontal();
    }

    void Wait()
    {
        if ( _waiting )
        {
            _waitTime += Time.deltaTime;

            if ( _waitTime < _waitTimer )
                return;

            _waiting = !_waiting;
        }
    }

    void RandomWaitTimer()
    {
        _waitTimer = Random.Range( _minValue, _maxValue );
    }

    void PathPatrol()
    {
        Transform wp = _waypoints[_currWaypoint];

        if ( Vector3.Distance( transform.position, wp.position ) < _acceptableRadius )
        { 
            if( !_Arrived )
            {
                RandomWaitTimer();

                _waitTime = 0f;
                _waiting = !_waiting;
            }

            if ( _loop )
            {
                _Arrived = false;

                if( _sorted )
                {
                    if ( _currWaypoint >= _waypoints.Length -1 )
                        _index = -1;
                    else if ( _currWaypoint <= 0 )
                        _index = 1;
                    _currWaypoint += _index;
                }
                else
                    _currWaypoint = ( _currWaypoint + 1 ) % _waypoints.Length;
            }
            else
            {
                if ( !_loop && _currWaypoint >= _waypoints.Length - 1 )
                {
                    _Arrived = true;
                    _waiting = false;
                    return;
                }
                _currWaypoint++;
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
