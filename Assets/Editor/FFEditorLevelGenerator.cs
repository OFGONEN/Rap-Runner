/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEditor;
using UnityEditor.SceneManagement;
using NaughtyAttributes;

[ CreateAssetMenu( fileName = "LevelGenerator", menuName = "FF/Editor/LevelGenerator" ) ]
public class FFEditorLevelGenerator : ScriptableObject
{
#region Fields
    [ BoxGroup( "Setup" ) ] public string levelCode;
    [ BoxGroup( "Setup" ) ] public CustomWaypoint[] customWaypoints;
    [ BoxGroup( "Setup" ) ] public Waypoint straightRoad;
    [ BoxGroup( "Setup" ) ] public Waypoint catwalk;
    [ BoxGroup( "Setup" ) ] public Waypoint startRoad;


    private static WaypointSewer sewer;
	private static Dictionary< char, Waypoint > customWaypointDictionary;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
    [ Button() ]
    public void Generate()
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

		if ( parent == null )
		{
			FFLogger.LogError( "Waypoints parent is abcent!!" );
			FFLogger.LogWarning( "Creating new Waypoints parent!" );

			parent = new GameObject( "waypoints" );
			parent.transform.SetSiblingIndex( 3 );
			parent.tag = "WaypointParent";
		}

		var parentTransform = parent.transform;

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

		start.transform.SetParent( parentTransform );
		start.transform.position = Vector3.zero;

		// Create sewer object
		sewer = new WaypointSewer();
		sewer.lastSewedWaypoint = start.GetComponent< Waypoint >();

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

		InstantiateWaypoint( catwalk, parentTransform );
		EditorSceneManager.SaveOpenScenes();
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

		var currentWayPoint = gameObject.GetComponentInChildren< Waypoint >();
		sewer.lastSewedWaypoint.Editor_SetNextWaypoint( currentWayPoint );

		PrefabUtility.RecordPrefabInstancePropertyModifications( sewer.lastSewedWaypoint );

		sewer.lastSewedWaypoint = currentWayPoint;
    }
#endif
#endregion

	class WaypointSewer
	{
		public Waypoint lastSewedWaypoint;
	}

	[ System.Serializable ]
	public struct CustomWaypoint
	{
		public char character;
		public Waypoint customWaypoint;
	}
}
