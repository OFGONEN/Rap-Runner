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
	public class FFLevelPatternPainter : MonoBehaviour
	{
#region Fields
        public Transform seperatorObject;
        public int patternIndex;

        public GameObject[] patternPallet;

        private GameObject patternToPaint;
        private GameObject lastSpawnedPattern;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
        [ Button( "Spawn Pattern" ) ]
        private void SpawnPattern()
        {
			EditorSceneManager.MarkAllScenesDirty();

			var gameObject = PrefabUtility.InstantiatePrefab( patternToPaint ) as GameObject;
			gameObject.transform.position = transform.position;
			gameObject.transform.SetSiblingIndex( seperatorObject.GetSiblingIndex() );
			gameObject.transform.forward = transform.forward;

			lastSpawnedPattern = gameObject;

			EditorSceneManager.SaveOpenScenes();

        }

        [ Button( "Delete Last Pattern" ) ]
        private void DeleteLastSpawnedPattern()
        {
			EditorSceneManager.MarkAllScenesDirty();

			DestroyImmediate( lastSpawnedPattern );

			EditorSceneManager.SaveOpenScenes();
        }

        [ Button() ]
        private void ChangePreview()
        {
			var valid = patternPallet != null && patternPallet.Length > 0 /* pattern pallet valid */ && patternIndex < patternPallet.Length && patternIndex >= 0 /*  index valid */;

			if( valid ) 
            {
				patternToPaint = patternPallet[ patternIndex ];
                SpawnPreview();
			}
        }

        [ Button() ]
        private void DeletePreview()
        {
			EditorSceneManager.MarkAllScenesDirty();

            if( transform.childCount > 0 )
				DestroyImmediate( transform.GetChild( 0 ).gameObject );

			EditorSceneManager.SaveOpenScenes();
        }


        private void SpawnPreview()
        {
			EditorSceneManager.MarkAllScenesDirty();

            DeletePreview();

			var gameObject = PrefabUtility.InstantiatePrefab( patternToPaint ) as GameObject;
			gameObject.transform.position = transform.position;
			gameObject.transform.SetParent( transform );
			gameObject.transform.forward = transform.forward;

			EditorSceneManager.SaveOpenScenes();
        }
#endregion

#region Editor Only
		private void OnDrawGizmosSelected()
		{
			Handles.color = Color.red;

			var position = transform.position;
			Handles.DrawWireDisc( position, Vector3.up, 0.1f );
			Handles.DrawDottedLine( position, position.AddUp( 0.5f ), 1 );

			if( patternToPaint != null )
			{
				GUIStyle style = new GUIStyle();
				style.fontSize = 15;
				style.normal.textColor = Color.red;
				style.fontStyle = FontStyle.Bold;

				Handles.Label( position.AddUp( 0.5f ), patternToPaint.name, style );
			}
		}
#endregion
	}
}
#endif