/* Created by and for usage of FF Studios (2021). */

using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using FFStudio;
using System.Reflection;

namespace FFEditor
{
	public static class FFShortcutUtility
	{
		static private TransformData currentTransformData;

		[ MenuItem( "FFShortcut/TakeScreenShot #F12" ) ]
		public static void TakeScreenShot()
		{
			int counter = 0;
			var path = Path.Combine( Application.dataPath, "../", "ScreenShot_" + counter + ".png" );

			while( File.Exists( path ) ) // If file is not exits new screen shot will be a new file
			{
				counter++;
				path = Path.Combine( Application.dataPath, "../", "ScreenShot_" + counter + ".png" ); // ScreenShot_1.png
			}

			ScreenCapture.CaptureScreenshot( "ScreenShot_" + counter + ".png" );
			AssetDatabase.SaveAssets();

			Debug.Log( "ScreenShot Taken: " + "ScreenShot_" + counter + ".png" );
		}

		[ MenuItem( "FFShortcut/Delete PlayerPrefs _F9" ) ]
		static private void ResetPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
			Debug.Log( "PlayerPrefs Deleted" );
		}

		[ MenuItem( "FFShortcut/Previous Level _F10" ) ]
		static private void PreviousLevel()
		{
			var currentLevel = PlayerPrefs.GetInt( "Level" );

			currentLevel = Mathf.Max( currentLevel - 1, 1 );

			PlayerPrefs.SetInt( "Level", currentLevel );
			PlayerPrefs.SetInt( "Consecutive Level", currentLevel );

			Debug.Log( "Level Set:" + currentLevel );
		}

		[ MenuItem( "FFShortcut/Next Level _F11" ) ]
		static private void NextLevel()
		{
			var nextLevel = PlayerPrefs.GetInt( "Level" ) + 1;

			PlayerPrefs.SetInt( "Level", nextLevel );
			PlayerPrefs.SetInt( "Consecutive Level", nextLevel );

			Debug.Log( "Level Set:" + nextLevel );

		}

		[ MenuItem( "FFShortcut/Save All Assets _F12" ) ]
		static private void SaveAllAssets()
		{
			AssetDatabase.SaveAssets();
			Debug.Log( "AssetDatabase Saved" );
		}

		[ MenuItem( "FFShortcut/Select Level Data &1" ) ]
		static private void SelectLevelData()
		{
			var levelData = Resources.Load( "level_data_1" );

			Selection.SetActiveObjectWithContext( levelData, levelData );
		}

		[ MenuItem( "FFShortcut/Select Game Settings &2" ) ]
		static private void SelectGameSettings()
		{
			var gameSettings = Resources.Load( "game_settings" );

			Selection.SetActiveObjectWithContext( gameSettings, gameSettings );
		}

		[ MenuItem( "FFShortcut/Select App Scene &3" ) ]
		static private void SelectAppScene()
		{
			var gameSettings = AssetDatabase.LoadAssetAtPath( "Assets/Scenes/app.unity", typeof( SceneAsset ) );

			Selection.SetActiveObjectWithContext( gameSettings, gameSettings );
		}

		[ MenuItem( "FFShortcut/Select Level Generator &4" ) ]
		static private void SelectLevelGenerator()
		{
			var gameSettings = AssetDatabase.LoadAssetAtPath( "Assets/Editor/RandomLevelGenerator.asset", typeof( ScriptableObject ) );

			Selection.SetActiveObjectWithContext( gameSettings, gameSettings );
		}

		[ MenuItem( "FFShortcut/Select Play Mode Utility &5" ) ]
		static private void SelectPlayModeUtility()
		{
			var gameSettings = AssetDatabase.LoadAssetAtPath( "Assets/Editor/PlayModeUtilitySettings.asset", typeof( ScriptableObject ) );

			Selection.SetActiveObjectWithContext( gameSettings, gameSettings );
		}

		[ MenuItem( "FFShortcut/Select LevelPainter &l" ) ]
		static private void SelectLevelPainter()
		{
			var levelPainter = GameObject.Find( "ff_level_painter" );

			Selection.SetActiveObjectWithContext( levelPainter, levelPainter );
		}

		[ MenuItem( "FFShortcut/Select LevelPattern Painter &p" ) ]
		static private void SelectLevelPatternPainter()
		{
			var levelPainter = GameObject.Find( "ff_level_pattern_painter" );

			Selection.SetActiveObjectWithContext( levelPainter, levelPainter );
		}

		[ MenuItem( "FFShortcut/Copy Global Transform &c" ) ]
		static private void CopyTransform()
		{
			currentTransformData = Selection.activeGameObject.transform.GetTransformData();
		}

		[ MenuItem( "FFShortcut/Paste Global Transform &v" ) ]
		static private void PasteTransform()
		{
			var gameObject = Selection.activeGameObject.transform;
			gameObject.SetTransformData( currentTransformData );
		}

		[ MenuItem( "FFShortcut/Clear Console %#x" ) ]
		private static void ClearLog()
		{
			var assembly = Assembly.GetAssembly( typeof( UnityEditor.Editor ) );
			var type = assembly.GetType( "UnityEditor.LogEntries" );
			var method = type.GetMethod( "Clear" );
			method.Invoke( new object(), null );
		}

		[ MenuItem( "FFShortcut/Layout Objects" ) ]
		private static void LayoutObjects()
		{
            EditorSceneManager.MarkAllScenesDirty();

            var selection = Selection.gameObjects;

            var parent = new GameObject("Layout");

            var position = Vector3.zero;

            for( var i = 0; i < 22; i++ )
			{
                var pattern = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Patterns/pattern_" + i + ".prefab", typeof( Object ));
                var spawned = PrefabUtility.InstantiatePrefab( pattern ) as GameObject;

                spawned.transform.position = position;
                spawned.transform.SetParent(parent.transform);

                position += Vector3.right * 10;

				if( i % 5 == 4)
				{
                    position += Vector3.forward * 15;
                    position.x = 0;
                }
            }

            EditorSceneManager.SaveOpenScenes();
        }
	}
}