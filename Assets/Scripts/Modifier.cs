/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Modifier : MonoBehaviour
{
#region Fields
    [ Header( "Fired Events" ) ]
    public ParticleSpawnEvent particleSpawnEvent;
	public FloatGameEvent modifier_Event;

	// Private \\
	[ BoxGroup( "Setup" ) ] public float modifier_Point;
    [ BoxGroup( "Setup" ) ] public string modifier_ParticleName;

    // Components 
    protected Collider modiferCollider;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		modiferCollider = GetComponentInChildren< Collider >();
	}

    protected virtual void OnTriggerEnter( Collider other )
    {
        modifier_Event.eventValue = modifier_Point;

		particleSpawnEvent.changePosition = true;
		particleSpawnEvent.spawnPoint     = transform.position;
		particleSpawnEvent.particleAlias  = modifier_ParticleName;

		modifier_Event.Raise();
		particleSpawnEvent.Raise();
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
