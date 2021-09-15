/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Test_Obstacle : MonoBehaviour
{
#region Fields
    [ SerializeField ] private Vector3 targetPoint;

	public Vector3 TargetPoint => transform.TransformPoint( targetPoint );

	[ SerializeField ] private BoxCollider boxCollider;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnTriggerEnter( Collider other )
    {
		var player = other.GetComponentInParent< Test_Player >();

		boxCollider.enabled = false;
		player.ApproachObstacle( this );
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

		position.y = 0.1f;
		targetPosition.y = 0.1f;

		Handles.color = Color.blue;
		Handles.DrawDottedLine( position, targetPosition, 1f );
		Handles.DrawWireDisc( targetPosition, Vector3.up, 0.1f );

		Handles.color = Color.red;
		Handles.DrawWireCube( transform.TransformPoint( boxCollider.center ), boxCollider.size );
	}
#endif
#endregion
}
