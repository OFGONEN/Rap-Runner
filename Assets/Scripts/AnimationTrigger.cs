/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimationTrigger : MonoBehaviour
{
#region Fields
    [ BoxGroup( "Setup" ), SerializeField ] public Animator[] animators;

    // Private Fields \\
    private ParticleSystem mainParticleSystem;
#endregion

#region Properties
#endregion

#region Unity API
    private void Awake()
    {
		mainParticleSystem = GetComponentInChildren< ParticleSystem >();
	}

    private void OnTriggerEnter( Collider other )
    {
        foreach( var animator in animators )
        {
			animator.SetTrigger( "trigger" );
		}
    }
#endregion

#region API
    public void PlayParticle()
    {
		mainParticleSystem.Play();
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
