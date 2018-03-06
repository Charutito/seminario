// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New AmplifyShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Float1("Float 1", Range( 0 , 1)) = 0
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_Float7("Float 7", Range( 0 , 1)) = 0
		_Color0("Color 0", Color) = (0,0.7941176,0.7283973,0)
		_FresnelPower("Fresnel Power", Range( -2 , 1)) = -2
		_alphatexture("alpha texture", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
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
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform sampler2D _alphatexture;
		uniform float4 _alphatexture_ST;
		uniform sampler2D _GrabTexture;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _TextureSample3;
		uniform float _Float1;
		uniform float _Float7;
		uniform float4 _Color0;
		uniform float _FresnelPower;
		uniform float _MaskClipValue = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_alphatexture = i.uv_texcoord * _alphatexture_ST.xy + _alphatexture_ST.zw;
			float4 temp_output_26_0 = ( tex2D( _alphatexture, uv_alphatexture ) * float4( float3(1,1,1) , 0.0 ) );
			o.Normal = temp_output_26_0.xyz;
			float4 screenPos18 = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
			float scale18 = -1.0;
			#else
			float scale18 = 1.0;
			#endif
			float halfPosW18 = screenPos18.w * 0.5;
			screenPos18.y = ( screenPos18.y - halfPosW18 ) * _ProjectionParams.x* scale18 + halfPosW18;
			screenPos18.w += 0.00000000001;
			screenPos18.xyzw /= screenPos18.w;
			float4 screenColor18 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( screenPos18 ) );
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float dotResult4 = dot( ase_worldNormal , ase_worldlightDir );
			float temp_output_6_0 = (0.0 + (dotResult4 - 0.0) * (1.0 - 0.0) / (1.0 - 0.0));
			float clampResult12 = clamp( ( temp_output_6_0 * ( temp_output_6_0 + ( 1.0 - _Float1 ) ) ) , 0.1 , 0.9 );
			float2 appendResult13 = (float2(clampResult12 , 0.0));
			float4 lerpResult19 = lerp( screenColor18 , ( tex2D( _TextureSample1, uv_TextureSample1 ) * tex2D( _TextureSample3, appendResult13 ) ) , _Float7);
			float4 lerpResult41 = lerp( lerpResult19 , float4( 0,0,0,0 ) , 0.0);
			o.Albedo = lerpResult41.xyz;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float fresnelFinalVal40 = (0.0 + 1.0*pow( 1.0 - dot( temp_output_26_0.xyz, worldViewDir ) , _FresnelPower));
			o.Emission = ( _Color0 * fresnelFinalVal40 ).rgb;
			o.Alpha = fresnelFinalVal40;
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
1927;29;1586;824;859.0035;-185.5912;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;1;-2332.043,146.1875;Float;False;465;303;Comment;3;3;4;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;2;-2277.993,199.1371;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;3;-2293.043,356.1876;Float;False;1;0;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.DotProductOpNode;4;-1989.043,230.1875;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;5;-2079.976,507.3073;Float;False;Property;_Float1;Float 1;1;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemap;6;-1789.638,240.5974;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;7;-1785.638,478.5976;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-1616.638,387.5975;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;11;-1411.889,552.1505;Float;False;Constant;_Float5;Float 5;3;0;0.9;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-1407.889,430.1503;Float;False;Constant;_Float3;Float 3;3;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1474.639,283.5974;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;12;-1166.081,348.3386;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;13;-947.889,417.1503;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;34;-490.1911,779.7894;Float;True;Property;_alphatexture;alpha texture;8;0;Assets/Textures/alpha texture.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;28;-391.5389,610.4174;Float;False;Constant;_Vector0;Vector 0;5;0;1,1,1;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;14;-954.3123,72.31252;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-788.8304,382.5661;Float;True;Property;_TextureSample3;Texture Sample 3;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenColorNode;18;-488.1155,-3.733704;Float;False;Global;_GrabScreen1;Grab Screen 1;4;0;Object;-1;False;1;0;FLOAT2;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-106.918,489.147;Float;True;2;2;0;FLOAT4;0,0,0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;16;-479.3765,468.3584;Float;False;Property;_Float7;Float 7;3;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-472.8707,214.3279;Float;True;2;2;0;FLOAT4;0.0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;38;-88.93176,745.1221;Float;False;Property;_FresnelPower;Fresnel Power;7;0;-2;-2;1;0;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;19;-188.1153,89.2663;Float;False;3;0;COLOR;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.ColorNode;25;-158.2219,258.556;Float;False;Property;_Color0;Color 0;5;0;0,0.7941176,0.7283973,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;40;172.0682,486.1221;Float;False;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;41;231.6391,-79.36415;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;227.4103,236.4014;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;523.9996,160.8;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;New AmplifyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;Transparent;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;2;0
WireConnection;4;1;3;0
WireConnection;6;0;4;0
WireConnection;7;0;5;0
WireConnection;8;0;6;0
WireConnection;8;1;7;0
WireConnection;10;0;6;0
WireConnection;10;1;8;0
WireConnection;12;0;10;0
WireConnection;12;1;9;0
WireConnection;12;2;11;0
WireConnection;13;0;12;0
WireConnection;15;1;13;0
WireConnection;26;0;34;0
WireConnection;26;1;28;0
WireConnection;17;0;14;0
WireConnection;17;1;15;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;19;2;16;0
WireConnection;40;0;26;0
WireConnection;40;3;38;0
WireConnection;41;0;19;0
WireConnection;21;0;25;0
WireConnection;21;1;40;0
WireConnection;0;0;41;0
WireConnection;0;1;26;0
WireConnection;0;2;21;0
WireConnection;0;9;40;0
ASEEND*/
//CHKSM=3DA9BE5C36061F8E0232DBA15BB6FA867320AA31