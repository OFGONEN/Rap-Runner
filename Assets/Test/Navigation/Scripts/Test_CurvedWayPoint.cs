/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes;

public class Test_CurvedWayPoint : Test_Waypoint
{
#region Fields
    public Vector3 turnOrigin;
    public Test_Curve curve;
    public float turn_SpeedModifier = 1f;

    private float turnModifier;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
        if( curve == Test_Curve.LeftCurve )
			turnModifier = -1f;
        else if ( curve == Test_Curve.RightCurve )
			turnModifier = 1f;
        
		targetPosition = turnOrigin + transform.forward * turnOrigin.x;
	}
#endregion

#region API
    public override Vector3 ApproachMethod( Transform targetTransform, float speed )
    {
		targetTransform.RotateAround( transform.TransformPoint( turnOrigin ), Vector3.up, Time.deltaTime * speed * turnModifier * turn_SpeedModifier );

		return targetTransform.position;
	}
#endregion

#region Implementation
#endregion

    public enum Test_Curve
    {
        LeftCurve,
        RightCurve
    }

#region Editor Only
#if UNITY_EDITOR
    [ Header( "EditorOnly" ), HorizontalLine ]
    public Vector3 sewingPoint;

    private void OnDrawGizmos()
    {
		var startPosition = transform.position;
		var middlePoint = transform.TransformPoint( new Vector3( turnOrigin.x - turnOrigin.x * Mathf.Cos( 45 ), 0, turnOrigin.x * Mathf.Sin( 45 ) ) );
		var middlePoint_Up = middlePoint + Vector3.up * 2f;
		var turnOrigin_World = transform.TransformPoint( turnOrigin );
		var turnOrigin_World_Up = turnOrigin_World + Vector3.up * 2f;

		var targetPoint_Up = TargetPosition + Vector3.up * 0.5f;

		if ( nextWaypoint == null )
			Handles.color = Color.red;
		else 
			Handles.color = Color.green;

		Handles.DrawSolidDisc( startPosition, Vector3.up, 0.1f );
		Handles.DrawSolidDisc( TargetPosition, Vector3.up, 0.1f );

		Handles.DrawLine( middlePoint, middlePoint_Up );
		Handles.DrawSolidDisc( middlePoint, Vector3.up, 0.05f );
		Handles.DrawSolidDisc( middlePoint_Up, Vector3.up, 0.05f );
		Handles.Label( middlePoint_Up , "Curved Rode Wide: " + wide );

		Handles.DrawWireArc( turnOrigin_World, Vector3.up, -transform.right, 90, turnOrigin.x );
		Handles.DrawWireArc( turnOrigin_World, Vector3.up, -transform.right, 90, turnOrigin.x + wide / 2f );

		Handles.color = Color.blue;
		Handles.DrawSolidDisc( turnOrigin_World, Vector3.up, 0.1f );
		Handles.DrawWireArc( turnOrigin_World, Vector3.up, -transform.right, 90f, turnOrigin.x - wide / 2f );
		Handles.DrawLine( turnOrigin_World, turnOrigin_World_Up );
		Handles.DrawSolidDisc( turnOrigin_World_Up, Vector3.up, 0.05f );
		Handles.Label( turnOrigin_World_Up, "Curved Rode Turning Wide: " + ( turnOrigin.x - wide / 2f ) );

		sewingPoint = turnOrigin_World + transform.forward * ( turnOrigin.x - wide / 2f );
		var sewingPoint_Up = sewingPoint + Vector3.up * 0.5f;
		Handles.DrawSolidDisc( sewingPoint, Vector3.up, 0.05f );
		Handles.DrawSolidDisc( targetPoint_Up , Vector3.up, 0.05f );
		Handles.DrawLine( TargetPosition, targetPoint_Up );
		Handles.Label( targetPoint_Up , "Sewing Point \n" + TargetPosition );

	}

    [ Button() ]
    public void CalculateTargetPosition()
    {
		targetPosition = turnOrigin + transform.forward * turnOrigin.x;
	}
#endif
#endregion
}
