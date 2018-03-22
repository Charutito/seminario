// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ParalaxShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MidDistance("MidDistance", Range( 0 , 1)) = 0
		_FrontDIstance("FrontDIstance", Range( 0 , 1)) = 0
		_Backdistance("Backdistance", Range( 0 , 1)) = 0
		_fondo_back("fondo_back", 2D) = "white" {}
		_fondo_mid("fondo_mid", 2D) = "white" {}
		_fondo_front("fondo_front", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
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
			float2 texcoord_0;
			float3 viewDir;
			INTERNAL_DATA
		};

		uniform sampler2D _fondo_back;
		uniform float _Backdistance;
		uniform sampler2D _fondo_mid;
		uniform float _MidDistance;
		uniform sampler2D _fondo_front;
		uniform float _FrontDIstance;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 Offset3 = ( ( 0.0 - 1.0 ) * i.viewDir.xy * _Backdistance ) + i.texcoord_0;
			float2 Offset6 = ( ( 0.0 - 1.0 ) * i.viewDir.xy * _MidDistance ) + i.texcoord_0;
			float4 tex2DNode11 = tex2D( _fondo_mid,Offset6);
			float2 Offset7 = ( ( _FrontDIstance - 1.0 ) * i.viewDir.xy * 0.0 ) + i.texcoord_0;
			float4 tex2DNode13 = tex2D( _fondo_front,Offset7);
			o.Emission = lerp( lerp( tex2D( _fondo_back,Offset3) , tex2DNode11 , tex2DNode11.a ) , tex2DNode13 , tex2DNode13.a ).xyz;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha vertex:vertexDataFunc 

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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = worldViewDir;
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
Version=5105
4;100;1483;655;1887.577;553.4632;2.2;False;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1052.668,-491.2807;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1377.6,-319.6994;Float;True;Property;_Backdistance;Backdistance;0;0;0;0;1
Node;AmplifyShaderEditor.ParallaxMappingNode;3;-619.2,-141.9;Float;False;Normal;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.ParallaxMappingNode;6;-599.5921,101.8424;Float;False;Normal;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1358.614,173.8002;Float;True;Property;_MidDistance;MidDistance;0;0;0;0;1
Node;AmplifyShaderEditor.RangedFloatNode;8;-1334.99,464.0429;Float;True;Property;_FrontDIstance;FrontDIstance;0;0;0;0;1
Node;AmplifyShaderEditor.ParallaxMappingNode;7;-599.3878,343.6429;Float;False;Normal;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;9;-298.3618,-173.379;Float;True;Property;_fondo_back;fondo_back;3;0;Assets/Materials/Imagenes/Paralax/fondo_back.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;11;-304.1066,82.81628;Float;True;Property;_fondo_mid;fondo_mid;4;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.LerpOp;10;44.69348,-120.7838;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.LerpOp;12;116.2936,302.0163;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False
Node;AmplifyShaderEditor.SamplerNode;13;-263.7064,326.5163;Float;True;Property;_fondo_front;fondo_front;5;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;472.2876,-105.9763;Float;False;True;2;Float;ASEMaterialInspector;Standard;ParalaxShader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;2;-1142.022,-55.81375;Float;False;World
WireConnection;3;0;1;0
WireConnection;3;2;4;0
WireConnection;3;3;2;0
WireConnection;6;0;1;0
WireConnection;6;2;5;0
WireConnection;6;3;2;0
WireConnection;7;0;1;0
WireConnection;7;1;8;0
WireConnection;7;3;2;0
WireConnection;9;1;3;0
WireConnection;11;1;6;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;10;2;11;4
WireConnection;12;0;10;0
WireConnection;12;1;13;0
WireConnection;12;2;13;4
WireConnection;13;1;7;0
WireConnection;0;2;12;0
ASEEND*/
//CHKSM=D1AA63895EA7188940B5AF6B1E249A6BCC413C5D