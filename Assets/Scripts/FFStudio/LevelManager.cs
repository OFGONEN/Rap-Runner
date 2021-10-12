/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Rendering;

namespace FFStudio
{
    public class LevelManager : MonoBehaviour
    {
#region Fields
        [ Header( "Event Listeners" ) ]
        public EventListenerDelegateResponse levelLoadedListener;
        public EventListenerDelegateResponse levelRevealedListener;
        public EventListenerDelegateResponse levelStartedListener;

        [ Header( "Fired Events" ) ]
        public GameEvent levelFailedEvent;
        public GameEvent levelCompleted;

        [ Header( "Level Releated" ) ]
        public SharedFloatProperty levelProgress;
#endregion

#region UnityAPI
        private void OnEnable()
        {
            levelLoadedListener.OnEnable();
            levelRevealedListener.OnEnable();
            levelStartedListener.OnEnable();
        }

        private void OnDisable()
        {
            levelLoadedListener.OnDisable();
            levelRevealedListener.OnDisable();
            levelStartedListener.OnDisable();
        }

        private void Awake()
        {
            levelLoadedListener.response   = LevelLoadedResponse;
            levelRevealedListener.response = LevelRevealedResponse;
            levelStartedListener.response  = LevelStartedResponse;
        }
#endregion

#region Implementation
        private void LevelLoadedResponse()
        {
            levelProgress.SetValue( 0 );

			// SetRenderSettings( CurrentLevelData.Instance.levelData );
		}

        private void LevelRevealedResponse()
        {
        }

        private void LevelStartedResponse()
        {
        }

        // private void SetRenderSettings( LevelData levelData )
        // {
        //     // Skybox
		// 	RenderSettings.skybox                 = levelData.skyboxMaterial;
		// 	RenderSettings.subtractiveShadowColor = levelData.subtractiveShadowColor;

        //     //Environment Lighting
        //     if( levelData.ambientMode == AmbientMode.Skybox )
        //     {
        //         if( levelData.AmbientIntensity )
		// 		    RenderSettings.ambientIntensity = levelData.ambientIntensity;
        //         else 
		// 		    RenderSettings.ambientLight = levelData.ambientLight;
        //     }
        //     else if( levelData.ambientMode == AmbientMode.Flat )
        //     {
		// 		RenderSettings.ambientLight = levelData.ambientLight;
		// 	}
        //     else if( levelData.ambientMode == AmbientMode.Trilight )
        //     {
		// 		RenderSettings.ambientSkyColor     = levelData.ambientSkyColor;
		// 		RenderSettings.ambientEquatorColor = levelData.ambientEquatorColor;
		// 		RenderSettings.ambientGroundColor  = levelData.ambientGroundColor;
        //     }

        //     // Fog Settings
        //     if( levelData.fogEnabled )
        //     {
		// 		RenderSettings.fog        = levelData.fogEnabled;
		// 		RenderSettings.fogColor   = levelData.fogColor;
		// 		RenderSettings.fogMode    = levelData.fogMode;
		// 		RenderSettings.fogDensity = levelData.fogDensity;
		// 	}
		// }
#endregion
    }
}