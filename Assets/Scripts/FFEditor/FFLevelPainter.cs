/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEditor;
using UnityEditor.SceneManagement;
using NaughtyAttributes;

namespace FFEditor
{
	public class FFLevelPainter : MonoBehaviour
	{
#region Fields
        public Transform seperatorObject;
        public GameObject objectToPaint;
		public PaintMode paintMode;

        public float height;
		[ ShowIf("Line") ] public int count;
        [ ShowIf("Line") ] public float gap;

        // Show If Properties
        public bool Single => paintMode == PaintMode.Single; 
        public bool Line => paintMode == PaintMode.Line; 
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
        [ Button(), ShowIf("Single") ]
        public void PaintSingle()
        {
			EditorSceneManager.MarkAllScenesDirty();

			var gameObject = PrefabUtility.InstantiatePrefab( objectToPaint ) as GameObject;
			gameObject.transform.position = transform.position.AddUp( height );
			gameObject.transform.SetSiblingIndex( seperatorObject.GetSiblingIndex() );

			EditorSceneManager.SaveOpenScenes();
		}

        [ Button(), ShowIf("Line") ]
        public void PaintLine()
        {
			EditorSceneManager.MarkAllScenesDirty();

            for( var i = 0; i < count; i++ )
            {
				var gameObject = PrefabUtility.InstantiatePrefab( objectToPaint ) as GameObject;

				var position = transform.position + transform.forward * i * gap;
				gameObject.transform.position = position.AddUp( height );

				gameObject.transform.SetSiblingIndex( seperatorObject.GetSiblingIndex() );
			}

			EditorSceneManager.SaveOpenScenes();
        }
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Handles.color = Color.red;

			var position = transform.position;
			Handles.DrawWireDisc( position, Vector3.up, 0.1f );
			Handles.DrawDottedLine( position, position.AddUp( 0.5f ), 1 );

			if( objectToPaint != null )
				Handles.Label( position.AddUp( 0.5f ), objectToPaint.name );
		}
#endif
#endregion
	}

    public enum PaintMode
    {
        Single,
        Line
    }
}