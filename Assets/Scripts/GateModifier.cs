/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateModifier : Modifier
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
    protected override void TriggerEnter( Collider other )
    {
		base.TriggerEnter( other );
		modiferCollider.enabled = false;
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
