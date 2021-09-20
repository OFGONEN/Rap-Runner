/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using NaughtyAttributes;
using UnityEngine.UI;

public class UILoadingBar : UIEntity
{
#region Fields
    [ BoxGroup( "Shared Variables" ) ] public SharedFloatProperty progressProperty;

	[ HorizontalLine ]
	[ BoxGroup( "UI Elements" ) ] public Image fillingImage;
#endregion

#region Unity API
    protected virtual void OnEnable()
    {
		progressProperty.changeEvent += OnValueChange;
	}

    protected virtual void OnDisable()
    {
		progressProperty.changeEvent -= OnValueChange;
    }

	protected virtual void Awake()
	{
		OnValueChange(); // Set filling amount to value at the start 
	}
#endregion

#region API
#endregion

#region Implementation
    protected virtual void OnValueChange()
    {
		fillingImage.fillAmount = progressProperty.sharedValue;
	}
#endregion
}
