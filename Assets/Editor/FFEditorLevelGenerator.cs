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
    [ BoxGroup( "Setup" ) ] public Waypoint straightRoad;
    [ BoxGroup( "Setup" ) ] public Waypoint leftCurvedRoad;
    [ BoxGroup( "Setup" ) ] public Waypoint rightCurvedRoad;

    private static WaypointSewer sewer;
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
	[ MenuItem( "FFGame/Start Sewing %#k" ) ]
    public static void SelectStartWaypoint()
    {
		sewer = new WaypointSewer();

		var gameObject              = Selection.activeGameObject;
		var startWaypoint           = gameObject.GetComponentInChildren<Waypoint>();
		    sewer.lastSewedWaypoint = startWaypoint;

		FFLogger.Log( "Start Sewing: " + startWaypoint.Editor_TargetPoint() );
	}

    [ Button() ]
    public void Generate()
    {
        if( sewer == null )
            FFLogger.LogError( " Select a Start Waypoint" );

		EditorSceneManager.MarkAllScenesDirty();

        for( var i = 0; i < levelCode.Length; i++ )
        {
            if( levelCode[ i ] == 'R' ) 
            {
				InstantiateWaypoint( rightCurvedRoad );
            }
            else if( levelCode[ i ] == 'L' )
            {
				InstantiateWaypoint( leftCurvedRoad );
            }
            else
            {
				int count = levelCode[ i ] - 48; // '0' is at 48th index in ASCII table

                for( var x = 0; x < count; x++ )
                {
					InstantiateWaypoint( straightRoad );
				}
			}
        }

		EditorSceneManager.SaveOpenScenes();
	}

    private void InstantiateWaypoint( Waypoint waypoint )
    {
		var gameObject = PrefabUtility.InstantiatePrefab( waypoint.gameObject ) as GameObject;
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
}
