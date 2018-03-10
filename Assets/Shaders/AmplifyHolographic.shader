// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify/Holographic"
{
	Properties
	{
		_TextureMap("Texture Map", 2D) = "white" {}
		_Color("Color", Color) = (0.1058824,0.3921569,0.7764707,0)
		_EmissionStrength("Emission Strength", Float) = 1
		_LightColorMask("Light Color Mask", Color) = (1,1,1,0)
		_FresnelPower("Fresnel Power", Float) = 1
		_OpacityStrength("Opacity Strength", Range( 0 , 1)) = 0.8
		_Cutoff( "Mask Clip Value", Float ) = 0
		_AmbientOcclusion("Ambient Occlusion", 2D) = "white" {}
		_AmbientOcclusionStrength("Ambient Occlusion Strength", Range( 0 , 1)) = 0
		_ScanlineScale("Scanline Scale", Float) = 0
		_AnimationStrength("Animation Strength", Float) = 0
		_ScanlineStrength("Scanline Strength", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend One One
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPosition;
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float _OpacityStrength;
		uniform float _FresnelPower;
		uniform sampler2D _AmbientOcclusion;
		uniform float4 _AmbientOcclusion_ST;
		uniform float _AmbientOcclusionStrength;
		uniform float _ScanlineScale;
		uniform float _AnimationStrength;
		uniform float _ScanlineStrength;
		uniform float4 _LightColorMask;
		uniform sampler2D _TextureMap;
		uniform float4 _TextureMap_ST;
		uniform float4 _Color;
		uniform float _EmissionStrength;
		uniform float _Cutoff = 0;


		inline float Dither4x4Bayer( int x, int y )
		{
			const float dither[ 16 ] = {
				 1,  9,  3, 11,
				13,  5, 15,  7,
				 4, 12,  2, 10,
				16,  8, 14,  6 };
			int r = y * 4 + x;
			return dither[r] / 16;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float4 ase_screenPos = ComputeScreenPos( UnityObjectToClipPos( v.vertex ) );
			o.screenPosition = ase_screenPos;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#if DIRECTIONAL
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			float4 ase_screenPos = i.screenPosition;
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float2 clipScreen21 = ase_screenPosNorm.xy * _ScreenParams.xy;
			float dither21 = Dither4x4Bayer( fmod(clipScreen21.x, 4), fmod(clipScreen21.y, 4) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float clampResult19 = clamp( _FresnelPower , 0.0 , 4.0 );
			float fresnelNDotV16 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode16 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNDotV16, clampResult19 ) );
			float2 uv_AmbientOcclusion = i.uv_texcoord * _AmbientOcclusion_ST.xy + _AmbientOcclusion_ST.zw;
			float clampResult52 = clamp( ( tex2D( _AmbientOcclusion, uv_AmbientOcclusion ).r + ( 1.0 - _AmbientOcclusionStrength ) ) , 0.0 , 1.0 );
			float ambientOcclusion53 = clampResult52;
			float lerpResult68 = lerp( 1.0 , sin( ( _ScanlineScale * ( ase_worldPos.y + ( _Time.x * _AnimationStrength ) ) ) ) , _ScanlineStrength);
			float scanLine71 = lerpResult68;
			dither21 = step( dither21, ( _OpacityStrength * ( 1.0 - fresnelNode16 ) * ambientOcclusion53 * scanLine71 ) );
			float3 appendResult29 = (float3(_LightColorMask.r , _LightColorMask.g , _LightColorMask.b));
			float3 weightedBlendVar30 = appendResult29;
			float weightedBlend30 = ( weightedBlendVar30.x*(_LightColor0.rgb).x + weightedBlendVar30.y*(_LightColor0.rgb).y + weightedBlendVar30.z*(_LightColor0.rgb).z );
			float clampResult37 = clamp( weightedBlend30 , 0.0 , 1.0 );
			float lerpResult23 = lerp( 0.0 , dither21 , clampResult37);
			float opacityClip1 = lerpResult23;
			float4 temp_cast_0 = (0.0).xxxx;
			float2 uv_TextureMap = i.uv_texcoord * _TextureMap_ST.xy + _TextureMap_ST.zw;
			float4 temp_output_14_0 = ( tex2D( _TextureMap, uv_TextureMap ) * _Color * ambientOcclusion53 );
			float4 lerpResult43 = lerp( temp_cast_0 , ( temp_output_14_0 + abs( ( temp_output_14_0 * _EmissionStrength ) ) ) , ase_lightAtten);
			float4 textureMap6 = lerpResult43;
			c.rgb = textureMap6.rgb;
			c.a = 1;
			clip( opacityClip1 - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14301
1973;43;1797;980;3047.734;-73.40015;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;72;-2562.692,-169.5804;Float;False;1421.901;327.5129;Comment;12;64;62;63;58;71;70;68;69;67;66;65;59;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;54;-2161.06,-1084.566;Float;False;1016.156;337.3123;Ambient Occlusion;6;53;52;46;49;48;45;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-2537.692,38.41959;Float;False;Property;_AnimationStrength;Animation Strength;10;0;Create;True;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;62;-2530.692,-105.5804;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-2139.059,-841.5676;Float;False;Property;_AmbientOcclusionStrength;Ambient Occlusion Strength;8;0;Create;True;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;45;-2143.06,-1044.566;Float;True;Property;_AmbientOcclusion;Ambient Occlusion;7;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;49;-1850.061,-835.5676;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;58;-2289.692,-128.5804;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-2246.692,17.41953;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;4;-2762.311,196.4591;Float;False;1656.516;705.6158;Opacity;20;37;30;34;29;33;35;24;25;1;23;21;22;15;20;56;3;16;19;17;73;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1680.061,-1031.567;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-2046.693,1.41952;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2083.692,-80.58036;Float;False;Property;_ScanlineScale;Scanline Scale;9;0;Create;True;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;7;-2542.288,-721.8578;Float;False;1406.734;528.0924;Texture Map;12;55;6;43;42;44;41;40;39;38;14;5;12;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2733.77,388.1339;Float;False;Property;_FresnelPower;Fresnel Power;4;0;Create;True;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1869.694,-42.58042;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;52;-1547.061,-1029.567;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;-1401.061,-1027.567;Float;False;ambientOcclusion;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-1870.694,56.41953;Float;False;Property;_ScanlineStrength;Scanline Strength;11;0;Create;True;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;67;-1733.694,-44.58042;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1756.694,-119.5804;Float;False;Constant;_constant1;constant1;11;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-2492.288,-671.8579;Float;True;Property;_TextureMap;Texture Map;0;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;55;-2448.687,-298.7975;Float;False;53;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;19;-2554.764,390.1339;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;4.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-2449.288,-471.858;Float;False;Property;_Color;Color;1;0;Create;True;0.1058824,0.3921569,0.7764707,0;0.1058824,0.3921569,0.7764707,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;68;-1548.694,-61.58039;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-2169.288,-566.8578;Float;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;25;-2519.375,719.8221;Float;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.FresnelNode;16;-2414.763,319.1333;Float;True;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-2178.955,-427.8372;Float;False;Property;_EmissionStrength;Emission Strength;2;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;24;-2560.375,538.8229;Float;False;Property;_LightColorMask;Light Color Mask;3;0;Create;True;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1953.956,-475.8373;Float;False;2;2;0;COLOR;0.0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;20;-2159.762,318.1333;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;-2162.481,403.4226;Float;False;53;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2270.658,244.2058;Float;False;Property;_OpacityStrength;Opacity Strength;5;0;Create;True;0.8;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;73;-2114.734,477.4001;Float;False;71;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;71;-1370.694,-62.58039;Float;False;scanLine;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;34;-2303.374,747.8221;Float;False;False;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;35;-2305.374,821.822;Float;False;False;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;29;-2297.374,549.8229;Float;False;FLOAT3;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;33;-2304.374,669.822;Float;False;True;False;False;True;1;0;FLOAT3;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;40;-1824.956,-475.8373;Float;False;1;0;COLOR;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.SummedBlendNode;30;-1970.317,586.4554;Float;False;5;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1883.655,338.2061;Float;False;4;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;21;-1744.848,342.3867;Float;False;0;2;0;FLOAT;0.0;False;1;SAMPLER2D;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1720.13,266.5596;Float;False;Constant;_constant0_1;constant0_1;5;0;Create;True;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;37;-1784.583,583.9165;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-1682.956,-566.8375;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightAttenuation;44;-1795.958,-381.8104;Float;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1713.123,-643.7058;Float;False;Constant;_constant0_2;constant0_2;7;0;Create;True;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;43;-1517.958,-577.8106;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;23;-1509.13,370.5599;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2;-267,145;Float;False;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;8;-273,227;Float;False;6;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;6;-1354.588,-585.8578;Float;False;textureMap;-1;True;1;0;COLOR;0.0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1;-1332.745,365.4594;Float;False;opacityClip;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;Amplify/Holographic;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;False;0;Custom;0;True;False;0;False;Transparent;AlphaTest;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;4;One;One;0;One;One;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;6;-1;-1;-1;0;0;0;False;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0.0,0,0;False;4;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;49;0;48;0
WireConnection;63;0;62;1
WireConnection;63;1;64;0
WireConnection;46;0;45;1
WireConnection;46;1;49;0
WireConnection;59;0;58;2
WireConnection;59;1;63;0
WireConnection;66;0;65;0
WireConnection;66;1;59;0
WireConnection;52;0;46;0
WireConnection;53;0;52;0
WireConnection;67;0;66;0
WireConnection;19;0;17;0
WireConnection;68;0;69;0
WireConnection;68;1;67;0
WireConnection;68;2;70;0
WireConnection;14;0;5;0
WireConnection;14;1;12;0
WireConnection;14;2;55;0
WireConnection;16;3;19;0
WireConnection;39;0;14;0
WireConnection;39;1;38;0
WireConnection;20;0;16;0
WireConnection;71;0;68;0
WireConnection;34;0;25;1
WireConnection;35;0;25;1
WireConnection;29;0;24;1
WireConnection;29;1;24;2
WireConnection;29;2;24;3
WireConnection;33;0;25;1
WireConnection;40;0;39;0
WireConnection;30;0;29;0
WireConnection;30;1;33;0
WireConnection;30;2;34;0
WireConnection;30;3;35;0
WireConnection;15;0;3;0
WireConnection;15;1;20;0
WireConnection;15;2;56;0
WireConnection;15;3;73;0
WireConnection;21;0;15;0
WireConnection;37;0;30;0
WireConnection;41;0;14;0
WireConnection;41;1;40;0
WireConnection;43;0;42;0
WireConnection;43;1;41;0
WireConnection;43;2;44;0
WireConnection;23;0;22;0
WireConnection;23;1;21;0
WireConnection;23;2;37;0
WireConnection;6;0;43;0
WireConnection;1;0;23;0
WireConnection;0;10;2;0
WireConnection;0;13;8;0
ASEEND*/
//CHKSM=95C5B6C6EFD2A932793F8A6FB24DD141496C0DEB