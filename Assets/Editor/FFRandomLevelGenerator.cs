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
	[CreateAssetMenu( fileName = "RandomLevelGenerator", menuName = "FF/Editor/RandomLevelGenerator" )]
    public class FFRandomLevelGenerator : ScriptableObject
    {
        [ BoxGroup( "Setup" ) ] public GameObject[] patternPallet;
        [ BoxGroup( "Setup" ) ] public Vector2 minMaxRandom;
        [ BoxGroup( "Setup" ) ] public float roadLenght;

        private Waypoint currentWaypoint;
        private int spawnSiblingIndex;
        private Vector3 spawnPosition;

        [ Button() ]
        public void RandomGenerate()
        {
			EditorSceneManager.MarkAllScenesDirty();

			var waypointParent = GameObject.FindWithTag( "WaypointParent" );

			var seperatorIndex_start = GameObject.Find( "--- Patterns_Start ---" ).transform.GetSiblingIndex();
			var seperatorIndex_end   = GameObject.Find( "--- Patterns_End ---" ).transform.GetSiblingIndex();
			    spawnSiblingIndex    = seperatorIndex_start + 1;
            
            DeleteObjects( seperatorIndex_start, seperatorIndex_end );

            currentWaypoint = waypointParent.transform.GetChild( 1 ).GetComponent< Waypoint >();

            var lastWaypointIndex = waypointParent.transform.childCount - 3;
            var lastWapoint       = waypointParent.transform.GetChild( lastWaypointIndex );

            spawnPosition = currentWaypoint.transform.position;

            while( lastWapoint.InverseTransformPoint( spawnPosition ).z < 0 )
            {
                var randomDistance = Random.Range(minMaxRandom.x, minMaxRandom.y);
                var randomPattern = patternPallet[Random.Range(0, patternPallet.Length - 1)];

                var forward = currentWaypoint.transform.forward;
                spawnPosition += forward * randomDistance;

                var spawnedPattern = PlaceRandomPattern(spawnPosition, forward, randomPattern);

                spawnPosition += forward * FarestChildDistance(spawnedPattern.transform);

                var spawnPosition_Relative = currentWaypoint.transform.InverseTransformPoint(spawnPosition);

                if (spawnPosition_Relative.z >= roadLenght)
                {
                    currentWaypoint = currentWaypoint.NextWaypoint;

                    if (currentWaypoint is Curved_Waypoint)
                    {
                        currentWaypoint = currentWaypoint.NextWaypoint;

                        var currentForward = currentWaypoint.transform.forward;
                        spawnedPattern.position = currentWaypoint.transform.position + currentForward * Random.Range(minMaxRandom.x, minMaxRandom.x * 2);
                        spawnedPattern.forward = currentForward;

                        spawnPosition = spawnedPattern.position + currentForward * FarestChildDistance(spawnedPattern.transform);
                    }
                    else
                    {
                        var relative = currentWaypoint.transform.InverseTransformPoint(spawnPosition);

                        if (spawnPosition_Relative.z >= roadLenght)
                        {
                            currentWaypoint = currentWaypoint.NextWaypoint;

                            if (currentWaypoint is Curved_Waypoint)
                            {
                                currentWaypoint = currentWaypoint.NextWaypoint;

                                var currentForward = currentWaypoint.transform.forward;
                                spawnedPattern.position = currentWaypoint.transform.position + currentForward * Random.Range(minMaxRandom.x, minMaxRandom.x * 2);
                                spawnedPattern.forward = currentForward;

                                spawnPosition = spawnedPattern.position + currentForward * FarestChildDistance(spawnedPattern.transform);
                            }
                        }
                    }

                }
            }

			EditorSceneManager.SaveOpenScenes();
        }

        [ Button() ]
        public void RandomSpawnStep()
        {
			EditorSceneManager.MarkAllScenesDirty();
			var waypointParent = GameObject.FindWithTag( "WaypointParent" );

			var seperatorIndex_start = GameObject.Find( "--- Patterns_Start ---" ).transform.GetSiblingIndex();
			var seperatorIndex_end   = GameObject.Find( "--- Patterns_End ---" ).transform.GetSiblingIndex();
			    spawnSiblingIndex    = seperatorIndex_start + 1;

            var lastWaypointIndex = waypointParent.transform.childCount - 2;
            var lastWapoint       = waypointParent.transform.GetChild( lastWaypointIndex );

            if( currentWaypoint == null )
            {
                currentWaypoint = waypointParent.transform.GetChild( 1 ).GetComponent< Waypoint >();
                spawnPosition   = currentWaypoint.transform.position;
            }

            var randomDistance = Random.Range( minMaxRandom.x, minMaxRandom.y );
            var randomPattern  = patternPallet[ Random.Range(0, patternPallet.Length - 1 ) ];

            var forward        = currentWaypoint.transform.forward;
                spawnPosition += forward * randomDistance;

            var spawnedPattern = PlaceRandomPattern( spawnPosition, forward, randomPattern );

            spawnPosition += forward * FarestChildDistance( spawnedPattern.transform );

            var spawnPosition_Relative = currentWaypoint.transform.InverseTransformPoint( spawnPosition );

            if( spawnPosition_Relative.z >= roadLenght )
            {
                currentWaypoint = currentWaypoint.NextWaypoint;

                if( currentWaypoint is Curved_Waypoint )
                {
                    currentWaypoint = currentWaypoint.NextWaypoint;

                    var currentForward          = currentWaypoint.transform.forward;
                        spawnedPattern.position = currentWaypoint.transform.position + currentForward * Random.Range( minMaxRandom.x, minMaxRandom.x * 2 );
                        spawnedPattern.forward  = currentForward;
                    
                    spawnPosition = spawnedPattern.position + currentForward * FarestChildDistance( spawnedPattern.transform );
                }
                else
                {
                    var relative = currentWaypoint.transform.InverseTransformPoint( spawnPosition );
                    
                    if( spawnPosition_Relative.z >= roadLenght )
                    {
                        currentWaypoint = currentWaypoint.NextWaypoint;

                        if ( currentWaypoint is Curved_Waypoint )
                        {
                            currentWaypoint = currentWaypoint.NextWaypoint;

                            var currentForward          = currentWaypoint.transform.forward;
                                spawnedPattern.position = currentWaypoint.transform.position + currentForward * Random.Range( minMaxRandom.x, minMaxRandom.x * 2 );
                                spawnedPattern.forward  = currentForward;

                            spawnPosition = spawnedPattern.position + currentForward * FarestChildDistance( spawnedPattern.transform );
                        }
                    }
                }
                
            }

			EditorSceneManager.SaveOpenScenes();
        }

        [ Button( "ClearLevel" ) ]
        public void DeletePatterns()
        {
            currentWaypoint = null;

			var seperatorIndex_start = GameObject.Find( "--- Patterns_Start ---" ).transform.GetSiblingIndex();
			var seperatorIndex_end   = GameObject.Find( "--- Patterns_End ---" ).transform.GetSiblingIndex();
            
            DeleteObjects( seperatorIndex_start, seperatorIndex_end );
        }

        private Transform PlaceRandomPattern( Vector3 position, Vector3 forward, GameObject pattern )
        {
			var gameObject = PrefabUtility.InstantiatePrefab( pattern ) as GameObject;
			gameObject.transform.position = position;
			gameObject.transform.SetSiblingIndex( spawnSiblingIndex );
			gameObject.transform.forward = forward;

            return gameObject.transform;
        }

        private float FarestChildDistance( Transform parent )
        {
            float distance = 0;

            for( var i = 0; i < parent.childCount; i++ )
            {
                var child = parent.GetChild( i );
                distance = Mathf.Max( distance, child.localPosition.z );
            }

            return distance;
        }

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
    }
}