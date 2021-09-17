/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingModifier : Modifier
{
#region Fields
#endregion

#region Properties
#endregion

#region Unity API
    protected override void OnTriggerEnter( Collider other )
    {
		base.OnTriggerEnter( other );

		gameObject.SetActive( false );
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
