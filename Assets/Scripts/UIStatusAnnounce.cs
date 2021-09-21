/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEngine.UI;
using DG.Tweening;

public class UIStatusAnnounce : UIText
{
#region Fields
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
		playerStatusProperty.changeEvent += OnPlayerStatusChange;
	}

    private void OnDisable()
    {
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
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
