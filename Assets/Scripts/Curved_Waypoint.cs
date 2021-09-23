/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using NaughtyAttributes;


public class Curved_Waypoint : Waypoint
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] protected Vector3 turnOrigin;

	// Private \\
	private Vector3 turnOrigin_WorldPosition;
	private float turnModifier;
#endregion

#region Properties
#endregion

#region Unity API
    protected override void Awake()
    {
		// Calculate turn modifier according to turn origin position
		turnModifier = Mathf.Sign( turnOrigin.x );

        // Calculate target point local position based on turn origin position
		targetPoint              = turnOrigin + Vector3.forward * Mathf.Abs( turnOrigin.x );
        // Cache turn origin world position
		turnOrigin_WorldPosition = transform.TransformPoint( turnOrigin );

        // Base class calculates target point world position
		base.Awake();
	}
#endregion

#region API
    public override Vector3 ApproachMethod( Transform targetTransform )
    {
		targetTransform.RotateAround( turnOrigin_WorldPosition, Vector3.up, Time.deltaTime * GameSettings.Instance.player_speed_rotation * turnModifier );

		return targetTransform.position;
	}
	#endregion

	#region Implementation
	#endregion

	#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		var sign = Mathf.Sign( turnOrigin.x );
		var absolute = Mathf.Abs( turnOrigin.x );
		var startPosition = transform.position;

		var targetPosition = transform.TransformPoint( turnOrigin + Vector3.forward * Mathf.Abs( turnOrigin.x ) );
		var middlePoint = transform.TransformPoint( new Vector3( turnOrigin.x - turnOrigin.x * Mathf.Cos( 45 ), 0, absolute * Mathf.Sin( 45 ) ) );
		var middlePoint_Up = middlePoint.AddUp( 2f );
		var turnOrigin_World = transform.TransformPoint( turnOrigin );
		var turnOrigin_World_Up = turnOrigin_World.AddUp( 2f );

		var targetPoint_Up = targetPosition.AddUp( 0.5f );

		if( nextWaypoint == null )
			Handles.color = Color.red;
		else
			Handles.color = Color.green;

		Handles.DrawSolidDisc( startPosition, Vector3.up, 0.1f );
		Handles.DrawSolidDisc( targetPosition, Vector3.up, 0.1f );

		Handles.DrawLine( middlePoint, middlePoint_Up );
		Handles.DrawSolidDisc( middlePoint, Vector3.up, 0.05f );
		Handles.DrawSolidDisc( middlePoint_Up, Vector3.up, 0.05f );
		Handles.Label( middlePoint_Up, "Curved Road Wide: " + wide );

		Handles.DrawWireArc( turnOrigin_World, Vector3.up, sign * -transform.right, sign * 90, absolute );
		Handles.DrawWireArc( turnOrigin_World, Vector3.up, sign * -transform.right, sign * 90, absolute + wide / 2f );

		Handles.color = Color.blue;
		Handles.DrawSolidDisc( turnOrigin_World, Vector3.up, 0.05f );
		Handles.DrawWireArc( turnOrigin_World, Vector3.up, sign * -transform.right, sign * 90f, absolute - wide / 2f );
		Handles.DrawLine( turnOrigin_World, turnOrigin_World_Up );
		Handles.DrawSolidDisc( turnOrigin_World_Up, Vector3.up, 0.05f );
		Handles.Label( turnOrigin_World_Up, "Curved Road Turning Wide: " + ( absolute - wide / 2f ) );

		Handles.DrawSolidDisc( targetPosition, Vector3.up, 0.05f );
		Handles.DrawSolidDisc( targetPoint_Up, Vector3.up, 0.05f );
		Handles.DrawLine( targetPosition, targetPoint_Up );
		Handles.Label( targetPoint_Up, "Sewing Point \n" + targetPosition );
	}

	private void OnValidate()
	{
		targetPoint = turnOrigin + Vector3.forward * Mathf.Abs( turnOrigin.x );
	}

	public Vector3 Editor_TurnOrigin()
	{
		return turnOrigin;
	}
#endif
#endregion
}
