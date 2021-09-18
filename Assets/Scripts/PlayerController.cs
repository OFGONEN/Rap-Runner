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

	[ Header( "Fired Events" ) ]
	public GameEvent levelFailedEvent;

	[ Header( "Shared Variables" ) ]
    public SharedFloatProperty inputDirectionProperty;
	public SharedReferenceProperty startWaypointReference;


    [ BoxGroup( "Setup" ) ] public Transform modelTransform;

    // Private Fields \\
    private Waypoint currentWaypoint;
	private Obstacle currentObstacle;
	private float modelRotationAmount;

	// Status releated
	private float statusPoint;
	private float statusDepleteSpeed;


	// Delegates
	private UnityMessage updateMethod;
	private UnityMessage approachMethod;
	private Sequence obstacleInteractonSequence;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelStartListener.OnEnable();
		modifierEventListener.OnEnable();
	}

    private void OnDisable()
    {
		levelStartListener.OnDisable();
		modifierEventListener.OnDisable();
    }
    
    private void Awake()
    {
		// Set Delegates
		levelStartListener.response    = LevelStartResponse;
		modifierEventListener.response = ModifierEventResponse;
		updateMethod                   = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
    public void StartApproachWaypoint()
    {
		approachMethod = StartApproachWaypoint;

		if( currentWaypoint != null )
			updateMethod = ApproachWaypointMethod;
	}

    public void StartApproach_DepletingWaypoint()
    {
		approachMethod = StartApproach_DepletingWaypoint;

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
	}

    private void ApproachWaypointMethod()
    {
		var position = transform.position;

		var approachDistance = currentWaypoint.ApproachMethod( transform );

        if( Vector3.Distance( approachDistance, currentWaypoint.TargetPoint ) <= GameSettings.Instance.player_target_checkDistance )
        {
            if( currentWaypoint.NextWaypoint != null )
            {
				currentWaypoint.PlayerExited( this );

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
		var lossStatus   = Time.deltaTime * GameSettings.Instance.player_speed_statusDepleting;
		    statusPoint -= lossStatus;

		//TODO:(ofg) handle status type

		if( statusPoint <= 0 )
		{
			updateMethod = ExtensionMethods.EmptyMethod;
			levelFailedEvent.Raise();
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
		statusDepleteSpeed = Mathf.Min( statusPoint, currentObstacle.StatusPoint ) / duration;

		obstacleInteractonSequence = DOTween.Sequence();

		obstacleInteractonSequence.Append( transform.DOMove( transform.position + currentObstacle.RappingDistance, duration ) );
		obstacleInteractonSequence.Join( currentObstacle.StartRapping( duration ) );

		obstacleInteractonSequence.OnUpdate( OnSequenceUpdate );

		if( statusPoint > currentObstacle.StatusPoint )
			obstacleInteractonSequence.OnComplete( OnSequenceComplete_Win );
		else
			obstacleInteractonSequence.OnComplete( OnSequenceComplete_Lost );

	}

	private void OnSequenceUpdate()
	{
		//TODO:(ofg) handle status type
		var lossStatus = Time.deltaTime * statusDepleteSpeed;

		statusPoint -= lossStatus;
		currentObstacle.StatusPoint -= lossStatus;
	}

	private void OnSequenceComplete_Win()
	{
		obstacleInteractonSequence.Kill();
		obstacleInteractonSequence = null;

		FFLogger.Log( "Sequence Won" );
		statusPoint = Mathf.CeilToInt( statusPoint );

		//TODO:(ofg) transform player etc.
		currentObstacle.Rapping_Lost();

		approachMethod();
	}

	private void OnSequenceComplete_Lost()
	{
		obstacleInteractonSequence.Kill();
		obstacleInteractonSequence = null;

		FFLogger.Log( "Sequence Lost" );
		currentObstacle.Rapping_Won();
		//TODO:(ofg) Player lost 
	}

    private void ModifierEventResponse()
    {
		var modifyAmount = ( modifierEventListener.gameEvent as FloatGameEvent ).eventValue;
		statusPoint += modifyAmount;

		//TODO:(ofg) level fails when status is lower than 0
		statusPoint = Mathf.Max( statusPoint, 0 );

		FFLogger.Log( "Status Point: " + statusPoint );
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
