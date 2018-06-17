// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "RedDepthFog"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaxFogIntencity("MaxFogIntencity", Range( 0 , 1)) = 0
		_FogIntensity("Fog Intensity", Range( 0 , 1)) = 0.5
		_FogColor("Fog Color", Color) = (0,1,0.5034485,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_FogSpeed("Fog Speed", Range( 0 , 1)) = 0.4705882
		_CloudColor("CloudColor", Color) = (0,0,0,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 texcoord_0;
			float4 screenPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float _FogSpeed;
		uniform float4 _FogColor;
		uniform sampler2D _TextureSample1;
		uniform float4 _CloudColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _FogIntensity;
		uniform float _MaxFogIntencity;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_27_0 = ( _FogSpeed * (abs( i.texcoord_0+_Time[1] * float2(0.05,0.05 ))) );
			o.Albedo = ( tex2D( _TextureSample0, temp_output_27_0 ) * _FogColor ).xyz;
			float4 clampResult29 = clamp( ( float4(0.1,0.1,0.1,0.1) * tex2D( _TextureSample1, temp_output_27_0 ) ) , float4( 0.1,0.1,0.1,0.1 ) , float4( 1,1,1,1 ) );
			o.Emission = ( clampResult29 * _CloudColor ).xyz;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos5 = ase_screenPos;
			float eyeDepth3 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos5))));
			float clampResult15 = clamp( ( abs( ( eyeDepth3 - ase_screenPos5.w ) ) * (0.1 + (_FogIntensity - 0.0) * (0.4 - 0.1) / (1.0 - 0.0)) ) , 0.0 , _MaxFogIntencity );
			o.Alpha = clampResult15;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13101
0;92;938;281;-95.74689;-97.52579;1.102892;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-949.3307,-279.4853;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;5;-993.3504,18.10022;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;20;-596.7217,-250.491;Float;False;0.05;0.05;2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;28;-805.4676,-463.0671;Float;False;Property;_FogSpeed;Fog Speed;5;0;0.4705882;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;3;-684.1797,-27.61456;Float;False;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-519.4907,-423.3582;Float;False;2;2;0;FLOAT;0.0,0;False;1;FLOAT2;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;8;-664.5416,275.342;Float;False;Property;_FogIntensity;Fog Intensity;1;0;0.5;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;4;-449.2783,133.3638;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector4Node;25;134.8247,-96.06355;Float;False;Constant;_Vector0;Vector 0;5;0;0.1,0.1,0.1,0.1;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;24;23.65027,88.20203;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;Assets/Textures/firstheightmap.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;13;-311.508,292.0485;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;0.4;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;12;-279.5754,102.2102;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;459.2174,80.87908;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-162.8522,173.891;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;18;-238.7307,-389.7939;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Assets/Textures/disolve guide.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;17;-94.59167,-182.8732;Float;False;Property;_FogColor;Fog Color;2;0;0,1,0.5034485,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;29;634.7793,131.0413;Float;False;3;0;FLOAT4;0.0;False;1;FLOAT4;0.1,0.1,0.1,0.1;False;2;FLOAT4;1,1,1,1;False;1;FLOAT4
Node;AmplifyShaderEditor.ColorNode;35;489.4792,259.6508;Float;False;Property;_CloudColor;CloudColor;6;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;14;-412.0803,575.207;Float;False;Property;_MaxFogIntencity;MaxFogIntencity;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;205.388,-246.0851;Float;False;2;2;0;FLOAT4;0.0;False;1;COLOR;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;733.1622,259.7539;Float;False;2;2;0;FLOAT4;0.0;False;1;COLOR;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ClampOpNode;15;-43.07085,320.5247;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1224.398,242.1625;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;DepthFog;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;19;0
WireConnection;3;0;5;0
WireConnection;27;0;28;0
WireConnection;27;1;20;0
WireConnection;4;0;3;0
WireConnection;4;1;5;4
WireConnection;24;1;27;0
WireConnection;13;0;8;0
WireConnection;12;0;4;0
WireConnection;26;0;25;0
WireConnection;26;1;24;0
WireConnection;9;0;12;0
WireConnection;9;1;13;0
WireConnection;18;1;27;0
WireConnection;29;0;26;0
WireConnection;21;0;18;0
WireConnection;21;1;17;0
WireConnection;33;0;29;0
WireConnection;33;1;35;0
WireConnection;15;0;9;0
WireConnection;15;2;14;0
WireConnection;0;0;21;0
WireConnection;0;2;33;0
WireConnection;0;9;15;0
ASEEND*/
//CHKSM=D52C4D8F0B390AFE1AC80D4C745ABF389529FDDF