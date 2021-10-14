/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GateModifier : Modifier
{
#region Fields
	public GateModifier pairGate;
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
		pairGate.DisableCollider();
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if( pairGate == null )
		{
			GUIStyle style = new GUIStyle();
			style.fontSize = 25;
			style.normal.textColor = Color.red;
			style.fontStyle = FontStyle.Bold;
			Handles.Label( transform.position + Vector3.up * 5, "GATE IS NOT PAIRED", style );
		}
	}
#endif
#endregion
}
