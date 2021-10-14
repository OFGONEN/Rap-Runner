/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straight_Depleting_Waypoint : Straight_Waypoint
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
    public override void PlayerEntered( PlayerController player )
    {
        // If not null
		player_EnteredEvent?.Raise();
		player.StartApproach_DepletingWaypoint();
	}
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
