/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRenderer : MonoBehaviour
{
#region Fields
	public string rendererName;

	// Private Fields \\
	private Renderer modelRenderer;

	// Public Delegates \\
	public Renderer Renderer => modelRenderer;
#endregion

#region Properties
#endregion

#region Unity API
	private void Awake()
	{
		modelRenderer = GetComponentInChildren< Renderer >();
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
