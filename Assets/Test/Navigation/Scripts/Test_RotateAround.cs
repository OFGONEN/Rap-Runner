/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Test_RotateAround : MonoBehaviour
{
#region Fields
    public Transform targetTransform;
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
    [ Button() ]
    public void RotateTargetAround()
    {
		targetTransform.RotateAround( transform.position, Vector3.up, 5f );
	}
#endif
#endregion
}
