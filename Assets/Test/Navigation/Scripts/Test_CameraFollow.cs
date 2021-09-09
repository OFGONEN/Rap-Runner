/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_CameraFollow : MonoBehaviour
{
#region Fields
    public Transform target;

	public float followSpeed;
	public Vector3 followAxis;
#endregion

#region Properties
#endregion

#region Unity API
    private void Update()
    {
		var localPosition = transform.localPosition;
		var targetLocalPosition = target.localPosition;
		var nextPosition = Mathf.Lerp( localPosition.x, targetLocalPosition.x, Time.deltaTime * followSpeed );

		localPosition.x = nextPosition;

		transform.localPosition = localPosition;
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
