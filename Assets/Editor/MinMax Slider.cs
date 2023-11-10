using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( Patrol ) )]
public class MinMaxSlider : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Patrol patrol = ( Patrol )target;

        patrol.Slider();
    }
}
