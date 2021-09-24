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
			var gameSettings = AssetDatabase.LoadAssetAtPath( "Assets/Editor/LevelGenerator.asset", typeof( ScriptableObject ) );

			Selection.SetActiveObjectWithContext( gameSettings, gameSettings );
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

		static LevelSewer sewer;

		// [ MenuItem( "FFGame/Start Sewing %#k" ) ]
		private static void StartSewing()
		{
			sewer = new LevelSewer();

			var gameObject              = Selection.activeGameObject;
			var startWaypoint           = gameObject.GetComponentInChildren< Waypoint >();
			    sewer.lastSewedWaypoint = startWaypoint;

			FFLogger.Log( "Start Sewing: " + startWaypoint.Editor_TargetPoint() );
		}

		// [ MenuItem( "FFGame/Sew Waypoint %#l" ) ]
		private static void SewWaypoint()
		{
			EditorSceneManager.MarkAllScenesDirty();

			var gameObject = PrefabUtility.InstantiatePrefab( Selection.activeObject ) as GameObject;
			gameObject.transform.position = sewer.lastSewedWaypoint.Editor_TargetPoint();

			if( sewer.lastSewedWaypoint is Curved_Waypoint )
			{
				var curvedWaypoint = sewer.lastSewedWaypoint as Curved_Waypoint;
				gameObject.transform.forward = curvedWaypoint.Editor_TurnOrigin().x < 0 ? -curvedWaypoint.transform.right : curvedWaypoint.transform.right;
			}
			else 
			{
				gameObject.transform.forward  = sewer.lastSewedWaypoint.transform.forward;
			}

			var currentWayPoint = gameObject.GetComponentInChildren< Waypoint >();
			sewer.lastSewedWaypoint.Editor_SetNextWaypoint( currentWayPoint );
			PrefabUtility.RecordPrefabInstancePropertyModifications( sewer.lastSewedWaypoint );

			sewer.lastSewedWaypoint = currentWayPoint;

			EditorSceneManager.SaveOpenScenes();
			FFLogger.Log( gameObject.name + " Sewed: " + sewer.lastSewedWaypoint.Editor_TargetPoint() );
		}

		class LevelSewer
		{
			public Waypoint lastSewedWaypoint;
		}
	}
}