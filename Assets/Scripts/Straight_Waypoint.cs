/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;

public class Straight_Waypoint : Waypoint
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
		return Vector3.MoveTowards( targetTransform.position, targetPoint_WorldPosition, Time.deltaTime * speed );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		var targetPoint = transform.TransformPoint( this.targetPoint );

		if( nextWaypoint == null )
		{
			var leftPoint = this.targetPoint / 2f + Vector3.left * wide / 2f;
			var rightPoint = this.targetPoint / 2f + Vector3.right * wide / 2f;

			leftPoint = transform.TransformPoint( leftPoint );
			rightPoint = transform.TransformPoint( rightPoint );
			var middlePoint = ( leftPoint + rightPoint ) / 2f;

			var targetPoint_Up = targetPoint.AddUp( 0.5f );
			var middlePoint_Up = middlePoint.AddUp( 1f );

			Handles.color = Color.blue;

			Handles.DrawLine( leftPoint, rightPoint, 1f );
			Handles.DrawSolidDisc( leftPoint, Vector3.up, 0.1f );
			Handles.DrawSolidDisc( rightPoint, Vector3.up, 0.1f );
			Handles.DrawSolidDisc( middlePoint_Up, Vector3.up, 0.05f );

			Handles.DrawLine( middlePoint, middlePoint.AddUp( 2f ) );
			Handles.Label( middlePoint_Up, "Straight Road Wide: " + wide );

			Handles.DrawLine( targetPoint, targetPoint_Up );
			Handles.DrawSolidDisc( targetPoint_Up, Vector3.up, 0.05f );

			Handles.Label( targetPoint_Up, "Sewing Point \n" + targetPoint );

			Handles.color = Color.red;

			Handles.DrawSolidDisc( transform.position, Vector3.up, 0.1f );
			Handles.DrawSolidDisc( targetPoint, Vector3.up, 0.1f );

			Handles.DrawLine( transform.position, targetPoint );
			Handles.DrawLine( middlePoint_Up, middlePoint.AddUp( 2f ) );
			Handles.DrawSolidDisc( middlePoint.AddUp( 2f ), Vector3.up, 0.05f );
			Handles.Label( middlePoint.AddUp( 2f ), "Straight Road Lenght: " + Vector3.Distance( transform.position, targetPoint ) );
		}
		else 
		{
			Handles.color = Color.green;
			Handles.DrawLine( transform.position, targetPoint );
		}
	}

	private void OnDrawGizmosSelected()
	{
		GUIStyle style = new GUIStyle();
		style.fontSize = 15;
		style.normal.textColor = Color.red;
		style.fontStyle = FontStyle.Bold;

		var center = transform.position + transform.forward * targetPoint.z / 2f;
		Handles.Label( center.AddUp( 1.5f ), " Center: " + center, style );
	}
#endif
#endregion
}
