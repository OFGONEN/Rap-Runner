/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FFStudio;
using UnityEditor;
using UnityEditor.SceneManagement;
using NaughtyAttributes;

namespace FFEditor
{
	[CreateAssetMenu( fileName = "LevelGenerator", menuName = "FF/Editor/LevelGenerator" )]
	public class FFEditorLevelGenerator : ScriptableObject
	{
#region Fields
		[ BoxGroup( "Setup" ) ] public string levelCode;
		[ BoxGroup( "Setup" ) ] public CustomWaypoint[] customWaypoints;
		[ BoxGroup( "Setup" ) ] public string patternCode;
		[ BoxGroup( "Setup" ) ] public GameObject[] patternPallet;
		[ BoxGroup( "Setup" ) ] public Waypoint straightRoad;
		[ BoxGroup( "Setup" ) ] public Waypoint catwalk;
		[ BoxGroup( "Setup" ) ] public Waypoint startRoad;
		[ BoxGroup( "Setup" ) ] public GameObject playerPrefab;


		private static WaypointSewer sewer;
		private static Dictionary<char, Waypoint> customWaypointDictionary;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
		[ Button( "--- GENERATE LEVEL ---" ) ]
		public void GenerateLevel()
		{
			GenerateEnvironment();
			GenerateEntities();
		}

		[ Button() ]
		public void GenerateEnvironment()
		{
			EditorSceneManager.MarkAllScenesDirty();

			// Generate Custom Road Dictionary
			customWaypointDictionary = new Dictionary<char, Waypoint>( customWaypoints.Length );

			for( var i = 0; i < customWaypoints.Length; i++ )
			{
				customWaypointDictionary.Add( customWaypoints[ i ].character, customWaypoints[ i ].customWaypoint );
			}

			// Find waypoints parent
			var parent = GameObject.FindWithTag( "WaypointParent" );

			if( parent == null )
			{
				FFLogger.LogWarning( "Waypoints parent is abcent!!" );
				FFLogger.LogWarning( "Creating new Waypoints parent!" );

				parent = new GameObject( "waypoints" );
				parent.tag = "WaypointParent";

				parent.transform.SetSiblingIndex( FindSeperatorIndex( "--- Environment ---" ) + 1 ); // refactor this
			}

			// Cache the transform of the  waypoint parent
			var parentTransform = parent.transform;

			// Destroy any child waypoint parent has
			if( parentTransform.childCount > 0 )
			{
				for( var i = parentTransform.childCount - 1; i >= 0; i-- )
				{
					var child = parentTransform.GetChild( i );
					DestroyImmediate( child.gameObject );
				}
			}

			// Spawn start road
			var start = PrefabUtility.InstantiatePrefab( startRoad.gameObject ) as GameObject;

			// Create sewer object
			sewer = new WaypointSewer();
			sewer.lastSewedWaypoint = start.GetComponent<Waypoint>();

			start.transform.SetParent( parentTransform );
			start.transform.position = -sewer.lastSewedWaypoint.Editor_TargetPoint();

			// Read level generation code than spawn waypoints
			for( var i = 0; i < levelCode.Length; i++ )
			{
				Waypoint waypoint;

				customWaypointDictionary.TryGetValue( levelCode[ i ], out waypoint );

				if( waypoint != null )
				{
					InstantiateWaypoint( waypoint, parentTransform );
				}
				else
				{
					int count = levelCode[ i ] - 48; // '0' is at 48th index in ASCII table

					for( var x = 0; x < count; x++ )
					{
						InstantiateWaypoint( straightRoad, parentTransform );
					}
				}
			}

			// Spawn constant level entites
			InstantiateWaypoint( catwalk, parentTransform ); // Final road: catwalk


			EditorSceneManager.SaveOpenScenes();
		}

		[ Button() ]
		public void GenerateEntities()
		{
			EditorSceneManager.MarkAllScenesDirty();

			var player = PrefabUtility.InstantiatePrefab( playerPrefab ) as GameObject; // Player Prefab

			player.transform.forward = Vector3.forward;
			player.transform.position = -Vector3.forward * 1.5f;
			player.transform.SetSiblingIndex( FindSeperatorIndex( "--- Entities ---" ) );

			EditorSceneManager.SaveOpenScenes();
		}


		[ Button() ]
		public void GeneratePatterns()
		{
			EditorSceneManager.MarkAllScenesDirty();

			// var code = "123.5,45.1,0,n-424.1,12.65,1,s";
			var patterns = patternCode.Split( '-' );

			var seperatorIndex_start = FindSeperatorIndex( "--- Patterns_Start ---" );
			var seperatorIndex_end   = FindSeperatorIndex( "--- Patterns_End ---" );
			var startIndex           = seperatorIndex_start + 1;

			DeleteObjects( seperatorIndex_start, seperatorIndex_end );

			foreach( var pattern in patterns )
			{
				var data = pattern.Split( ',' );

				if( data.Length != 4)
				{
					FFLogger.LogError( "Wrong Info: " + data );
					return;
				}

				// Info
				var position        = new Vector3( float.Parse( data[ 0 ] ), 0, float.Parse( data[ 1 ] ) );
				var selectedPattern = patternPallet[ int.Parse( data[ 2 ] ) ];
				var direction       = ReturnDirection( data[ 3 ][ 0 ] );

				// Spawn Object
				var gameObject = PrefabUtility.InstantiatePrefab( selectedPattern ) as GameObject;
				gameObject.transform.position = position;
				gameObject.transform.SetSiblingIndex( startIndex );
				gameObject.transform.forward = direction;

				startIndex++;
			}

			EditorSceneManager.SaveOpenScenes();
		}
#endregion

#region Implementation
		private void DeleteObjects( int startIndex, int endIndex )
		{
			GameObject[] objects = new GameObject[ endIndex - startIndex - 1 ];

			Scene scene = SceneManager.GetActiveScene();
			var rootObjects = scene.GetRootGameObjects();

			for( var i = 0; i < objects.Length; i++ )
			{
				objects[ i ] = rootObjects[ startIndex + i + 1 ].gameObject;
			}


			for( var i = 0; i < objects.Length; i++ )
			{
				DestroyImmediate( objects[ i ] );
			}
		}

		private void InstantiateWaypoint( Waypoint waypoint, Transform parent )
		{
			var gameObject = PrefabUtility.InstantiatePrefab( waypoint.gameObject ) as GameObject;
			gameObject.transform.SetParent( parent );
			gameObject.transform.position = sewer.lastSewedWaypoint.Editor_TargetPoint();

			if( sewer.lastSewedWaypoint is Curved_Waypoint )
			{
				var curvedWaypoint = sewer.lastSewedWaypoint as Curved_Waypoint;
				gameObject.transform.forward = curvedWaypoint.Editor_TurnOrigin().x < 0 ? -curvedWaypoint.transform.right : curvedWaypoint.transform.right;
			}
			else
			{
				gameObject.transform.forward = sewer.lastSewedWaypoint.transform.forward;
			}

			var currentWayPoint = gameObject.GetComponentInChildren<Waypoint>();
			sewer.lastSewedWaypoint.Editor_SetNextWaypoint( currentWayPoint );

			PrefabUtility.RecordPrefabInstancePropertyModifications( sewer.lastSewedWaypoint );

			sewer.lastSewedWaypoint = currentWayPoint;
		}

		private void InstantiatePattern( GameObject waypoint, Vector3 direction, int index )
		{
			var gameObject = PrefabUtility.InstantiatePrefab( waypoint.gameObject ) as GameObject;
			gameObject.transform.SetSiblingIndex( index );
			gameObject.transform.forward = direction;
		}

		private int FindSeperatorIndex( string seperatorName )
		{
			var seperator = GameObject.Find( seperatorName );

			if( seperatorName == null )
			{
				FFLogger.LogError( seperatorName + " is not FOUND!" );
				FFLogger.LogError( "Discard this scene then create SEPERATOR Object" );
				return -1;
			}
			else 
			{
				return seperator.transform.GetSiblingIndex();
			}
		}

		private Vector3 ReturnDirection( char direction )
		{
			if( direction == 'n' )
				return Vector3.forward;
			else if( direction == 's' )
				return Vector3.forward * -1f;
			else if( direction == 'w' )
				return Vector3.right * -1f;
			else if( direction == 'e' )
				return Vector3.right;

			return Vector3.zero;
		}
#endregion

#region Editor Only
#if UNITY_EDITOR

#endif
#endregion
		class WaypointSewer
		{
			public Waypoint lastSewedWaypoint;
		}

		[System.Serializable]
		public struct CustomWaypoint
		{
			public char character;
			public Waypoint customWaypoint;
		}
	}
}