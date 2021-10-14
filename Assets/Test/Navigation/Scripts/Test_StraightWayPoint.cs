/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test_StraightWayPoint : Test_Waypoint
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    public override Vector3 ApproachMethod( Transform targetTransform, float speed )
    {
		return Vector3.MoveTowards( targetTransform.position, TargetPosition, Time.deltaTime * speed );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		var leftPoint  = targetPosition / 2f + Vector3.left * wide / 2f;
		var rightPoint = targetPosition / 2f + Vector3.right * wide / 2f;

		    leftPoint   = transform.TransformPoint( leftPoint );
		    rightPoint  = transform.TransformPoint( rightPoint );
		var middlePoint = ( leftPoint + rightPoint ) / 2f;

		var targetPoint_Up = TargetPosition + Vector3.up * 0.5f;
		var middlePoint_Up = middlePoint + Vector3.up;

		Handles.color = Color.blue;

		Handles.DrawLine( leftPoint, rightPoint, 1f );
		Handles.DrawSolidDisc( leftPoint, Vector3.up, 0.1f );
		Handles.DrawSolidDisc( rightPoint, Vector3.up, 0.1f );
		Handles.DrawSolidDisc( middlePoint_Up, Vector3.up, 0.05f );

		Handles.DrawLine( middlePoint, middlePoint + Vector3.up * 2f );
		Handles.Label( middlePoint_Up, "Straight Road Wide: " + wide );

		Handles.DrawLine( TargetPosition, targetPoint_Up );
		Handles.DrawSolidDisc( targetPoint_Up, Vector3.up, 0.05f );

		Handles.Label( targetPoint_Up , "Sewing Point \n" + TargetPosition );

		if ( nextWaypoint == null )
			Handles.color = Color.red;
		else 
			Handles.color = Color.green;

		Handles.DrawSolidDisc( transform.position, Vector3.up, 0.1f );
		Handles.DrawSolidDisc( TargetPosition, Vector3.up, 0.1f );

		Handles.DrawLine( transform.position, TargetPosition );
		Handles.DrawLine( middlePoint_Up, middlePoint + Vector3.up * 2f );
		Handles.DrawSolidDisc( middlePoint + Vector3.up * 2f, Vector3.up, 0.05f );
		Handles.Label( middlePoint + Vector3.up * 2f, "Straight Road Lenght: " + Vector3.Distance( transform.position, TargetPosition) );


	}
#endif
#endregion
}
