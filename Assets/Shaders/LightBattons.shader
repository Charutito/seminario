// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Light Batton Shader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Intencity("Intencity", Range( 0 , 1)) = 1
		_MaxIntencity("MaxIntencity", Range( 0 , 1)) = 0
		_LightStrengt("LightStrengt", Range( 0 , 1)) = 0.4089456
		_Color2("Color 2", Color) = (0,0.7941176,0.7283973,0)
		_2312v2("2312-v2", 2D) = "white" {}
		_fresnelbias("fresnel bias", Range( 0 , 1)) = 0
		_fresnelPower("fresnel Power", Range( -0.5 , 0.5)) = -0.9841692
		_fresnelscale("fresnelscale", Range( 1 , 2)) = -0.9841692
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _LightStrengt;
		uniform float4 _Color2;
		uniform float _fresnelbias;
		uniform float _fresnelscale;
		uniform float _fresnelPower;
		uniform sampler2D _2312v2;
		uniform float4 _2312v2_ST;
		uniform sampler2D _CameraDepthTexture;
		uniform float _Intencity;
		uniform float _MaxIntencity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelFinalVal10 = (_fresnelbias + _fresnelscale*pow( 1.0 - dot( ase_worldNormal, worldViewDir ) , _fresnelPower));
			float2 uv_2312v2 = i.uv_texcoord * _2312v2_ST.xy + _2312v2_ST.zw;
			float4 tex2DNode14 = tex2D( _2312v2, uv_2312v2 );
			o.Emission = ( _LightStrengt * ( _Color2 * fresnelFinalVal10 ) * tex2DNode14 ).rgb;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos17 = ase_screenPos;
			ase_screenPos17.xyzw /= ase_screenPos17.w;
			float eyeDepth19 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos17))));
			float clampResult23 = clamp( ( ( eyeDepth19 - ase_screenPos17.w ) * (0.0 + (_Intencity - 0.0) * (0.5 - 0.0) / (1.0 - 0.0)) ) , 0.0 , _MaxIntencity );
			o.Alpha = ( clampResult23 * tex2DNode14 ).x;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

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
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
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
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
0;92;1181;439;1000.548;231.4613;2.825767;True;True
Node;AmplifyShaderEditor.ScreenPosInputsNode;17;108.6572,514.449;Float;False;0;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;19;361.5612,452.2466;Float;False;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;18;198.4311,707.4799;Float;False;Property;_Intencity;Intencity;0;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-838.3478,904.679;Float;False;Property;_fresnelbias;fresnel bias;5;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;12;-848.3478,990.679;Float;False;Property;_fresnelscale;fresnelscale;7;0;-0.9841692;1;2;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;604.8942,559.2538;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;21;615.4739,746.8519;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-857.2582,1076.207;Float;False;Property;_fresnelPower;fresnel Power;6;0;-0.9841692;-0.5;0.5;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;836.1282,726.6393;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.FresnelNode;10;-386.6417,817.0608;Float;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;16;553.503,955.2288;Float;False;Property;_MaxIntencity;MaxIntencity;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;8;-457.6691,361.1343;Float;False;Property;_Color2;Color 2;4;0;0,0.7941176,0.7283973,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-793.8757,302.1009;Float;False;Property;_LightStrengt;LightStrengt;1;0;0.4089456;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-113.8182,321.3761;Float;False;2;2;0;COLOR;0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;14;-127.843,-206.7086;Float;True;Property;_2312v2;2312-v2;5;0;Assets/Textures/2312-v2.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;23;888.8042,912.432;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;559.1772,-25.83427;Float;True;3;3;0;FLOAT;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT4;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;984.0159,539.735;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1446.435,149.548;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Light Batton Shader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;17;0
WireConnection;20;0;19;0
WireConnection;20;1;17;4
WireConnection;21;0;18;0
WireConnection;22;0;20;0
WireConnection;22;1;21;0
WireConnection;10;1;11;0
WireConnection;10;2;12;0
WireConnection;10;3;13;0
WireConnection;7;0;8;0
WireConnection;7;1;10;0
WireConnection;23;0;22;0
WireConnection;23;2;16;0
WireConnection;9;0;5;0
WireConnection;9;1;7;0
WireConnection;9;2;14;0
WireConnection;24;0;23;0
WireConnection;24;1;14;0
WireConnection;0;2;9;0
WireConnection;0;9;24;0
ASEEND*/
//CHKSM=E0886141BADE112DF7DB8ED2F8A9A7AB99415D49