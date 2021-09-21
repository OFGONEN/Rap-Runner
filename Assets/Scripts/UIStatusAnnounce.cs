/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FFStudio;
using UnityEngine.UI;
using DG.Tweening;

public class UIStatusAnnounce : UIText
{
#region Fields
    public EventListenerDelegateResponse levelStartListener;
    public Status_Property playerStatusProperty;

	public Vector3 targetPoint;
	public Vector3 targetSize;

	// Delegates
	private Sequence sequence;

    private Vector3 uiStartLocalPosition;
    private Vector3 uiStartLocalSize;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		levelStartListener.OnEnable();
	}

    private void OnDisable()
    {
		levelStartListener.OnDisable();

		playerStatusProperty.changeEvent -= OnPlayerStatusChange;

        if ( sequence != null )
        {
			sequence.Kill();
			sequence = null;
		}
    }

    private void Awake()
    {
		uiStartLocalPosition = uiTransform.localPosition;
		uiStartLocalSize     = uiTransform.localScale;

		textRenderer.enabled = false;

		levelStartListener.response = LevelStartResponse;
	}
#endregion

#region API
#endregion

#region Implementation
    private void  OnPlayerStatusChange()
    {
        if ( sequence != null )
			sequence.Kill();

		uiTransform.localPosition = uiStartLocalPosition;
		uiTransform.localScale    = uiStartLocalSize;

		textRenderer.text    = playerStatusProperty.status_Name;
		textRenderer.color   = playerStatusProperty.status_Color;
		textRenderer.enabled = true;

        var duration = GameSettings.Instance.ui_world_announce_duration;

		sequence = DOTween.Sequence();

		sequence.Append( uiTransform.DOLocalMove( uiStartLocalPosition + targetPoint, duration ) );
		sequence.Join( uiTransform.DOScale( uiStartLocalSize + targetSize, duration ) );
		sequence.AppendInterval( duration / 2f );
		sequence.Append( textRenderer.DOFade( 0, duration / 2f ) );

		sequence.OnComplete( OnSequenceComplete );
	}

    private void OnSequenceComplete()
    {
		sequence.Kill();
		sequence = null;

		textRenderer.enabled = false;
	}

    private void LevelStartResponse()
    {
		playerStatusProperty.changeEvent += OnPlayerStatusChange;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		Handles.DrawDottedLine( uiTransform.position, uiTransform.position + targetPoint, 1f );
		Handles.DrawWireDisc( uiTransform.position + targetPoint, uiTransform.forward, 0.05f );
	}
#endif
#endregion
}
