/* Created by and for usage of FF Studios (2021). */

using System;
using UnityEngine;
using UnityEngine.Rendering;
using NaughtyAttributes;

namespace FFStudio
{
	[ CreateAssetMenu( fileName = "LevelData", menuName = "FF/Data/LevelData" ) ]
	public class LevelData : ScriptableObject
    {
        [ Scene() ]
		public int sceneIndex;

        // [ Header( "Skybox" ), HorizontalLine ]
        // [ BoxGroup( "Render Settings" ) ] public Material skyboxMaterial;
		// [BoxGroup( "Render Settings" ), Label( "Realtime Shadow Color" )] public Color subtractiveShadowColor;


		// [ Header( "Environment Lighting" ), HorizontalLine ]
        // //Ambient Lighting
        // [ BoxGroup( "Render Settings" ), Label( "Environment Lighting Mode" ) ] public AmbientMode ambientMode;
        // [ BoxGroup( "Render Settings" ), Label( "Intensity Multiplier" ), ShowIf( "AmbientIntensity" ) ] public float ambientIntensity;
        // [ BoxGroup( "Render Settings" ), Label( "Ambient Color" ), ShowIf( "AmbientColor" ) ] public Color ambientLight;
        // [ BoxGroup( "Render Settings" ), Label( "Sky Color" ), ShowIf( "AmbientGradient" ) ] public Color ambientSkyColor;
        // [ BoxGroup( "Render Settings" ), Label( "Equator Color" ), ShowIf( "AmbientGradient" ) ] public Color ambientEquatorColor;
        // [ BoxGroup( "Render Settings" ), Label( "Ground Color" ), ShowIf( "AmbientGradient" ) ] public Color ambientGroundColor;

		
        // //Fog
        // [ HorizontalLine ]
        // [ BoxGroup( "Render Settings" ), Label( "Fog" ) ] public bool fogEnabled;
        // [ BoxGroup( "Render Settings" ), Label( "Color" ), ShowIf( "FogEnabled" ) ] public Color fogColor;
        // [ BoxGroup( "Render Settings" ), Label( "Mode" ), ShowIf( "FogEnabled" ) ] public FogMode fogMode;
        // [ BoxGroup( "Render Settings" ), Label( "Density" ), ShowIf( "FogEnabled" ) ] public float fogDensity;


		// public bool AmbientIntensity => ambientMode == AmbientMode.Skybox && skyboxMaterial != null;
		// public bool AmbientSkyBox    => ambientMode == AmbientMode.Skybox;
		// public bool AmbientColor     => ambientMode == AmbientMode.Flat || ( skyboxMaterial == null && ambientMode == AmbientMode.Skybox );
		// public bool AmbientGradient  => ambientMode == AmbientMode.Trilight;
		// public bool FogEnabled       => fogEnabled;
	}
}
