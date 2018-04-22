// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "EnemyShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Albedo("Albedo", 2D) = "white" {}
		_Float1("Float 1", Range( -10 , 10)) = 10
		_Lut("Lut", 2D) = "white" {}
		_OpacityMask("OpacityMask", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}
		_Float7("Float 7", Range( 0 , 1)) = 0.8296579
		_Disolve("Disolve", Range( 0 , 0.7)) = 0.7
		_Float13("Float 13", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ "_GrabScreen2" }
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
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
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float4 screenPos;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Lut;
		uniform half _Float1;
		uniform half _Float7;
		uniform fixed _Disolve;
		uniform sampler2D _GrabScreen2;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _OpacityMask;
		uniform float4 _OpacityMask_ST;
		uniform sampler2D _GrabTexture;
		uniform float _ChromaticAberration;
		uniform half _Float13;
		uniform float _MaskClipValue = 0.5;

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
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
				screenPos.w += 0.00000000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( _GrabTexture, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			float temp_output_18_0 = ( 1.0 - _Disolve );
				color.rgb = color.rgb + Refraction( i, o, ( (0.0 + (temp_output_18_0 - 0.0) * (0.001 - 0.0) / (1.0 - 0.0)) + _Float13 ), _ChromaticAberration ) * ( 1 - color.a );
				color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult5 = dot( ase_worldNormal , ase_worldlightDir );
			float clampResult9 = clamp( ( dotResult5 * ( 1.0 - _Float1 ) ) , 0.1 , 0.9 );
			half2 temp_cast_0 = (clampResult9).xx;
			float temp_output_18_0 = ( 1.0 - _Disolve );
			o.Albedo = ( ( ( tex2D( _Albedo, uv_Albedo ) * tex2D( _Lut, temp_cast_0 ) ) * _Float7 ) * temp_output_18_0 ).xyz;
			half4 temp_cast_2 = 0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos19 = ase_screenPos;
			float4 componentMask20 = ase_screenPos19.xyzw;
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 componentMask28 = ( (0.0 + (_Disolve - 0.0) * (0.5 - 0.0) / (1.0 - 0.0)) * UnpackNormal( tex2D( _Normal, uv_Normal ) ) ).xyz;
			fixed4 screenColor29 = tex2Dproj( _GrabScreen2, UNITY_PROJ_COORD( ( componentMask20 + half4( componentMask28 , 0.0 ) ) ) );
			float4 lerpResult31 = lerp( temp_cast_2 , screenColor29 , _Disolve);
			float2 uv_OpacityMask = i.uv_texcoord * _OpacityMask_ST.xy + _OpacityMask_ST.zw;
			float clampResult49 = clamp( (-4.0 + (( (-1.0 + (( 1.0 - _Disolve ) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) + tex2D( _OpacityMask, uv_OpacityMask ).r ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			float2 appendResult53 = (half2(( 1.0 - clampResult49 ) , 0));
			o.Emission = ( lerpResult31 + tex2D( _Albedo, appendResult53 ) ).rgb;
			o.Alpha = 1;
			clip( clampResult49 - _MaskClipValue );
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha finalcolor:RefractionF fullforwardshadows exclude_path:deferred 

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
0;92;983;295;2157.25;-2182.592;2.179409;True;False
Node;AmplifyShaderEditor.RangedFloatNode;42;-2706.361,2040.674;Fixed;False;Property;_Disolve;Disolve;9;0;0.7;0;0.7;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;52;-1977.81,-303.1994;Float;False;2567.195;720.6106;Toon Shader;12;13;14;12;10;11;9;6;8;7;4;3;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-2566.551,2244.441;Float;False;1731.08;835.6802;Disolve;10;40;41;37;49;48;47;46;45;44;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;15;-1819.85,-100.5503;Float;False;465;303;Comment;3;1;5;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;41;-2538.164,2384.98;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;3;-1180.886,270.4952;Float;False;Property;_Float1;Float 1;4;0;10;-10;10;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;47;-2361.563,2416.784;Float;True;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-1.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;16;-2132.099,1047.127;Float;False;2015.758;1031.04;Cloacking;17;30;29;24;28;19;31;27;26;25;23;22;21;20;18;35;34;32;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-1765.8,-47.60073;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-1780.85,109.4497;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;40;-2531.962,2731.483;Float;True;Property;_OpacityMask;OpacityMask;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;26;-1908.519,1677.401;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;5;-1543.151,-43.85033;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;4;-1299.044,194.6047;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-2109.863,2693.586;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;25;-2043.459,1882.475;Float;True;Property;_Normal;Normal;7;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;19;-1513.76,1423.174;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1075.063,-34.33514;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;48;-1995.464,2369.286;Float;True;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-4.0;False;4;FLOAT;4.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;8;-940.7787,192.3145;Float;False;Constant;_Float5;Float 5;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;6;-899.6963,305.4124;Float;False;Constant;_Float3;Float 3;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1662.461,1704.975;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT3;0;False;1;FLOAT3
Node;AmplifyShaderEditor.ClampOpNode;49;-1681.112,2326.342;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;9;-704.5881,95.10071;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ComponentMaskNode;28;-1369.963,1791.975;Float;True;True;True;True;True;1;0;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ComponentMaskNode;20;-1288.962,1394.775;Float;True;True;True;True;True;1;0;FLOAT4;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;10;-326.9155,86.44957;Float;True;Property;_Lut;Lut;5;0;Assets/Textures/Luts/lu7.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;-442.1194,-174.4254;Float;True;Property;_Albedo;Albedo;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;46;-1441.666,2418.936;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.IntNode;37;-1588.939,2792.057;Float;False;Constant;_Int6;Int 6;4;0;0;0;1;INT
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1074.062,1657.475;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;39.32186,-32.40993;Float;True;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.WireNode;23;-1264.442,1324.426;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.IntNode;32;-891.3295,1525.727;Float;False;Constant;_Int5;Int 5;3;0;0;0;1;INT
Node;AmplifyShaderEditor.OneMinusNode;18;-1955.643,1147.727;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;53;-1291.397,2758.199;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;INT;1.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ScreenColorNode;29;-926.0668,1831.676;Fixed;False;Global;_GrabScreen2;Grab Screen 2;3;0;Object;-1;True;1;0;FLOAT4;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;11.51955,203.3665;Float;False;Property;_Float7;Float 7;8;0;0.8296579;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;34;-689.6235,1342.402;Float;True;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;0.001;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;44;-1134.157,2482.523;Float;True;Property;_TextureSample17;Texture Sample 17;10;0;None;True;0;False;white;Auto;False;Instance;11;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;400.6854,-66.29414;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.LerpOp;31;-748.6528,1646.426;Float;False;3;0;INT;0,0,0,0;False;1;COLOR;0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;21;-528.6179,1591.501;Float;False;Property;_Float13;Float 13;11;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-538.2123,1737.5;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT4;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-272.3876,1193.04;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-230.4185,1512.302;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;51;1844.061,1629.919;Float;False;304;481.9999;Comment;1;0;;1,1,1,1;0;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1894.061,1679.919;Half;False;True;2;Half;ASEMaterialInspector;0;0;Standard;EnemyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;2;-1;0;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;41;0;42;0
WireConnection;47;0;41;0
WireConnection;26;0;42;0
WireConnection;5;0;1;0
WireConnection;5;1;2;0
WireConnection;4;0;3;0
WireConnection;45;0;47;0
WireConnection;45;1;40;1
WireConnection;7;0;5;0
WireConnection;7;1;4;0
WireConnection;48;0;45;0
WireConnection;27;0;26;0
WireConnection;27;1;25;0
WireConnection;49;0;48;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;9;2;6;0
WireConnection;28;0;27;0
WireConnection;20;0;19;0
WireConnection;10;1;9;0
WireConnection;46;0;49;0
WireConnection;30;0;20;0
WireConnection;30;1;28;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;23;0;42;0
WireConnection;18;0;42;0
WireConnection;53;0;46;0
WireConnection;53;1;37;0
WireConnection;29;0;30;0
WireConnection;34;0;18;0
WireConnection;44;1;53;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;31;0;32;0
WireConnection;31;1;29;0
WireConnection;31;2;23;0
WireConnection;22;0;31;0
WireConnection;22;1;44;0
WireConnection;24;0;14;0
WireConnection;24;1;18;0
WireConnection;35;0;34;0
WireConnection;35;1;21;0
WireConnection;0;0;24;0
WireConnection;0;2;22;0
WireConnection;0;8;35;0
WireConnection;0;10;49;0
ASEEND*/
//CHKSM=D78A26AD56B25765B4BA088C7DBEBF1EF660C085