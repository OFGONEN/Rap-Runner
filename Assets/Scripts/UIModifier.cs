/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;

public class UIModifier : UIText
{
#region Fields
    [ Header( "Event Listeners" ) ]
    public EventListenerDelegateResponse modifierEventListener;

    public Image imageRenderer; // Dolar sign
	[HorizontalLine]
	public Vector3 targetPoint;
    public Compare compare;

	// Private Fields \\
	private Color textStartColor;
    private Color iconStartColor;

	private Vector3 uiStartLocalPosition;

	// Delegates
	private Sequence sequence;
#endregion

#region Properties
#endregion

#region Unity API
    private void OnEnable()
    {
		modifierEventListener.OnEnable();
	}

    private void OnDisable()
    {
		modifierEventListener.OnDisable();

        if( sequence != null )
        {
			sequence.Kill();
			sequence = null;
		}
    }

    private void Awake()
    {
		textStartColor = textRenderer.color;
		iconStartColor = imageRenderer.color;

		uiStartLocalPosition = uiTransform.localPosition;

		textRenderer.enabled  = false;
		imageRenderer.enabled = false;
	}
#endregion

#region API
#endregion

#region Implementation
    private void ModifierEventResponse()
    {
		var modifyAmount = ( modifierEventListener.gameEvent as FloatGameEvent ).eventValue;

		if( compare == Compare.Greater && modifyAmount > 0 )
        {
			StartSequence();
		}
        else if( modifyAmount < 0 )
        {
			StartSequence();
        }
    }

    private void StartSequence()
    {
        if( sequence != null )
			sequence.Kill();

		sequence = DOTween.Sequence();

		uiTransform.localPosition = uiStartLocalPosition;

		textRenderer.color  = textStartColor;
		imageRenderer.color = iconStartColor;

		textRenderer.enabled  = true;
		imageRenderer.enabled = true;

		var duration = GameSettings.Instance.ui_world_modifier_duration;

		sequence.Append( uiTransform.DOLocalMove( uiStartLocalPosition + targetPoint, GameSettings.Instance.ui_world_modifier_duration ) );
		sequence.AppendInterval( duration / 2f );
		sequence.Append( textRenderer.DOFade( 0, duration / 2f ) );
		sequence.Join( imageRenderer.DOFade( 0, duration / 2f ) );

		sequence.OnComplete( OnSequenceComplete );
	}

    private void OnSequenceComplete()
    {
		sequence.Kill();
		sequence = null;

		textRenderer.enabled  = false;
		imageRenderer.enabled = false;
	}
#endregion

#region Editor Only
#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if ( compare == Compare.Greater )
			Handles.color = Color.green;
		else
			Handles.color = Color.red;

		Handles.DrawDottedLine( uiTransform.position, uiTransform.position + targetPoint, 1f );
		Handles.DrawWireDisc( uiTransform.position + targetPoint, uiTransform.forward, 0.05f );
	}
#endif
#endregion
}
