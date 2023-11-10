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
    private float _timer = 0f;
    [SerializeField] private float _waitTime;

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
        //Layout code for our Min Max Slider. This function is used in our Editor script "MinMax Slider"
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField( "Min 0", GUILayout.Width( 40f ) );
        EditorGUILayout.MinMaxSlider( ref _minValue, ref _maxValue, _minLimit, _maxLimit );
        EditorGUILayout.LabelField( "10 Max", GUILayout.Width( 40f ) );
        EditorGUILayout.EndHorizontal();
    }

    void Wait()
    {
        //We increase the time of the _timer untill it's higher then the _waitTime
        if ( _waiting )
        {
            _timer += Time.deltaTime;

            if ( _timer < _waitTime )
                return;

            _waiting = !_waiting;
        }
    }

    void RandomWaitTimer()
    {
        //We get a random value between the min and max value of the slider and set the _waitTime to this value.
        _waitTime = Random.Range( _minValue, _maxValue );
    }

    void PathPatrol()
    {
        //We get our current Waypoint and check if we're in range to continue to do logic.
        Transform wp = _waypoints[_currWaypoint];

        if ( Vector3.Distance( transform.position, wp.position ) < _acceptableRadius )
        { 
            //If we arn't looping and have arrived at our last waypoint we no longer need to run this code
            if( !_Arrived )
            {
                //We get a new _waitTime value and reset the timer.
                RandomWaitTimer();
                _timer = 0f;
                _waiting = !_waiting;
            }

            //If we loop we go from A -> B -> C then back to A and repeat.
            if ( _loop )
            {
                _Arrived = false;
                
                //If we sort while looping we go from A -> B -> C then back in reverese C -> B -> A and loop back.
                if( _sorted )
                {
                    if ( _currWaypoint >= _waypoints.Length -1 )
                        _index = -1;
                    else if ( _currWaypoint <= 0 )
                        _index = 1;
                    _currWaypoint += _index;
                }
                else
                    _currWaypoint = ( _currWaypoint + 1 ) % _waypoints.Length; //Takes us back from C to A.
            }
            else
            {
                //If we are not looping and have reached our final waypoint then we stop.
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
                //Moving the AI and making so he rotates in the direction of movement.
                _navAgent.SetDestination( wp.position );
                Quaternion lookRotation = Quaternion.LookRotation( _navAgent.velocity );
                transform.rotation = Quaternion.RotateTowards( transform.rotation, lookRotation, _navAgent.angularSpeed * Time.deltaTime );
            }
        }
    }
}
