/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using NaughtyAttributes;

public class Test_Obstacle : MonoBehaviour
{
#region Fields
	public float statusPoint;

	[ HorizontalLine ]
    [ SerializeField ] private Vector3 targetPoint;
    [ SerializeField ] private Vector3 rapping_targetPoint;

	public Vector3 TargetPoint => transform.TransformPoint( targetPoint );
	public Vector3 RappingDistance => rapping_targetPoint;

	[ SerializeField ] private BoxCollider boxCollider;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnTriggerEnter( Collider other )
    {
		var player = other.GetComponentInParent< Test_Player >();

		boxCollider.enabled = false;
		player.StartApproachObstacle( this );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos() 
    {
		var position = transform.position;
		var targetPosition = transform.TransformPoint( targetPoint );
		var rapping_targetPosition = transform.TransformPoint( rapping_targetPoint );

		Handles.color = Color.blue;
		Handles.Label( targetPosition.AddUp( 0.5f ), "Approach Point:\n" + targetPosition );

		Handles.DrawDottedLine( position.AddUp( 0.1f ), targetPosition.AddUp( 0.1f ), 1f );
		Handles.DrawWireDisc( targetPosition.AddUp( 0.1f ), Vector3.up, 0.1f );

		Handles.color = Color.red;

		Handles.Label( rapping_targetPosition.AddUp( 0.5f ), "Rapping Point:\n" + rapping_targetPosition );
		Handles.DrawWireCube( transform.TransformPoint( boxCollider.center ), boxCollider.size );
		Handles.DrawDottedLine( position.AddUp( 0.1f ), rapping_targetPosition.AddUp( 0.1f ), 1f );
		Handles.DrawWireDisc( rapping_targetPosition.AddUp( 0.1f ), Vector3.up, 0.1f );

		Handles.Label( transform.position.AddUp( 1.5f ), "Status: " + Mathf.CeilToInt( statusPoint ) );
	}
#endif
#endregion
}
