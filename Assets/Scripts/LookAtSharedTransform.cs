/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class LookAtSharedTransform : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ) ] public SharedReferenceProperty sharedTransformReference;

    // Private Fields \\
    private Transform sharedTransform;

    // Delegates
    private UnityMessage updateMethod;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		sharedTransformReference.changeEvent += OnSharedReferenceSet;
	}

    private void OnDisable()
    {
		sharedTransformReference.changeEvent -= OnSharedReferenceSet;
	}

    private void Awake()
    {
        updateMethod = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		updateMethod();
	}

#endregion

#region API
#endregion

#region Implementation
    private void OnSharedReferenceSet()
    {
        if( sharedTransformReference.sharedValue == null )
			updateMethod = ExtensionMethods.EmptyMethod;
        else
        {
			sharedTransform = sharedTransformReference.sharedValue as Transform;
			updateMethod    = LookAtReference;
		}
    }

    private void LookAtReference()
    {
        transform.LookAtAxis( sharedTransform.position, Vector3.up, -1f);
    }
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
