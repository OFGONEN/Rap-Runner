/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRenderer : MonoBehaviour
{
#region Fields
	public string rendererName;

	// Private Fields \\
	private Renderer[] modelRenderers;

	// Public Delegates \\
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		modelRenderers = GetComponentsInChildren< Renderer >();
	}
#endregion

#region API
	public void ToggleRenderer( bool value )
	{
		foreach( var renderer in modelRenderers )
		{
			renderer.enabled = value;
		}
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
