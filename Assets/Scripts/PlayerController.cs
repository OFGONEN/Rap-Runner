/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour
{
#region Fields
    [ Header( "Event Listeners" ) ]
    public EventListenerDelegateResponse levelStartListener;
    [ Header( "Shared Variables" ) ]
    public SharedFloatProperty inputDirectionProperty;
	public SharedReferenceProperty startWaypointReference;

    [ BoxGroup( "Setup" ) ] public Transform modelTransform;

    // Private Fields \\
    private Waypoint currentWaypoint;

	private float modelRotationAmount;

	// Delegates
	private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelStartListener.OnEnable();
	}

    private void OnDisable()
    {
		levelStartListener.OnDisable();
    }
    
    private void Awake()
    {
        // Set Delegates
		levelStartListener.response = LevelStartResponse;
		updateMethod                = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
    public void StartApproachWaypoint()
    {
        if( currentWaypoint != null )
			updateMethod = ApproachWaypointMethod;
	}

    public void StartApproach_DepletingWaypoint()
    {
        if( currentWaypoint != null )
			updateMethod = Approach_DepletingWaypointMethod;
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
		modelRotationAmount = Mathf.MoveTowards( modelRotationAmount, 
                                inputDirectionProperty.sharedValue * GameSettings.Instance.player_clamp_rotation, 
                                Time.deltaTime * GameSettings.Instance.player_speed_rotation );

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

    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
