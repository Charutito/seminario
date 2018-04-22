// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "EnemyShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_Albedo("Albedo", 2D) = "white" {}
		_ToonStrenght("Toon Strenght", Range( -10 , 10)) = 10
		_Lut("Lut", 2D) = "white" {}
		_HeightMap("HeightMap", 2D) = "white" {}
		_Normal("Normal", 2D) = "white" {}
		_Float7("Float 7", Range( 0 , 1)) = 0.8296579
		_Disolve("Disolve", Range( 0 , 1)) = 0
		_Tint("Tint", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "TreeTransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ "_GrabScreen2" }
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
			float4 screenPos;
		};

		uniform half4 _Tint;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Lut;
		uniform half _ToonStrenght;
		uniform half _Float7;
		uniform fixed _Disolve;
		uniform sampler2D _GrabScreen2;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _HeightMap;
		uniform float4 _HeightMap_ST;
		uniform float _MaskClipValue = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult5 = dot( i.worldNormal , ase_worldlightDir );
			float clampResult9 = clamp( ( dotResult5 * ( 1.0 - _ToonStrenght ) ) , 0.1 , 0.9 );
			half2 temp_cast_0 = (clampResult9).xx;
			o.Albedo = ( _Tint * ( ( ( tex2D( _Albedo, uv_Albedo ) * tex2D( _Lut, temp_cast_0 ) ) * _Float7 ) * ( 1.0 - _Disolve ) ) ).rgb;
			half4 temp_cast_3 = 0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos19 = ase_screenPos;
			float4 componentMask20 = ase_screenPos19.xyzw;
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float4 componentMask28 = ( (0.0 + (_Disolve - 0.0) * (0.5 - 0.0) / (1.0 - 0.0)) * tex2D( _Normal, uv_Normal ) ).xyzw;
			fixed4 screenColor29 = tex2Dproj( _GrabScreen2, UNITY_PROJ_COORD( ( componentMask20 + componentMask28 ) ) );
			float4 lerpResult31 = lerp( temp_cast_3 , screenColor29 , _Disolve);
			float2 uv_HeightMap = i.uv_texcoord * _HeightMap_ST.xy + _HeightMap_ST.zw;
			float clampResult49 = clamp( (-4.0 + (( (-1.0 + (( 1.0 - _Disolve ) - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) + tex2D( _HeightMap, uv_HeightMap ).r ) - 0.0) * (4.0 - -4.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
			o.Emission = ( _Disolve * ( lerpResult31 + ( 1.0 - clampResult49 ) ) ).rgb;
			o.Alpha = 1;
			clip( clampResult49 - _MaskClipValue );
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
0;92;905;295;2953.179;-1951.078;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;52;-3059.018,17.78394;Float;False;2567.195;720.6106;Toon Shader;12;13;14;12;10;11;9;6;8;7;4;3;15;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;15;-2901.058,220.433;Float;False;465;303;Comment;3;1;5;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-2566.551,2244.441;Float;False;1731.08;835.6802;Disolve;7;40;41;49;48;47;46;45;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-2862.058,430.433;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.CommentaryNode;16;-2132.099,1047.127;Float;False;2015.758;1031.04;Cloacking;14;30;29;24;28;19;31;27;26;25;23;22;20;18;32;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;1;-2847.008,273.3826;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;3;-2260.094,591.4788;Float;False;Property;_ToonStrenght;Toon Strenght;2;0;10;-10;10;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;42;-2706.361,2040.674;Fixed;False;Property;_Disolve;Disolve;7;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;25;-2043.459,1882.475;Float;True;Property;_Normal;Normal;5;0;Assets/Textures/11027-normal.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;5;-2624.36,277.133;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;41;-2538.164,2384.98;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;26;-1908.519,1677.401;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;4;-2380.252,515.5881;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;47;-2302.682,2428.273;Float;True;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-1.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;19;-1513.76,1423.174;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1662.461,1704.975;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT4;0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;40;-2531.962,2731.483;Float;True;Property;_HeightMap;HeightMap;4;0;Assets/Textures/220px-Grid_illusion.svg.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-2156.271,286.6482;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;6;-1980.904,626.3961;Float;False;Constant;_Float3;Float 3;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;8;-2021.986,513.2979;Float;False;Constant;_Float5;Float 5;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ComponentMaskNode;20;-1288.962,1394.775;Float;True;True;True;True;True;1;0;FLOAT4;0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ClampOpNode;9;-1785.795,416.084;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-2092.016,2706.587;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.ComponentMaskNode;28;-1373.963,1777.975;Float;True;True;True;True;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;10;-1408.123,407.4329;Float;True;Property;_Lut;Lut;3;0;Assets/Textures/Luts/lu7.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;48;-1915.95,2380.331;Float;True;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-4.0;False;4;FLOAT;4.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-1074.062,1657.475;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.SamplerNode;11;-1523.327,146.558;Float;True;Property;_Albedo;Albedo;1;0;Assets/Models/basicEnemy.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;13;-1069.688,524.3499;Float;False;Property;_Float7;Float 7;6;0;0.8296579;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;49;-1615.547,2329.132;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.WireNode;23;-1264.442,1324.426;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.IntNode;32;-846.0941,1478.338;Float;False;Constant;_Int5;Int 5;3;0;0;0;1;INT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1041.886,288.5734;Float;True;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ScreenColorNode;29;-926.0668,1831.676;Fixed;False;Global;_GrabScreen2;Grab Screen 2;3;0;Object;-1;True;1;0;FLOAT4;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;46;-1360.609,2392.431;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-677.2451,251.4124;Float;True;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.OneMinusNode;18;-1955.643,1147.727;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;31;-667.6528,1621.272;Float;True;3;0;INT;0,0,0,0;False;1;COLOR;0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-296.7685,1716.978;Float;True;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-272.3876,1193.04;Float;False;2;2;0;FLOAT4;0.0;False;1;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.ColorNode;55;1350.276,1259.312;Float;False;Property;_Tint;Tint;8;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;1176.27,1693.166;Float;False;2;2;0;FLOAT;0.0,0,0,0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1705.422,1594.687;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0.0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1879.888,1703.54;Half;False;True;2;Half;ASEMaterialInspector;0;0;Standard;EnemyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TreeTransparentCutout;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;0;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;5;1;2;0
WireConnection;41;0;42;0
WireConnection;26;0;42;0
WireConnection;4;0;3;0
WireConnection;47;0;41;0
WireConnection;27;0;26;0
WireConnection;27;1;25;0
WireConnection;7;0;5;0
WireConnection;7;1;4;0
WireConnection;20;0;19;0
WireConnection;9;0;7;0
WireConnection;9;1;8;0
WireConnection;9;2;6;0
WireConnection;45;0;47;0
WireConnection;45;1;40;1
WireConnection;28;0;27;0
WireConnection;10;1;9;0
WireConnection;48;0;45;0
WireConnection;30;0;20;0
WireConnection;30;1;28;0
WireConnection;49;0;48;0
WireConnection;23;0;42;0
WireConnection;12;0;11;0
WireConnection;12;1;10;0
WireConnection;29;0;30;0
WireConnection;46;0;49;0
WireConnection;14;0;12;0
WireConnection;14;1;13;0
WireConnection;18;0;42;0
WireConnection;31;0;32;0
WireConnection;31;1;29;0
WireConnection;31;2;23;0
WireConnection;22;0;31;0
WireConnection;22;1;46;0
WireConnection;24;0;14;0
WireConnection;24;1;18;0
WireConnection;56;0;42;0
WireConnection;56;1;22;0
WireConnection;54;0;55;0
WireConnection;54;1;24;0
WireConnection;0;0;54;0
WireConnection;0;2;56;0
WireConnection;0;10;49;0
ASEEND*/
//CHKSM=B0C0052E4780F25D8EE92969A128D2D90D648891