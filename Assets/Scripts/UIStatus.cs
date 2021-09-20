/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using TMPro;
using NaughtyAttributes;

public class UIStatus : UILoadingBar
{
#region Fields
    [ BoxGroup( "Shared Variables" ) ] public SharedReference playerStatusReference;
	[ BoxGroup( "UI Elements" ) ] public TextMeshProUGUI statusText;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
    protected override void OnValueChange()
    {
		base.OnValueChange();

		var playerStatus = playerStatusReference.sharedValue as Status;

		statusText.text    = playerStatus.status_Name;
		statusText.color   = playerStatus.status_Color;
		fillingImage.color = playerStatus.status_Color;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
