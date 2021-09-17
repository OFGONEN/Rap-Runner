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
    protected override void OnTriggerEnter( Collider other )
    {
		base.OnTriggerEnter( other );
		modiferCollider.enabled = false;
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
