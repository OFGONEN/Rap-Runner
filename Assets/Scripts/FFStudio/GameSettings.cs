﻿/* Created by and for usage of FF Studios (2021). */

using NaughtyAttributes;
using UnityEngine;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
#region Fields
        public int maxLevelCount;
        [ Foldout( "UI Settings" ), Tooltip( "Duration of the movement for ui element"          ) ] public float ui_Entity_Move_TweenDuration;
        [ Foldout( "UI Settings" ), Tooltip( "Duration of the fading for ui element"            ) ] public float ui_Entity_Fade_TweenDuration;
		[ Foldout( "UI Settings" ), Tooltip( "Duration of the scaling for ui element"           ) ] public float ui_Entity_Scale_TweenDuration;
		[ Foldout( "UI Settings" ), Tooltip( "Duration of the movement for floating ui element" ) ] public float ui_Entity_FloatingMove_TweenDuration;
        [ Foldout( "UI Settings" ), Tooltip( "Percentage of the screen to register a swipe"     ) ] public int swipeThreshold;

        //Player
		[ BoxGroup( "Player" ), Tooltip( "Player's target point check distance" ) ] public float player_target_checkDistance = 0.1f;
		[ BoxGroup( "Player" ), Tooltip( "Player's rotation clamp value" ) ] public float player_clamp_rotation = 30;
		[ BoxGroup( "Player" ), Tooltip( "Player's vertical movement speed" ) ] public float player_speed_vertical = 10f;
		[ BoxGroup( "Player" ), Tooltip( "Player's horizontal movement speed" ) ] public float player_speed_horizontal = 8f;
		[ BoxGroup( "Player" ), Tooltip( "Player's rotation movement speed" ) ] public float player_speed_rotation = 10f;
		[ BoxGroup( "Player" ), Tooltip( "Player model's turning speed" ) ] public float player_speed_turning = 20f;
		[ BoxGroup( "Player" ), Tooltip( "Player's approach to obstacle speed" ) ] public float player_speed_approach = 5f;
		[ BoxGroup( "Player" ), Tooltip( "Player's Catwalk status depleting speed" ) ] public float player_speed_statusDepleting = 5f;
		[ BoxGroup( "Player" ), Tooltip( "Player's interaction duration with obstacle" ) ] public float player_duration_obstacleInteraction = 1.5f;

        // Camera
		[ BoxGroup( "Camera" ), Tooltip( "Camera's target follow speed" ) ] public float camera_speed_follow = 1f;
		[ BoxGroup( "Camera" ), Tooltip( "Camera's movement duration" ) ] public float camera_duration_movement = 1f;
		[ BoxGroup( "Camera" ), Tooltip( "Camera's end game rotation clamp" ) ] public float camera_clamp_LevelEndRotation = 20f;
		[ BoxGroup( "Camera" ), Tooltip( "Camera's end game rotation speed" ) ] public float camera_speed_LevelEndRotation = 3f;

        private static GameSettings instance;

        private delegate GameSettings ReturnGameSettings();
        private static ReturnGameSettings returnInstance = LoadInstance;

        public static GameSettings Instance
        {
            get
            {
                return returnInstance();
            }
        }
#endregion

#region Implementation
        static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "game_settings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		static GameSettings ReturnInstance()
        {
            return instance;
        }
#endregion
    }
}
