using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using Facebook.Unity;
using ElephantSDK;

namespace FFStudio
{
	public class FFAnalytic : MonoBehaviour
	{
#region Fields
		[Header( "Event Listeners" )]
		public EventListenerDelegateResponse elephantEventListener;
#endregion

#region UnityAPI
		private void OnEnable()
		{
			elephantEventListener.OnEnable();
		}

		private void OnDisable()
		{
			elephantEventListener.OnDisable();
		}

		private void Awake()
		{
			elephantEventListener.response = ElephantEventResponse;
		}

#endregion

#region Implementation
		void ElephantEventResponse()
		{
			var gameEvent = elephantEventListener.gameEvent as ElephantLevelEvent;

			switch( gameEvent.elephantEventType )
			{
				case ElephantEvent.LevelStarted:
					Elephant.LevelStarted( gameEvent.level );
					FFLogger.Log( "FFAnalytic Elephant LevelStarted: " + gameEvent.level );
					break;
				case ElephantEvent.LevelCompleted:
					Elephant.LevelCompleted( gameEvent.level );
					FFLogger.Log( "FFAnalytic Elephant LevelFinished: " + gameEvent.level );
					break;
				case ElephantEvent.LevelFailed:
					Elephant.LevelFailed( gameEvent.level );
					FFLogger.Log( "FFAnalytic Elephant LevelFailed: " + gameEvent.level );
					break;
			}
		}
#endregion
	}
}