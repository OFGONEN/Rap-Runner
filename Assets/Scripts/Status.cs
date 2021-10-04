/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[ CreateAssetMenu( fileName = "Status", menuName = "FF/Game/Status" ) ]
public class Status : ScriptableObject
{
#region Fields
    public Status nextStatus;
    public Status prevStatus;
    public Color status_Color;
    public string status_Name;
	public int status_Walking;
	public float status_Point;
#endregion

#region Properties
#endregion

#region Unity API
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
