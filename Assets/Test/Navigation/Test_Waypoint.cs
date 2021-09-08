/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Test_Waypoint : MonoBehaviour
{
#region Fields
    [ Header( "Setup" ) ]
    [ SerializeField  ] protected Test_Waypoint nextWaypoint;
	[ SerializeField  ] protected Vector3 targetPosition;
	[ SerializeField  ] protected float wide;

    public Vector3 TargetPosition => transform.TransformPoint( targetPosition );
    public Test_Waypoint NextWaypoint => nextWaypoint;
    public float Wide => wide;

#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public abstract Vector3 ApproachMethod( Vector3 position, float speed);
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
