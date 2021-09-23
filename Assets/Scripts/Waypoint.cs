/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public abstract class Waypoint : MonoBehaviour
{
#region Fields
    [ Header( "Fired Events" ) ]
    [ SerializeField ] protected GameEvent player_EnteredEvent; 
    [ SerializeField ] protected GameEvent player_ExitedEvent; 

    [ BoxGroup( "Setup" ), SerializeField ] protected Waypoint nextWaypoint;
    [ BoxGroup( "Setup" ), SerializeField ] protected Vector3 targetPoint;
    [ BoxGroup( "Setup" ), SerializeField ] protected float wide;

	// Properties \\
	public Waypoint NextWaypoint => nextWaypoint;
	public Vector3 TargetPoint => targetPoint_WorldPosition;
	public float Wide => wide;

	// Private \\
	protected Vector3 targetPoint_WorldPosition;
#endregion

#region Properties
#endregion

#region Unity API
    protected virtual void Awake()
    {
        // Cache the targetPoint's position as World position
		targetPoint_WorldPosition = transform.TransformPoint( targetPoint );
	}
#endregion

#region API
    public abstract Vector3 ApproachMethod( Transform targetTransform );

    public virtual void PlayerEntered( PlayerController player ) 
    {
        // If not null
		player_EnteredEvent?.Raise();
		player.StartApproachWaypoint();
	}

    public virtual void PlayerExited( PlayerController player )
    {
        // If not null
        player_ExitedEvent?.Raise();
	}

#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    public Vector3 Editor_TargetPoint()
    {
		return transform.TransformPoint( targetPoint );
    }
    
    public void Editor_SetNextWaypoint( Waypoint waypoint )
    {
		nextWaypoint = waypoint;
	}
#endif
#endregion
}
