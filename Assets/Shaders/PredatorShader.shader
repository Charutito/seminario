// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PredatorShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Heightmap("Heightmap", 2D) = "white" {}
		_NormalDist("NormalDist", 2D) = "bump" {}
		_Imbecibilidad("Imbecibilidad", Range( 0 , 0.7)) = 0.7
		_Albedo("Albedo", 2D) = "white" {}
		_REfAmount("REfAmount", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
	}

	SubShader
	{
		Tags{ "RenderType" = "TreeTransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		Blend One Zero , One One
		BlendOp Add , Add
		GrabPass{ "RefractionGrab0" }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform fixed _Imbecibilidad;
		uniform sampler2D _GrabScreen0;
		uniform sampler2D _NormalDist;
		uniform float4 _NormalDist_ST;
		uniform sampler2D _Heightmap;
		uniform float4 _Heightmap_ST;
		uniform sampler2D RefractionGrab0;
		uniform float _ChromaticAberration;
		uniform half _REfAmount;
		uniform float _MaskClipValue = 0.5;

		inline float4 Refraction( Input i, SurfaceOutputStandardSpecular o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.0000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( RefractionGrab0, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( RefractionGrab0, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( RefractionGrab0, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandardSpecular o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			float temp_output_19_0 = ( 1.0 - _Imbecibilidad );
				color.rgb = color.rgb + Refraction( i, o, ( (0.0 + (temp_output_19_0 - 0.0) * (0.001 - 0.0) / (1.0 - 0.0)) + _REfAmount ), _ChromaticAberration ) * ( 1 - color.a );
				color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float temp_output_19_0 = ( 1.0 - _Imbecibilidad );
			o.Albedo = ( tex2D( _Albedo,uv_Albedo) * temp_output_19_0 ).xyz;
			half4 temp_cast_1 = 0;
			float4 componentMask7 = i.screenPos.xyzw;
			float2 uv_NormalDist = i.uv_texcoord * _NormalDist_ST.xy + _NormalDist_ST.zw;
			float3 componentMask5 = ( (0.0 + (_Imbecibilidad - 0.0) * (0.5 - 0.0) / (1.0 - 0.0)) * UnpackNormal( tex2D( _NormalDist,uv_NormalDist) ) ).xyz;
			float2 uv_Heightmap = i.uv_texcoord * _Heightmap_ST.xy + _Heightmap_ST.zw;
			float temp_output_32_0 = clamp( (-4.0 + (( (-1.0 + (( 1.0 - _Imbecibilidad ) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) + tex2D( _Heightmap,uv_Heightmap).r ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float2 appendResult33 = float2( ( 1.0 - temp_output_32_0 ) , (float)0 );
			o.Emission = ( lerp( temp_cast_1 , tex2Dproj( _GrabScreen0, UNITY_PROJ_COORD( ( componentMask7 + half4( componentMask5 , 0.0 ) ) ) ) , _Imbecibilidad ) + tex2D( _Albedo,appendResult33) ).xyz;
			o.Alpha = temp_output_19_0;
			clip( temp_output_32_0 - _MaskClipValue );
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
		CGPROGRAM
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#pragma surface surf StandardSpecular keepalpha finalcolor:RefractionF exclude_path:deferred 

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
			#pragma multi_compile_instancing
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 )
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
				surfIN.uv_texcoord = IN.texcoords01.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutputStandardSpecular o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardSpecular, o )
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
Version=5105
1920;260;1230;475;3660.905;-111.4188;3.1;True;True
Node;AmplifyShaderEditor.CommentaryNode;47;-2046.234,-422.949;Float;False;2015.758;1031.04;Cloacking;18;30;43;29;24;42;53;8;2;7;9;55;51;28;6;5;4;17;19;
Node;AmplifyShaderEditor.CommentaryNode;46;-2480.686,774.3657;Float;False;1731.08;835.6802;Disolve;11;40;41;38;34;36;33;32;39;37;35;64;
Node;AmplifyShaderEditor.OneMinusNode;19;-1869.778,-322.3489;Float;True;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;17;-778.8707,-391.2491;Float;True;Property;_Albedo;Albedo;3;0;Assets/gallo_diff.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;6;-1427.895,-46.90139;Float;False;False;False
Node;AmplifyShaderEditor.ComponentMaskNode;7;-1203.097,-75.30059;Float;True;True;True;True;True;0;FLOAT4;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;53;-442.7525,121.4252;Float;False;Property;_REfAmount;REfAmount;4;0;0;0;1
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-452.3469,267.4245;Float;True;0;COLOR;0.0;False;1;FLOAT4;0;False
Node;AmplifyShaderEditor.WireNode;29;-1178.577,-145.6496;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-327.1801,-230.1501;Float;False;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;205.6998,-8.59993;Half;False;True;2;Half;ASEMaterialInspector;StandardSpecular;PredatorShader;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Custom;0.5;True;True;0;True;TreeTransparentCutout;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;4;One;One;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;2;-1957.594,413.3992;Float;True;Property;_NormalDist;NormalDist;1;0;Assets/Plasma_1.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.TFHCRemap;43;-1822.654,207.3254;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;0.5;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1576.596,234.8991;Float;True;0;FLOAT;0.0;False;1;FLOAT3;0;False
Node;AmplifyShaderEditor.ComponentMaskNode;5;-1284.098,321.899;Float;True;True;True;True;True;0;FLOAT3;0,0,0,0;False
Node;AmplifyShaderEditor.ScreenColorNode;9;-840.2014,361.5997;Fixed;False;Global;_GrabScreen0;Grab Screen 0;3;0;Object;-1;0;FLOAT4;0,0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-988.1962,187.3988;Float;False;0;FLOAT4;0.0;False;1;FLOAT3;0,0,0,0;False
Node;AmplifyShaderEditor.LerpOp;28;-662.7875,176.3501;Float;False;0;INT;0,0,0,0;False;1;COLOR;0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.IntNode;24;-805.4642,55.65089;Float;False;Constant;_Int1;Int 1;3;0;0
Node;AmplifyShaderEditor.WireNode;61;-5.703595,-78.52332;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.TFHCRemap;51;-603.7582,-127.6735;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;0.001;False
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-144.5531,42.22575;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.WireNode;63;78.3949,-31.82214;Float;False;0;FLOAT4;0.0;False
Node;AmplifyShaderEditor.IntNode;34;-1503.074,1321.981;Float;False;Constant;_Int2;Int 2;4;0;0
Node;AmplifyShaderEditor.WireNode;64;-1128.509,849.5784;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.WireNode;62;-47.00348,758.1763;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;37;-2446.097,1261.407;Float;True;Property;_Heightmap;Heightmap;0;0;Assets/Plasma_2.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.OneMinusNode;41;-2452.298,914.9047;Float;False;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-2620.496,570.5982;Fixed;False;Property;_Imbecibilidad;Imbecibilidad;2;0;0.7;0;0.7
Node;AmplifyShaderEditor.AppendNode;33;-1194.276,1197.604;Float;False;FLOAT2;0;0;0;0;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;35;-1067.183,1000.992;Float;True;Property;_Alvedo2;Alvedo2;4;0;Assets/gallo_diff.jpg;True;0;False;white;Auto;False;Instance;17;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-2023.998,1223.51;Float;True;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False
Node;AmplifyShaderEditor.OneMinusNode;36;-1355.801,948.8597;Float;True;0;FLOAT;0.0;False
Node;AmplifyShaderEditor.TFHCRemap;40;-2275.697,946.7084;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-1.0;False;4;FLOAT;1.0;False
Node;AmplifyShaderEditor.TFHCRemap;39;-1909.599,899.2101;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-4.0;False;4;FLOAT;4.0;False
Node;AmplifyShaderEditor.ClampOpNode;32;-1595.247,856.2659;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
WireConnection;19;0;3;0
WireConnection;7;0;6;0
WireConnection;42;0;28;0
WireConnection;42;1;35;0
WireConnection;29;0;3;0
WireConnection;30;0;17;0
WireConnection;30;1;19;0
WireConnection;0;0;63;0
WireConnection;0;2;42;0
WireConnection;0;8;55;0
WireConnection;0;9;61;0
WireConnection;0;10;62;0
WireConnection;43;0;3;0
WireConnection;4;0;43;0
WireConnection;4;1;2;0
WireConnection;5;0;4;0
WireConnection;9;0;8;0
WireConnection;8;0;7;0
WireConnection;8;1;5;0
WireConnection;28;0;24;0
WireConnection;28;1;9;0
WireConnection;28;2;29;0
WireConnection;61;0;19;0
WireConnection;51;0;19;0
WireConnection;55;0;51;0
WireConnection;55;1;53;0
WireConnection;63;0;30;0
WireConnection;64;0;32;0
WireConnection;62;0;64;0
WireConnection;41;0;3;0
WireConnection;33;0;36;0
WireConnection;33;1;34;0
WireConnection;35;1;33;0
WireConnection;38;0;40;0
WireConnection;38;1;37;1
WireConnection;36;0;32;0
WireConnection;40;0;41;0
WireConnection;39;0;38;0
WireConnection;32;0;39;0
ASEEND*/
//CHKSM=DF6FBD99AD9E08A7CC25CF35F335BEBBB9FE0010