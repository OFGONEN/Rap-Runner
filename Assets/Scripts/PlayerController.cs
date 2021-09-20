/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
#region Fields
    [ Header( "Event Listeners" ) ]
    public EventListenerDelegateResponse levelStartListener;
	public EventListenerDelegateResponse modifierEventListener;
	public EventListenerDelegateResponse catwalkEventListener;

	[ Header( "Fired Events" ) ]
	public GameEvent levelCompleteEvent;
	public GameEvent levelFailEvent;

	[ Header( "Shared Variables" ) ]
    public SharedFloatProperty inputDirectionProperty;
	public SharedReferenceProperty startWaypointReference;


    [ BoxGroup( "Setup" ) ] public Transform modelTransform;
    [ BoxGroup( "Setup" ) ] public Status currentStatus;
	//TODO:(ofg) private float statusPoint_Ratio; probably SharedFloatTweener for WorldUI to use

	// Private Fields \\
	private Waypoint currentWaypoint;
	private Obstacle currentObstacle;
	private float modelRotationAmount;

	// Status releated
	private float statusPoint_Current;
	private float statusDepleteSpeed;
	private float statusPoint_Floor;
	private float statusPoint_Ceil;

	private bool transformAfterSequence = false;
	private bool catwalking = false;

	// Delegates
	private UnityMessage updateMethod;
	private UnityMessage startApproachMethod;
	private Sequence obstacleInteractonSequence;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelStartListener.OnEnable();
		modifierEventListener.OnEnable();
		catwalkEventListener.OnEnable();
	}

    private void OnDisable()
    {
		levelStartListener.OnDisable();
		modifierEventListener.OnDisable();
		catwalkEventListener.OnDisable();
    }
    
    private void Awake()
    {
		// Set Delegates
		levelStartListener.response    = LevelStartResponse;
		modifierEventListener.response = ModifierEventResponse;
		updateMethod                   = ExtensionMethods.EmptyMethod;
		catwalkEventListener.response  = CatwalkEventResponse;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
    public void StartApproachWaypoint()
    {
		startApproachMethod = StartApproachWaypoint;

		if( currentWaypoint != null )
			updateMethod = ApproachWaypointMethod;
	}

    public void StartApproach_DepletingWaypoint()
    {
		startApproachMethod = StartApproach_DepletingWaypoint;

        if( currentWaypoint != null )
			updateMethod = Approach_DepletingWaypointMethod;
    }

	public void StartApproachObstacle( Obstacle obstacle )
	{
		currentObstacle = obstacle;
		updateMethod    = ApproachObstacleMethod;
	}
#endregion

#region Implementation
    private void LevelStartResponse()
    {
		currentWaypoint = startWaypointReference.sharedValue as Waypoint;
		currentWaypoint.PlayerEntered( this );

		statusPoint_Floor = 0;
		statusPoint_Ceil = currentStatus.status_Point;
	}

    private void ModifierEventResponse()
    {
		var modifyAmount = ( modifierEventListener.gameEvent as FloatGameEvent ).eventValue;

		var transform = ModifyStatus( modifyAmount );

		if( statusPoint_Current < 0 )
		{
			LevelComplete( levelFailEvent );
		}
		else if ( transform ) 
		{
			FFLogger.Log( "Transform the player" );
			//TODO:(ofg): Transform the player
		}
	}

	private void CatwalkEventResponse()
	{
		catwalking = true;
	}

    private void ApproachWaypointMethod()
    {
		var position = transform.position;

		var approachDistance = currentWaypoint.ApproachMethod( transform );

        if( Vector3.Distance( approachDistance, currentWaypoint.TargetPoint ) <= GameSettings.Instance.player_target_checkDistance )
        {
			currentWaypoint.PlayerExited( this );

            if( currentWaypoint.NextWaypoint != null )
            {

				currentWaypoint = currentWaypoint.NextWaypoint;
				currentWaypoint.PlayerEntered( this );
			}
            else
            {
				updateMethod = ExtensionMethods.EmptyMethod;
				return;
			}
		}
        else
			transform.position = approachDistance;

		// Move GFX Object

		Vector3 horizontalMove = Vector3.right * inputDirectionProperty.sharedValue;
		modelRotationAmount = Mathf.Lerp( modelRotationAmount, 
                                inputDirectionProperty.sharedValue * GameSettings.Instance.player_clamp_rotation, 
                                Time.deltaTime * GameSettings.Instance.player_speed_turning );

        // Calculate new local position for model
		var modelPosition = modelTransform.localPosition;
		var model_NewPosition = Vector3.MoveTowards( modelPosition, modelPosition + horizontalMove, Time.deltaTime * GameSettings.Instance.player_speed_horizontal );

		model_NewPosition.x = Mathf.Clamp( model_NewPosition.x, -currentWaypoint.Wide / 2f, currentWaypoint.Wide / 2f );
		modelTransform.localPosition = model_NewPosition;

		// Calculate new local rotation for model
		modelTransform.localRotation = Quaternion.Euler( 0, modelRotationAmount, 0 );
	}

    private void Approach_DepletingWaypointMethod()
    {
		var lossStatus = Time.deltaTime * GameSettings.Instance.player_speed_statusDepleting;

		var transform = ModifyStatus( -lossStatus );
		//TODO:(ofg) handle status type

		if( statusPoint_Current < 0 )
		{
			if( catwalking )
				LevelComplete( levelCompleteEvent );
			else 
				LevelComplete( levelFailEvent );
		}
		else if ( !catwalking && transform ) 
		{
			FFLogger.Log( "Transform the player" );
			//TODO:(ofg): Transform the player
		}

		ApproachWaypointMethod();
	}

	private void ApproachObstacleMethod()
	{
		var position                   = transform.position;
		var position_gfx               = modelTransform.localPosition;
		var targetPosition             = currentObstacle.transform.position + currentObstacle.TargetDistance;
		var model_targetPosition       = currentObstacle.TargetPoint;
		var model_TargetPosition_Local = transform.InverseTransformPoint( model_targetPosition );

		if( Vector3.Distance( modelTransform.position.SetY( 0 ), model_targetPosition ) <= 0.1f )
		{
			FFLogger.Log( "Target Approached" );
			updateMethod = ExtensionMethods.EmptyMethod;

			position_gfx.x               = model_TargetPosition_Local.x;
			position_gfx.z               = 0;
			modelTransform.localPosition = position_gfx;

			modelTransform.LookAtAxis( currentObstacle.LookTargetPoint, Vector3.up );
			StartObstacleSequence();
			return;
		}

		var newPosition = Vector3.MoveTowards( position, 
							targetPosition, 
							Time.deltaTime * GameSettings.Instance.player_speed_approach );

		var newPosition_GFX_X = Mathf.MoveTowards( position_gfx.x, 
									model_TargetPosition_Local.x, 
									Time.deltaTime * GameSettings.Instance.player_speed_horizontal );

		position_gfx.x               = newPosition_GFX_X;
		transform.position           = newPosition;
		modelTransform.localPosition = position_gfx;

		modelTransform.LookAtOverTimeAxis( currentObstacle.LookTargetPoint, Vector3.up, GameSettings.Instance.player_speed_turning );
	}

	private void StartObstacleSequence()
	{
		var duration = GameSettings.Instance.player_duration_obstacleInteraction;
		statusDepleteSpeed = Mathf.Min( statusPoint_Current, currentObstacle.StatusPoint ) / duration;

		obstacleInteractonSequence = DOTween.Sequence();

		obstacleInteractonSequence.Append( transform.DOMove( transform.position + currentObstacle.RappingDistance, duration ) );
		obstacleInteractonSequence.Join( currentObstacle.StartRapping( duration ) );

		obstacleInteractonSequence.OnUpdate( OnSequenceUpdate );

		if( statusPoint_Current > currentObstacle.StatusPoint )
			obstacleInteractonSequence.OnComplete( OnSequenceComplete_Win );
		else
			obstacleInteractonSequence.OnComplete( OnSequenceComplete_Lost );

	}

	private void OnSequenceUpdate()
	{
		//TODO:(ofg) handle status type
		var lossStatus = Time.deltaTime * statusDepleteSpeed;

		transformAfterSequence = ModifyStatus( -lossStatus );
		currentObstacle.StatusPoint -= lossStatus;
	}

	private void OnSequenceComplete_Win()
	{
		obstacleInteractonSequence.Kill();
		obstacleInteractonSequence = null;

		if( transformAfterSequence )
		{
			FFLogger.Log( "Transform Player" );
			//TODO:(ofg) transform player etc.
			transformAfterSequence = false;
		}

		currentObstacle.Rapping_Lost();

		startApproachMethod();
	}

	private void OnSequenceComplete_Lost()
	{
		obstacleInteractonSequence.Kill();
		obstacleInteractonSequence = null;

		currentObstacle.Rapping_Won();
		LevelComplete( levelFailEvent );
	}

	private bool ModifyStatus( float modifyAmount )
	{
		bool transform = false;
		var newStatusPoint = statusPoint_Current + modifyAmount;

		if( newStatusPoint < statusPoint_Floor && currentStatus.prevStatus != null )
		{
			statusPoint_Floor = statusPoint_Ceil;
			statusPoint_Ceil  = statusPoint_Ceil - currentStatus.prevStatus.status_Point;
			currentStatus     = currentStatus.prevStatus;

			transform = true;
		}
		else if( newStatusPoint >= statusPoint_Ceil && currentStatus.nextStatus != null )
		{
			statusPoint_Floor = statusPoint_Ceil;
			statusPoint_Ceil  = statusPoint_Ceil + currentStatus.nextStatus.status_Point;
			currentStatus     = currentStatus.nextStatus;

			transform = true;
		}

		    statusPoint_Current = newStatusPoint;
		var ratio               = statusPoint_Current / GameSettings.Instance.status_maxPoint;

		return transform;
	}

	private void LevelComplete( GameEvent completeEvent )
	{
		completeEvent.Raise();
		updateMethod = ExtensionMethods.EmptyMethod;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.Label( transform.position.AddUp( 2.5f ), currentStatus.status_Name + ": " + Mathf.CeilToInt( statusPoint_Current ) );
	}
#endif
#endregion
}
