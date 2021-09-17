/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using NaughtyAttributes;

public class Obstacle : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] public float statusPoint;
    [ BoxGroup( "Setup" ), SerializeField ] public Vector3 targetPosition; // Local position
    [ BoxGroup( "Setup" ), SerializeField ] public Vector3 rappingPosition; // Local position

	// Private Fields \\
	private Vector3 targetPosition_WorldPoint;

    // Components
    private ColliderListener_EventRaiser colliderListener;
    private BoxCollider boxCollider;
#endregion

#region Properties
	// Properties \\
	public Vector3 TargetPoint => targetPosition_WorldPoint;
    public Vector3 RappingDistance => rappingPosition;
#endregion

#region Unity API
    private void OnEnable()
    {
		colliderListener.triggerEnter += TriggerEnter;
	}

    private void OnDisable()
    {
		colliderListener.triggerEnter -= TriggerEnter;
    }

    private void Awake()
    {
        // Cache world position of target position
		targetPosition_WorldPoint = transform.TransformPoint( targetPosition );

		colliderListener = GetComponentInChildren< ColliderListener_EventRaiser >();
		boxCollider      = GetComponentInChildren< BoxCollider >();
	}
#endregion

#region API
    public void Rapping_Won()
    {
        //TODO:(ofg) Obstacle plays victory animation
    }

    public void Rapping_Lost()
    {
        //TODO:(ofg) Enable ragdoll object
    }
#endregion

#region Implementation
    private void TriggerEnter( Collider other )
    {
		var player = other.GetComponentInParent< PlayerController >();

		boxCollider.enabled = false;
        //TODO:(ofg) player.StartApproacObstacle
	}
	#endregion

	#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
        if( boxCollider == null )
			boxCollider = GetComponentInChildren< BoxCollider >();

		var position = transform.position;
		var targetPosition = transform.TransformPoint( this.targetPosition );
		var rappingPosition = transform.TransformPoint( this.rappingPosition );

		Handles.color = Color.blue;
		Handles.Label( targetPosition.AddUp( 0.5f ), "Approach Point:\n" + targetPosition );

		Handles.DrawDottedLine( position.AddUp( 0.1f ), targetPosition.AddUp( 0.1f ), 1f );
		Handles.DrawWireDisc( targetPosition.AddUp( 0.1f ), Vector3.up, 0.1f );

		Handles.color = Color.red;

		Handles.Label( rappingPosition.AddUp( 0.5f ), "Rapping Point:\n" + rappingPosition );
		Handles.DrawWireCube( transform.TransformPoint( boxCollider.center ), boxCollider.size );
		Handles.DrawDottedLine( position.AddUp( 0.1f ), rappingPosition.AddUp( 0.1f ), 1f );
		Handles.DrawWireDisc( rappingPosition.AddUp( 0.1f ), Vector3.up, 0.1f );

		Handles.Label( transform.position.AddUp( 1f ), "Status: " + Mathf.CeilToInt( statusPoint ) );
	}
#endif
#endregion
}
