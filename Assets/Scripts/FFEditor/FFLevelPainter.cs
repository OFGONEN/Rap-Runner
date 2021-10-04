/* Created by and for usage of FF Studios (2021). */

#if UNITY_EDITOR
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

		[ HorizontalLine ]
		public GameObject[] objectPalette;

        // Show If Properties
        public bool Single => paintMode == PaintMode.Single; 
        public bool Line => paintMode == PaintMode.Line; 

		// Private Fields \\ 
		private List< GameObject > cachedObjects = new List< GameObject >( 64 );
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

			cachedObjects.Clear();

			var gameObject = PrefabUtility.InstantiatePrefab( objectToPaint ) as GameObject;
			gameObject.transform.position = transform.position.AddUp( height );
			gameObject.transform.SetSiblingIndex( seperatorObject.GetSiblingIndex() );

			cachedObjects.Add( gameObject );

			EditorSceneManager.SaveOpenScenes();
		}

        [ Button(), ShowIf("Line") ]
        public void PaintLine()
        {
			EditorSceneManager.MarkAllScenesDirty();

			cachedObjects.Clear();

			for( var i = 0; i < count; i++ )
            {
				var gameObject = PrefabUtility.InstantiatePrefab( objectToPaint ) as GameObject;

				var position = transform.position + transform.forward * i * gap;
				gameObject.transform.position = position.AddUp( height );

				gameObject.transform.SetSiblingIndex( seperatorObject.GetSiblingIndex() );

				cachedObjects.Add( gameObject );
			}

			EditorSceneManager.SaveOpenScenes();
        }

		[ Button() ]
		public void DeleteLast()
		{
			EditorSceneManager.MarkAllScenesDirty();

            for( var i = cachedObjects.Count - 1; i >= 0; i-- )
            {
				DestroyImmediate( cachedObjects[ i ] );
			}

			EditorSceneManager.SaveOpenScenes();
		}

#endregion

#region Implementation
#endregion

#region Editor Only
		private void OnDrawGizmosSelected()
		{
			Handles.color = Color.red;

			var position = transform.position;
			Handles.DrawWireDisc( position, Vector3.up, 0.1f );
			Handles.DrawDottedLine( position, position.AddUp( 0.5f ), 1 );

			if( objectToPaint != null )
			{
				GUIStyle style = new GUIStyle();
				style.fontSize = 15;
				style.normal.textColor = Color.red;
				style.fontStyle = FontStyle.Bold;

				Handles.Label( position.AddUp( 0.5f ), objectToPaint.name, style );
			}
		}
#endregion
	}

    public enum PaintMode
    {
        Single,
        Line
    }
}
#endif