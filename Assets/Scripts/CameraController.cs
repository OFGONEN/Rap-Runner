/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;

public class CameraController : MonoBehaviour
{
#region Fields
    [ Header( "Event Listeners" ) ] 
    public EventListenerDelegateResponse levelRevealedListener;
    public MultipleEventListenerDelegateResponse levelCompleteListener;

    [ Header( "Fired Events" ) ] 
    public GameEvent levelStartEvent;

    [ HorizontalLine ]
    [ BoxGroup( "Setup" ), SerializeField ] private Transform target;
    [ BoxGroup( "Setup" ), SerializeField ] private Vector3 targetPosition;
    [ BoxGroup( "Setup" ), SerializeField ] private Vector3 targetRotation;

    // Private Fields \\
    private UnityMessage updateMethod;
    private Sequence levelStartSequence;
	private Sequence moveAndLookSequence;

	private float totalRotateAmount = 0f;
	private float rotateSign = 1f;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelRevealedListener.OnEnable();
		levelCompleteListener.OnEnable();
	}
    
    private void OnDisable()
    {
 		levelRevealedListener.OnDisable();
		levelCompleteListener.OnDisable();

		if( moveAndLookSequence != null )
		{
			moveAndLookSequence.Kill();
			moveAndLookSequence = null;
		}
    }

    private void Awake()
    {
		levelRevealedListener.response = LevelRevealedResponse;
		levelCompleteListener.response = LevelCompleteResponse;
		updateMethod                   = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		updateMethod();
	}
#endregion

#region API
	public void MoveAndLook( Vector3 movePosition, Vector3 lookRotation )
	{
		updateMethod = ExtensionMethods.EmptyMethod;

		var duration = GameSettings.Instance.camera_duration_moveAndLook;

		moveAndLookSequence = DOTween.Sequence();
		moveAndLookSequence.Append( transform.DOLocalMove( movePosition, duration ) );
		moveAndLookSequence.Join( transform.DOLocalRotate( lookRotation, duration ) );
		moveAndLookSequence.OnComplete( OnMoveAndLookSequenceComplete );
	}

	public void ReturnDefault()
	{
		var duration = GameSettings.Instance.camera_duration_moveAndLook;

		levelStartSequence = DOTween.Sequence();
		levelStartSequence.Append( transform.DOLocalMove( targetPosition, duration ) );
		levelStartSequence.Join( transform.DOLocalRotate( targetRotation, duration ) );
		levelStartSequence.OnComplete( OnReturnDefaultComplete );
	}
#endregion

#region Implementation
    private void FollowTargetMethod()
    {
		var localPosition        = transform.localPosition;
		var target_localPosition = target.localPosition;
		var nextPosition         = Mathf.Lerp( localPosition.x, target_localPosition.x, Time.deltaTime * GameSettings.Instance.camera_speed_follow );

		localPosition.x         = nextPosition;
		transform.localPosition = localPosition;
	}

    private void RotateAroundTargetMethod()
    {
        var rotateAmount       = Time.deltaTime * rotateSign * GameSettings.Instance.camera_speed_LevelEndRotation;
            totalRotateAmount += rotateAmount;

		transform.RotateAround( target.position, Vector3.up, rotateAmount );

		if ( Mathf.Abs( totalRotateAmount ) >= GameSettings.Instance.camera_clamp_LevelEndRotation )
		    rotateSign *= -1f;
	}


	private void LevelRevealedResponse()
    {
		levelStartSequence = DOTween.Sequence();
		levelStartSequence.Append( transform.DOLocalMove( targetPosition, GameSettings.Instance.camera_duration_movement ) );
		levelStartSequence.Join( transform.DOLocalRotate( targetRotation, GameSettings.Instance.camera_duration_movement ) );
		levelStartSequence.OnComplete( OnLevelStartSequenceComplete );
	}

    private void LevelCompleteResponse()
    {
		    updateMethod    = ExtensionMethods.EmptyMethod;
		var localPosition   = transform.localPosition;
		    localPosition.x = target.localPosition.x;

		transform.DOLocalMove( localPosition, GameSettings.Instance.camera_duration_movement ).OnComplete( () => updateMethod = RotateAroundTargetMethod );
	}

    private void OnLevelStartSequenceComplete()
    {
		levelStartSequence.Kill();
		levelStartSequence = null;

		updateMethod = FollowTargetMethod;
        levelStartEvent.Raise();
	}

	private void OnReturnDefaultComplete()
	{
		levelStartSequence.Kill();
		levelStartSequence = null;

		updateMethod = FollowTargetMethod;
	}

	private void OnMoveAndLookSequenceComplete()
	{
		moveAndLookSequence.Kill();
		moveAndLookSequence = null;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
		var final_cameraPosition = transform.position + ( targetPosition - transform.localPosition ) ;

		Handles.ArrowHandleCap( 0, final_cameraPosition, Quaternion.Euler( targetRotation ), 1f, EventType.Repaint );
		Handles.Label( final_cameraPosition.AddUp( 0.5f ), "Final Camera Position\n" + final_cameraPosition );
	}
#endif
#endregion
}
