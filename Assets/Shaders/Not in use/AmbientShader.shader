// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmbientShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Alvedo1("Alvedo 1", 2D) = "white" {}
		_ToonShader("Toon Shader", Range( 0 , 1)) = 0
		_Lut("Lut", 2D) = "white" {}
		_Color0("Color 0", Color) = (0,0.7941176,0.7283973,0)
		_fresnelbiasss("fresnel biasss", Range( 0 , 1)) = 0
		_FresnelPower("Fresnel Power", Range( -0.5 , 0.5)) = -0.9841692
		_fresnelscale("fresnel scale", Range( 1 , 2)) = -0.9841692
		_AlpphaMask("AlpphaMask", 2D) = "white" {}
		_WorldChange("WorldChange", Range( 0 , 1)) = 0
		_TotalOpacity("TotalOpacity", Range( 0 , 1)) = 0
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
			float2 uv_texcoord;
			float3 worldNormal;
			float3 worldPos;
			INTERNAL_DATA
		};

		uniform sampler2D _Alvedo1;
		uniform float4 _Alvedo1_ST;
		uniform sampler2D _Lut;
		uniform float _ToonShader;
		uniform float _WorldChange;
		uniform float4 _Color0;
		uniform float _fresnelbiasss;
		uniform float _fresnelscale;
		uniform float _FresnelPower;
		uniform sampler2D _AlpphaMask;
		uniform float4 _AlpphaMask_ST;
		uniform float _TotalOpacity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Alvedo1 = i.uv_texcoord * _Alvedo1_ST.xy + _Alvedo1_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult4 = dot( i.worldNormal , ase_worldlightDir );
			float temp_output_6_0 = (0.0 + (dotResult4 - 0.0) * (1.0 - 0.0) / (1.0 - 0.0));
			float clampResult12 = clamp( ( temp_output_6_0 * ( temp_output_6_0 + ( 1.0 - _ToonShader ) ) ) , 0.1 , 0.9 );
			float2 appendResult13 = (float2(clampResult12 , 0.0));
			float temp_output_59_0 = ( 1.0 - _WorldChange );
			o.Albedo = ( ( tex2D( _Alvedo1, uv_Alvedo1 ) * tex2D( _Lut, appendResult13 ) ) * temp_output_59_0 ).rgb;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelFinalVal40 = (_fresnelbiasss + _fresnelscale*pow( 1.0 - dot( ase_worldNormal, worldViewDir ) , _FresnelPower));
			o.Emission = ( _WorldChange * ( _Color0 * fresnelFinalVal40 ) ).rgb;
			float2 uv_AlpphaMask = i.uv_texcoord * _AlpphaMask_ST.xy + _AlpphaMask_ST.zw;
			float4 temp_cast_3 = (1.0).xxxx;
			float4 lerpResult83 = lerp( tex2D( _AlpphaMask, uv_AlpphaMask ) , temp_cast_3 , temp_output_59_0);
			o.Alpha = ( lerpResult83 * _TotalOpacity ).x;
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
0;92;1181;439;1257.478;309.9175;3.41614;True;True
Node;AmplifyShaderEditor.CommentaryNode;88;-2360.618,-153.876;Float;False;2160.646;928.2598;Toon Shader;13;17;15;14;13;12;11;9;10;8;7;6;5;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1;-2332.043,146.1875;Float;False;465;303;Comment;3;3;4;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;3;-2293.043,356.1876;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.WorldNormalVector;2;-2277.993,199.1371;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;4;-1989.043,230.1875;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-2113.046,502.0159;Float;False;Property;_ToonShader;Toon Shader;2;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;7;-1785.638,478.5976;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;6;-1789.638,240.5974;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-1616.638,387.5975;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-1411.889,552.1505;Float;False;Constant;_Float5;Float 5;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1474.639,283.5974;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-1407.889,430.1503;Float;False;Constant;_Float3;Float 3;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;44;103.9334,987.6331;Float;False;Property;_fresnelscale;fresnel scale;7;0;-0.9841692;1;2;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;38;95.02299,1073.161;Float;False;Property;_FresnelPower;Fresnel Power;6;0;-0.9841692;-0.5;0.5;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;12;-1166.081,348.3386;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;45;113.9335,901.6331;Float;False;Property;_fresnelbiasss;fresnel biasss;5;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;52;-162.1072,489.5186;Float;False;Property;_WorldChange;WorldChange;9;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;13;-947.889,417.1503;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.FresnelNode;40;565.6395,814.0149;Float;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;25;529.9771,161.2841;Float;False;Property;_Color0;Color 0;4;0;0,0.7941176,0.7283973,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;59;168.6197,381.0222;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;14;-896.2997,93.31252;Float;True;Property;_Alvedo1;Alvedo 1;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;94;882.5289,589.4234;Float;False;Constant;_Float0;Float 0;10;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;34;387.6382,490.4544;Float;True;Property;_AlpphaMask;AlpphaMask;8;0;Assets/Textures/circle2.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-786.5304,336.5661;Float;True;Property;_Lut;Lut;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WireNode;74;693.4188,567.9786;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;838.463,321.6461;Float;False;2;2;0;COLOR;0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-400.9458,166.8992;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;83;1115.421,396.1962;Float;False;3;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;108;1093.682,557.9066;Float;False;Property;_TotalOpacity;TotalOpacity;10;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;482.5042,-42.49907;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;1007.351,232.5497;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;1373.121,410.6396;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1522.944,157.8482;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;AmbientShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;1;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
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
WireConnection;13;0;12;0
WireConnection;40;1;45;0
WireConnection;40;2;44;0
WireConnection;40;3;38;0
WireConnection;59;0;52;0
WireConnection;15;1;13;0
WireConnection;74;0;40;0
WireConnection;21;0;25;0
WireConnection;21;1;74;0
WireConnection;17;0;14;0
WireConnection;17;1;15;0
WireConnection;83;0;34;0
WireConnection;83;1;94;0
WireConnection;83;2;59;0
WireConnection;60;0;17;0
WireConnection;60;1;59;0
WireConnection;87;0;52;0
WireConnection;87;1;21;0
WireConnection;107;0;83;0
WireConnection;107;1;108;0
WireConnection;0;0;60;0
WireConnection;0;2;87;0
WireConnection;0;9;107;0
ASEEND*/
//CHKSM=185040C8373ECDE4BDD7C3C98AF2C6E0CAACF0D5