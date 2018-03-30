// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SwordShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Float1("Float 1", Range( -10 , 10)) = 10
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Float7("Float 7", Range( 0 , 1)) = 0.8296579
		_lightMask("lightMask", 2D) = "white" {}
		_FresnelBias("FresnelBias", Range( 0 , 1)) = 0
		_Glowcolor("Glow color", Color) = (1,0,0,0)
		_FresnelPower("FresnelPower", Range( -0.5 , 0.5)) = -0.9841692
		_FresnelScale("FresnelScale", Range( 1 , 2)) = -0.9841692
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
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
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
			INTERNAL_DATA
		};

		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform sampler2D _TextureSample1;
		uniform float _Float1;
		uniform float _Float7;
		uniform sampler2D _lightMask;
		uniform float4 _lightMask_ST;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;
		uniform float4 _Glowcolor;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult5 = dot( i.worldNormal , ase_worldlightDir );
			float clampResult9 = clamp( ( dotResult5 * ( 1.0 - _Float1 ) ) , 0.1 , 0.9 );
			float2 temp_cast_0 = (clampResult9).xx;
			float2 uv_lightMask = i.uv_texcoord * _lightMask_ST.xy + _lightMask_ST.zw;
			float4 tex2DNode16 = tex2D( _lightMask, uv_lightMask );
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelFinalVal18 = (_FresnelBias + _FresnelScale*pow( 1.0 - dot( ase_worldNormal, worldViewDir ) , _FresnelPower));
			o.Emission = ( ( ( ( tex2D( _TextureSample3, uv_TextureSample3 ) * tex2D( _TextureSample1, temp_cast_0 ) ) * _Float7 ) - tex2DNode16 ) + ( tex2DNode16 * ( fresnelFinalVal18 * _Glowcolor ) ) ).xyz;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
0;92;905;295;1235.5;-78.2216;1.909303;True;False
Node;AmplifyShaderEditor.CommentaryNode;15;-2426.419,15.20914;Float;False;465;303;Comment;3;5;2;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1787.454,386.2548;Float;False;Property;_Float1;Float 1;1;0;10;-10;10;0;1;FLOAT
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-2387.419,225.2092;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.WorldNormalVector;1;-2372.369,68.15876;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;5;-2149.72,71.90915;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;4;-1905.613,310.3642;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;8;-1547.347,308.074;Float;False;Constant;_Float5;Float 5;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1681.631,81.42435;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;6;-1506.264,421.1718;Float;False;Constant;_Float3;Float 3;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;9;-1311.155,210.8602;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;10;-930.4816,203.2091;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Assets/Textures/Luts/lu7.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;-1048.686,-58.66587;Float;True;Property;_TextureSample3;Texture Sample 3;0;0;Assets/Models/Materials/katanaTexture.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;22;-886.1811,943.9521;Float;False;Property;_FresnelPower;FresnelPower;7;0;-0.9841692;-0.5;0.5;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;23;-867.2706,772.4241;Float;False;Property;_FresnelBias;FresnelBias;5;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;21;-875.8337,858.4242;Float;False;Property;_FresnelScale;FresnelScale;8;0;-0.9841692;1;2;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-567.2443,83.34955;Float;True;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;13;-490.1851,323.4354;Float;False;Property;_Float7;Float 7;3;0;0.8296579;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.FresnelNode;18;-535.2328,746.1669;Float;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;20;-541.41,913.4662;Float;False;Property;_Glowcolor;Glow color;6;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-205.8808,49.46535;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;16;-668.6729,440.026;Float;True;Property;_lightMask;lightMask;4;0;Assets/Textures/lightMask.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-194.3427,699.3247;Float;True;2;2;0;FLOAT;0.0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-147.1969,309.2474;Float;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-46.42412,555.4724;Float;True;2;2;0;FLOAT4;0.0;False;1;COLOR;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleAddOpNode;25;179.2846,306.2182;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;430.5135,67.42091;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;SwordShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;5;1;2;0
WireConnection;4;0;3;0
WireConnection;7;0;5;0
WireConnection;7;1;4;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;9;2;6;0
WireConnection;10;1;9;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;18;1;23;0
WireConnection;18;2;21;0
WireConnection;18;3;22;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;19;0;18;0
WireConnection;19;1;20;0
WireConnection;27;0;14;0
WireConnection;27;1;16;0
WireConnection;24;0;16;0
WireConnection;24;1;19;0
WireConnection;25;0;27;0
WireConnection;25;1;24;0
WireConnection;0;2;25;0
ASEEND*/
//CHKSM=E56660F7F3F6348928119ADD779EE0F32F1A6285