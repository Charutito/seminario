// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonShaderAMp"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		[HideInInspector]_SpecColor("SpecularColor",Color)=(1,1,1,1)
		_Dioffuse("Dioffuse", 2D) = "white" {}
		_ToonScale("ToonScale", Range( 0 , 1)) = 0
		_ToonRamp("ToonRamp", 2D) = "white" {}
		_Float2("Float 2", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ "_GrabTexture" }
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
			float4 screenPos;
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
		};

		uniform sampler2D _GrabTexture;
		uniform sampler2D _Dioffuse;
		uniform float4 _Dioffuse_ST;
		uniform sampler2D _ToonRamp;
		uniform float _ToonScale;
		uniform float _Float2;

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 screenPos30 = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
			float scale30 = -1.0;
			#else
			float scale30 = 1.0;
			#endif
			float halfPosW30 = screenPos30.w * 0.5;
			screenPos30.y = ( screenPos30.y - halfPosW30 ) * _ProjectionParams.x* scale30 + halfPosW30;
			screenPos30.w += 0.00000000001;
			screenPos30.xyzw /= screenPos30.w;
			float4 screenColor30 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( screenPos30 ) );
			float2 uv_Dioffuse = i.uv_texcoord * _Dioffuse_ST.xy + _Dioffuse_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult4 = dot( i.worldNormal , ase_worldlightDir );
			float temp_output_12_0 = (0.0 + (dotResult4 - 0.0) * (1.0 - 0.0) / (1.0 - 0.0));
			float clampResult18 = clamp( ( temp_output_12_0 * ( temp_output_12_0 + ( 1.0 - _ToonScale ) ) ) , 0.1 , 0.9 );
			float2 appendResult21 = (float2(clampResult18 , 0.0));
			float4 lerpResult31 = lerp( screenColor30 , ( tex2D( _Dioffuse, uv_Dioffuse ) * tex2D( _ToonRamp, appendResult21 ) ) , _Float2);
			o.Emission = lerpResult31.xyz;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf BlinnPhong keepalpha fullforwardshadows 

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
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
0;92;1478;522;849.5453;626.0905;1.671569;True;False
Node;AmplifyShaderEditor.CommentaryNode;16;-1745.318,-240.9858;Float;False;465;303;Comment;3;3;5;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;5;-1691.268,-188.0362;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;3;-1706.318,-30.98576;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.DotProductOpNode;4;-1402.318,-156.9858;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;17;-1493.251,120.1338;Float;False;Property;_ToonScale;ToonScale;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;12;-1202.913,-146.5759;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;13;-1198.913,91.42411;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1029.913,0.4241428;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;-821.1644,42.9769;Float;False;Constant;_Float0;Float 0;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-887.9135,-103.5759;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;20;-825.1644,164.9769;Float;False;Constant;_Float1;Float 1;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;18;-579.3564,-38.83475;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;21;-361.1644,29.9769;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;6;-367.5876,-314.8608;Float;True;Property;_Dioffuse;Dioffuse;0;0;Assets/Materials/Imagenes/color viva A2.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;22;-202.1057,-4.607208;Float;True;Property;_ToonRamp;ToonRamp;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;28;107.3479,81.18493;Float;False;Property;_Float2;Float 2;3;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;113.8537,-172.8454;Float;True;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ScreenColorNode;30;98.60889,-390.907;Float;False;Global;_GrabScreen0;Grab Screen 0;4;0;Object;-1;False;1;0;FLOAT2;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;31;310.6089,-333.907;Float;False;3;0;COLOR;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;547.6598,-242.9761;Float;False;True;2;Float;ASEMaterialInspector;0;0;BlinnPhong;ToonShaderAMp;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Translucent;0.5;True;True;0;False;Opaque;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;4;1;3;0
WireConnection;12;0;4;0
WireConnection;13;0;17;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;15;0;12;0
WireConnection;15;1;14;0
WireConnection;18;0;15;0
WireConnection;18;1;19;0
WireConnection;18;2;20;0
WireConnection;21;0;18;0
WireConnection;22;1;21;0
WireConnection;24;0;6;0
WireConnection;24;1;22;0
WireConnection;31;0;30;0
WireConnection;31;1;24;0
WireConnection;31;2;28;0
WireConnection;0;2;31;0
ASEEND*/
//CHKSM=2B1D9441506418E86EDFCF5520CDA200F20DC222