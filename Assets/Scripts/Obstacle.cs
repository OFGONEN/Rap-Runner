/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using DG.Tweening;
using TMPro;
using NaughtyAttributes;

public class Obstacle : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] private float statusPoint;
    [ BoxGroup( "Setup" ), SerializeField ] private Vector3 targetPosition; // Local position
    [ BoxGroup( "Setup" ), SerializeField ] private Vector3 rappingPosition; // Local position
    [ BoxGroup( "Setup" ), Tooltip( "Should Camera transition while rapping" ), SerializeField ] private bool cameraTransition = false; 
    [ BoxGroup( "Setup" ), SerializeField ] private Obstacle[] pairedObstacles; 

	// Private Fields \\
	private Vector3 targetPosition_WorldPoint;
	private Vector3 lookTargetPosition;
	private Color statusColor;
	private float currentStatusPoint;

	// Components
	private ColliderListener_EventRaiser colliderListener;
    private BoxCollider boxCollider;
	private Animator animator;
	private TextMeshProUGUI worldUIText;
#endregion

#region Properties
	// Properties \\
	public Vector3 TargetPoint     => targetPosition_WorldPoint;
	public Vector3 TargetDistance  => transform.forward * targetPosition.z;
	public Vector3 RappingDistance => transform.forward * rappingPosition.z;
	public Vector3 LookTargetPoint => lookTargetPosition;
	public bool CameraTransition   => cameraTransition;

	public float StatusPoint
	{
		get 
		{
			return currentStatusPoint;
		}
		set
		{
			UpdateStatusWorldUIText( value );
		}
	}
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
		colliderListener = GetComponentInChildren< ColliderListener_EventRaiser >();
		boxCollider      = GetComponentInChildren< BoxCollider >();
		animator         = GetComponentInChildren< Animator >();
		worldUIText      = GetComponentInChildren< TextMeshProUGUI >();

		// Cache world position of target position
		targetPosition_WorldPoint = boxCollider.transform.TransformPoint( targetPosition ).SetY( 0 );
		lookTargetPosition        = boxCollider.transform.position.SetY( 0 );

		currentStatusPoint = statusPoint;
		statusColor        = worldUIText.color;
	}
#endregion

#region API
    public void Rapping_Won()
    {

		animator.SetBool( "victory", true );
		animator.SetTrigger( "complete" );
    }

    public void Rapping_Lost()
    {
		worldUIText.enabled = false;

		animator.SetBool( "victory", false );
		animator.SetTrigger( "complete" );
    }

	public Tween StartRapping( float duration )
	{
		RetirePairObstacles();
		boxCollider.enabled = false;

		animator.SetTrigger( "rapping" );
		return transform.DOMove( transform.position + RappingDistance, duration );
	}

	public void RetireObstacle()
	{
		boxCollider.enabled = false;
		worldUIText.enabled = false;
	}
#endregion

#region Implementation
	private void RetirePairObstacles()
	{
		foreach( var obstacle in pairedObstacles )
		{
			obstacle.RetireObstacle();
		}
	}

    private void TriggerEnter( Collider other )
    {
		var player = other.GetComponentInParent< PlayerController >();

		boxCollider.enabled = false;
		player.StartApproachObstacle( this );
	}

	private void UpdateStatusWorldUIText( float newValue )
	{
		var newIntValue = ( int )newValue;

		if( (int)currentStatusPoint != newIntValue )
			worldUIText.color = Color.Lerp( GameSettings.Instance.status_depleted_color, statusColor , currentStatusPoint / statusPoint );

		currentStatusPoint = newValue;
	}
	#endregion

	#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
        if( boxCollider == null )
			boxCollider = GetComponentInChildren< BoxCollider >();

		var position        = boxCollider.transform.position.SetY( 0 );
		var targetPosition  = boxCollider.transform.TransformPoint( this.targetPosition ).SetY( 0 );
		var rappingPosition = boxCollider.transform.TransformPoint( this.rappingPosition ).SetY( 0 );

		Handles.color = Color.blue;
		Handles.Label( targetPosition.AddUp( 0.5f ), "Approach Point:\n" + targetPosition );

		Handles.DrawDottedLine( position.AddUp( 0.1f ), targetPosition.AddUp( 0.1f ), 1f );
		Handles.DrawWireDisc( targetPosition.AddUp( 0.1f ), Vector3.up, 0.1f );

		Handles.color = Color.red;

		Handles.Label( rappingPosition.AddUp( 0.5f ), "Rapping Point:\n" + rappingPosition );
		Handles.DrawWireCube( boxCollider.transform.TransformPoint( boxCollider.center ), boxCollider.size );
		Handles.DrawDottedLine( position.AddUp( 0.1f ), rappingPosition.AddUp( 0.1f ), 1f );
		Handles.DrawWireDisc( rappingPosition.AddUp( 0.1f ), Vector3.up, 0.1f );

		Handles.Label( transform.position.AddUp( 1f ), "Status: " + Mathf.CeilToInt( currentStatusPoint ) );
	}
#endif
#endregion
}
