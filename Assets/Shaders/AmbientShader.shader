// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmbientSahder"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_ToonShader("Toon Shader", Range( 0 , 1)) = 0
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0.7941176,0.7283973,0)
		_fresnelscale("fresnel scale", Range( 1 , 2)) = -0.9841692
		_fresnelbiasss("fresnel biasss", Range( 0 , 1)) = 0
		_FresnelPower("Fresnel Power", Range( -0.5 , 0.5)) = -0.9841692
		_alphatexture("alpha texture", 2D) = "white" {}
		_WorldChange("WorldChange", Range( 0 , 1)) = 0
		_gymHJ("gymHJ", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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

		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample3;
		uniform float _ToonShader;
		uniform float _WorldChange;
		uniform float4 _Color0;
		uniform sampler2D _alphatexture;
		uniform float4 _alphatexture_ST;
		uniform float _fresnelbiasss;
		uniform float _fresnelscale;
		uniform float _FresnelPower;
		uniform sampler2D _gymHJ;
		uniform float4 _gymHJ_ST;
		uniform float _MaskClipValue = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult4 = dot( i.worldNormal , ase_worldlightDir );
			float temp_output_6_0 = (0.0 + (dotResult4 - 0.0) * (1.0 - 0.0) / (1.0 - 0.0));
			float clampResult12 = clamp( ( temp_output_6_0 * ( temp_output_6_0 + ( 1.0 - _ToonShader ) ) ) , 0.1 , 0.9 );
			float2 appendResult13 = (float2(clampResult12 , 0.0));
			float temp_output_59_0 = ( 1.0 - _WorldChange );
			o.Albedo = ( ( tex2D( _TextureSample1, uv_TextureSample1 ) * tex2D( _TextureSample3, appendResult13 ) ) * temp_output_59_0 ).xyz;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float2 uv_alphatexture = i.uv_texcoord * _alphatexture_ST.xy + _alphatexture_ST.zw;
			float4 tex2DNode34 = tex2D( _alphatexture, uv_alphatexture );
			float fresnelFinalVal40 = (_fresnelbiasss + _fresnelscale*pow( 1.0 - dot( tex2DNode34.xyz, worldViewDir ) , _FresnelPower));
			o.Emission = ( _WorldChange * ( _Color0 * fresnelFinalVal40 ) ).rgb;
			float temp_output_52_0 = _WorldChange;
			o.Alpha = temp_output_52_0;
			float2 uv_gymHJ = i.uv_texcoord * _gymHJ_ST.xy + _gymHJ_ST.zw;
			float4 lerpResult83 = lerp( tex2DNode34 , tex2D( _gymHJ, uv_gymHJ ) , temp_output_59_0);
			clip( lerpResult83 - ( _MaskClipValue ).xxxx );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

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
1922;17;1586;821;3384.529;751.6613;3.693755;True;False
Node;AmplifyShaderEditor.CommentaryNode;88;-2360.618,-153.876;Float;False;2160.646;928.2598;Toon Shader;13;17;15;14;13;12;11;9;10;8;7;6;5;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1;-2332.043,146.1875;Float;False;465;303;Comment;3;3;4;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;3;-2293.043,356.1876;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.WorldNormalVector;2;-2277.993,199.1371;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;4;-1989.043,230.1875;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-2079.976,507.3073;Float;False;Property;_ToonShader;Toon Shader;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;7;-1785.638,478.5976;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;6;-1789.638,240.5974;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-1616.638,387.5975;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-1407.889,430.1503;Float;False;Constant;_Float3;Float 3;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1474.639,283.5974;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-1411.889,552.1505;Float;False;Constant;_Float5;Float 5;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;45;42.47693,942.858;Float;False;Property;_fresnelbiasss;fresnel biasss;7;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;34;14.31441,726.9657;Float;True;Property;_alphatexture;alpha texture;8;0;Assets/Textures/220px-Grid_illusion.svg.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;38;23.56647,1114.386;Float;False;Property;_FresnelPower;Fresnel Power;7;0;-0.9841692;-0.5;0.5;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;12;-1166.081,348.3386;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;32.47691,1028.858;Float;False;Property;_fresnelscale;fresnel scale;7;0;-0.9841692;1;2;0;1;FLOAT
Node;AmplifyShaderEditor.FresnelNode;40;494.1829,855.2398;Float;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;13;-947.889,417.1503;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;15;-788.8304,382.5661;Float;True;Property;_TextureSample3;Texture Sample 3;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WireNode;74;693.4188,567.9786;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;14;-896.2997,93.31252;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;52;71.77328,581.2944;Float;False;Property;_WorldChange;WorldChange;10;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;25;403.0467,302.8761;Float;False;Property;_Color0;Color 0;5;0;0,0.7941176,0.7283973,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;765.0001,402.9965;Float;False;2;2;0;COLOR;0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;59;132.2576,406.9951;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-424.2999,210.3153;Float;True;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;84;864.6046,688.5479;Float;True;Property;_gymHJ;gymHJ;10;0;Assets/Textures/full white.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;964.6344,312.6587;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;213.5477,160.7997;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.LerpOp;83;1112.782,496.7251;Float;False;3;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1299.014,175.4525;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;AmbientSahder;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;7;0;5;0
WireConnection;6;0;4;0
WireConnection;8;0;6;0
WireConnection;8;1;7;0
WireConnection;10;0;6;0
WireConnection;10;1;8;0
WireConnection;12;0;10;0
WireConnection;12;1;9;0
WireConnection;12;2;11;0
WireConnection;40;0;34;0
WireConnection;40;1;45;0
WireConnection;40;2;44;0
WireConnection;40;3;38;0
WireConnection;13;0;12;0
WireConnection;15;1;13;0
WireConnection;74;0;40;0
WireConnection;21;0;25;0
WireConnection;21;1;74;0
WireConnection;59;0;52;0
WireConnection;17;0;14;0
WireConnection;17;1;15;0
WireConnection;87;0;52;0
WireConnection;87;1;21;0
WireConnection;60;0;17;0
WireConnection;60;1;59;0
WireConnection;83;0;34;0
WireConnection;83;1;84;0
WireConnection;83;2;59;0
WireConnection;0;0;60;0
WireConnection;0;2;87;0
WireConnection;0;9;52;0
WireConnection;0;10;83;0
ASEEND*/
//CHKSM=1781617B2333470CC4EEEA5B743DBEABDD4E5B86