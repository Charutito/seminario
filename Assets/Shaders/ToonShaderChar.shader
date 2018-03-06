// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonShaderChar"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		[HideInInspector]_SpecColor("SpecularColor",Color)=(1,1,1,1)
		_ToonScale("ToonScale", Range( 0 , 1)) = 0
		_ToonRamp("ToonRamp", 2D) = "white" {}
		_DiffuseReal("DiffuseReal", 2D) = "white" {}
		_DiffuseGhost("DiffuseGhost", 2D) = "white" {}
		_WorldSelect("WorldSelect", Range( 0 , 1)) = 0.2959003
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
		};

		uniform sampler2D _DiffuseReal;
		uniform float4 _DiffuseReal_ST;
		uniform sampler2D _DiffuseGhost;
		uniform float4 _DiffuseGhost_ST;
		uniform float _WorldSelect;
		uniform sampler2D _ToonRamp;
		uniform float _ToonScale;

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_DiffuseReal = i.uv_texcoord * _DiffuseReal_ST.xy + _DiffuseReal_ST.zw;
			float2 uv_DiffuseGhost = i.uv_texcoord * _DiffuseGhost_ST.xy + _DiffuseGhost_ST.zw;
			float4 lerpResult28 = lerp( tex2D( _DiffuseReal, uv_DiffuseReal ) , tex2D( _DiffuseGhost, uv_DiffuseGhost ) , _WorldSelect);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult4 = dot( i.worldNormal , ase_worldlightDir );
			float temp_output_12_0 = (0.0 + (dotResult4 - 0.0) * (1.0 - 0.0) / (1.0 - 0.0));
			float clampResult18 = clamp( ( temp_output_12_0 * ( temp_output_12_0 + ( 1.0 - _ToonScale ) ) ) , 0.1 , 0.9 );
			float2 appendResult21 = (float2(clampResult18 , 0.0));
			o.Emission = ( lerpResult28 * tex2D( _ToonRamp, appendResult21 ) ).rgb;
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
0;92;1476;655;3611.68;1009.872;3.360348;True;False
Node;AmplifyShaderEditor.WorldNormalVector;5;-1970.268,-239.0362;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;3;-1985.318,-81.98576;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;17;-1491.395,10.61716;Float;False;Property;_ToonScale;ToonScale;0;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;4;-1681.318,-207.9858;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;12;-1447.057,-200.0926;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;13;-1197.057,-18.09253;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-1028.057,-109.0925;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-886.0573,-213.0926;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;20;-619.9489,214.0774;Float;False;Constant;_Float1;Float 1;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;-609.8009,-5.063939;Float;False;Constant;_Float0;Float 0;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;18;-406.4504,-61.29325;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;27;-441.1755,-670.2032;Float;True;Property;_DiffuseGhost;DiffuseGhost;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;26;-491.1989,-891.2033;Float;True;Property;_DiffuseReal;DiffuseReal;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;21;-260.9806,180.5508;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;29;-400.2202,-428.2086;Float;False;Property;_WorldSelect;WorldSelect;4;0;0.2959003;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;22;-179.8602,-85.40076;Float;True;Property;_ToonRamp;ToonRamp;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;28;19.17981,-701.2084;Float;False;3;0;COLOR;0.0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;304.8859,-158.4663;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT4;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;606.6598,-151.9761;Float;False;True;2;Float;ASEMaterialInspector;0;0;BlinnPhong;ToonShaderChar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;14;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;16;-2024.318,-291.9858;Float;False;465;303;Comment;0;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;31;-518.6451,-986.3786;Float;False;678.6934;677.2517;Comment;0;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;32;-635.7036,-164.6794;Float;False;755.7354;535.2078;Comment;0;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;30;-1499.934,-353.4318;Float;False;742.2501;536.2103;Comment;0;;1,1,1,1;0;0
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
WireConnection;28;0;26;0
WireConnection;28;1;27;0
WireConnection;28;2;29;0
WireConnection;24;0;28;0
WireConnection;24;1;22;0
WireConnection;0;2;24;0
ASEEND*/
//CHKSM=5213B089418B2DDB7CA7948F70B8F32FA3D73140