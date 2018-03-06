// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SmokeShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_MaskClipValue( "Mask Clip Value", Float ) = 0.5
		_smoketexture("smoke texture", 2D) = "white" {}
		_Float0("Float 0", Range( 0 , 1)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 texcoord_0;
		};

		uniform sampler2D _smoketexture;
		uniform float _Float0;
		uniform float _MaskClipValue = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 tex2DNode10 = tex2D( _smoketexture,(abs( i.texcoord_0+_Time.y * float2(1,0 ))));
			o.Emission = ( tex2DNode10.a * tex2DNode10 ).xyz;
			o.Alpha = 1;
			clip( ( tex2DNode10.a * _Float0 ) - _MaskClipValue );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
0;92;1476;655;2007.59;252.0679;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;8;-570.7562,406.6095;Float;False;Property;_Float0;Float 0;2;0;1;0;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-219.1563,-118.0907;Float;True;0;FLOAT;0.0,0,0,0;False;1;FLOAT4;0.0;False
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;160,-1.6;Float;False;True;2;Float;ASEMaterialInspector;Standard;SmokeShader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.SamplerNode;10;-774.9776,-28.42629;Float;True;Property;_smoketexture;smoke texture;2;0;Assets/Materials/ghost line renderer.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-231.9553,268.3094;Float;True;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.RangedFloatNode;11;-1524.691,-43.06778;Float;False;Property;_Float1;Float 1;2;0;0;0;10
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1476.553,-215.0908;Float;False;0;-1;2;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False
Node;AmplifyShaderEditor.TimeNode;15;-1463.59,72.83209;Float;False
Node;AmplifyShaderEditor.PannerNode;3;-1122.355,-180.9908;Float;True;1;0;0;FLOAT2;0,0;False;1;FLOAT;0.0;False
WireConnection;7;0;10;4
WireConnection;7;1;10;0
WireConnection;0;2;7;0
WireConnection;0;10;9;0
WireConnection;10;1;3;0
WireConnection;9;0;10;4
WireConnection;9;1;8;0
WireConnection;3;0;1;0
WireConnection;3;1;15;2
ASEEND*/
//CHKSM=2A6E6D05DF7290972402980D94F07FB38EDF4316