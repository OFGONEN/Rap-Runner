/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class Test_Player : MonoBehaviour
{
#region Fields
    public Test_Waypoint waypoint; //TODO Use SharedReferenceSetter to obtain the first waypoint of the level
	public Test_Obstacle obstacle;
	public float speed_Move_Vertical;
	public float speed_Move_Approach;
    public float speed_Move_Horizontal;
    public float speed_Rotation;
	public float duration_Rapping;

	[ HorizontalLine ]
	public float statusPoint;

    public Transform gfx_Transform;

	public float rotationValue;
	public bool moveOnStart = false;

	private UnityMessage updateMethod;
	private float gfx_Rotation;

	private float rapSpeed;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {
        updateMethod = ExtensionMethods.EmptyMethod;

        if( moveOnStart )
			StartApproachWaypoint();
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
	public void StartApproachWaypoint()
	{
		if( waypoint != null )
			updateMethod = ApproachWaypointMethod;
	}
	
	public void StartApproachObstacle( Test_Obstacle obstacle)
	{
		//TODO: disable input

		this.obstacle = obstacle;
		updateMethod = ApproachObstacleMethod;
	}
#endregion


#region Implementation
	private void StartRapping()
	{
		//TODO: Start rapping animation 
		rapSpeed   = Mathf.Min( statusPoint, obstacle.statusPoint ) / duration_Rapping;

		var rapping_sequence = DOTween.Sequence();
		rapping_sequence.Append( transform.DOMove( transform.position + obstacle.RappingDistance, duration_Rapping ) );
		rapping_sequence.Join( obstacle.transform.DOMove(  obstacle.transform.position + obstacle.RappingDistance , duration_Rapping ) );

		rapping_sequence.OnUpdate( OnRappingUpdate );

		if( statusPoint > obstacle.statusPoint )
			rapping_sequence.OnComplete( OnRappingDone_Win );
		else 
			rapping_sequence.OnComplete( OnRappingDone_Lost );
	}

	private void OnRappingUpdate()
	{
		//TODO: Reduce status points

		var lossStatus = Time.deltaTime * rapSpeed;

		statusPoint -= lossStatus;
		obstacle.statusPoint -= lossStatus;
	}

	private void OnRappingDone_Win()
	{
		FFLogger.Log( "Rapping Won" );
		statusPoint = Mathf.CeilToInt( statusPoint );
		//TODO: Transform etc.

		StartApproachWaypoint();
	}

	private void OnRappingDone_Lost()
	{
		FFLogger.Log( "Rapping Lost" );
		//TODO: Transform etc.
	}

    private void ApproachObstacleMethod()
	{
		var position            = transform.position;
		var position_gfx        = gfx_Transform.localPosition;
		var targetPosition      = obstacle.TargetPoint;
		var targetPositionLocal = transform.InverseTransformPoint( targetPosition );

		var newPosition       = Vector3.MoveTowards( position, targetPosition, Time.deltaTime * speed_Move_Approach );
		var newPosition_GFX_X = Mathf.Lerp( position_gfx.x, targetPositionLocal.x, Time.deltaTime * speed_Move_Horizontal );

		position_gfx.x = newPosition_GFX_X;

		transform.position          = newPosition;
		gfx_Transform.localPosition = position_gfx;

		gfx_Transform.LookAtOverTimeAxis( obstacle.transform.position, Vector3.up, speed_Rotation );

		if( Vector3.Distance( newPosition, targetPosition) <= 0.1f )
		{
			FFLogger.Log( "Target Approached" );
			updateMethod = ExtensionMethods.EmptyMethod;
			StartRapping();
		}
	}

    private void ApproachWaypointMethod()
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
		{
			horizontalMove += Vector3.right;
			gfx_Rotation = Mathf.Lerp( gfx_Rotation, rotationValue, Time.deltaTime * speed_Rotation );
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) ) 
		{ 
			horizontalMove += Vector3.left;
			gfx_Rotation = Mathf.Lerp( gfx_Rotation, -rotationValue, Time.deltaTime * speed_Rotation );
		}
		else 
		{ 
			gfx_Rotation = Mathf.Lerp( gfx_Rotation, 0, Time.deltaTime * speed_Rotation );
		}

		gfx_Transform.localRotation = Quaternion.Euler( 0, gfx_Rotation, 0 );


		var gfx_Position = gfx_Transform.localPosition;
		var nextPosition = Vector3.MoveTowards( gfx_Position, gfx_Position + horizontalMove, Time.deltaTime * speed_Move_Horizontal );

		nextPosition.x = Mathf.Clamp( nextPosition.x, -waypoint.Wide / 2f, waypoint.Wide / 2f );

		gfx_Transform.localPosition = nextPosition;
	}
	
	

#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.Label( transform.position.AddUp( 1.5f ), "Status: " + Mathf.CeilToInt( statusPoint ) );
	}
#endif
#endregion
}
