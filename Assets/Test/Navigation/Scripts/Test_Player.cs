/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Test_Player : MonoBehaviour
{
#region Fields
    public Test_Waypoint waypoint;
    public float speed_Move_Vertical;
    public float speed_Move_Horizontal;
    
    public Transform gfx_Transform;

	public bool moveOnStart = false;

	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
        updateMethod = ExtensionMethods.EmptyMethod;

        if( moveOnStart )
		    updateMethod = ApproachWaypoint;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
#endregion

#region Implementation

    private void ApproachWaypoint()
    {
		var position = transform.position;

		var approachDistance = waypoint.ApproachMethod( transform, speed_Move_Vertical );

        if( Vector3.Distance( approachDistance, waypoint.TargetPosition ) <= 0.1f)
        {
            if( waypoint.NextWaypoint != null )
				waypoint = waypoint.NextWaypoint;
            else
            {
				updateMethod = ExtensionMethods.EmptyMethod;
				return;
			}
		}
        else
			transform.position = approachDistance;

		// Move GFX Object

		Vector3 horizontalMove = Vector3.zero;

		if( Input.GetKey( KeyCode.RightArrow ) ) 
			horizontalMove += Vector3.right;
		else if( Input.GetKey( KeyCode.LeftArrow ) ) 
			horizontalMove += Vector3.left;

		var gfx_Position = gfx_Transform.localPosition;
		var nextPosition = Vector3.MoveTowards( gfx_Position, gfx_Position + horizontalMove, Time.deltaTime * speed_Move_Horizontal );

		nextPosition.x = Mathf.Clamp( nextPosition.x, -waypoint.Wide / 2f, waypoint.Wide / 2f );

		gfx_Transform.localPosition = nextPosition;
	}

#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
