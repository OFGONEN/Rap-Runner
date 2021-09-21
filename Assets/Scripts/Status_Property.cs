/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "SharedStatusProperty", menuName = "FF/Game/SharedStatusProperty" ) ]
public class Status_Property : ScriptableObject
{
#region Fields
    public string status_Name;
    public Color status_Color;

    public event ChangeEvent changeEvent;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API

	public void SetValue( string name, Color color )
	{
		status_Name  = name;
		status_Color = color;

        changeEvent?.Invoke();
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}