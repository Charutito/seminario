// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FloorShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Heightmap("Heightmap", 2D) = "white" {}
		_GrassAmount("GrassAmount", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _GrassAmount;
		uniform sampler2D _Heightmap;
		uniform float4 _Heightmap_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 tex2DNode7 = tex2D( _TextureSample1,uv_TextureSample1);
			float temp_output_34_0 = ( 1.0 - _GrassAmount );
			float4 temp_cast_0 = clamp( ( tex2DNode7.a + temp_output_34_0 ) , 0.0 , 1.0 );
			float2 uv_Heightmap = i.uv_texcoord * _Heightmap_ST.xy + _Heightmap_ST.zw;
			float4 temp_cast_1 = clamp( ( temp_output_34_0 + tex2D( _Heightmap,uv_Heightmap).g ) , 0.0 , 1.0 );
			o.Emission = lerp( tex2D( _TextureSample0,uv_TextureSample0) , tex2DNode7 , ( saturate( ( 1.0 - ( ( 1.0 - temp_cast_1) / temp_cast_0) ) )).r ).xyz;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=5105
0;92;988;651;1432.281;724.2615;1;False;True
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;171.2,-152;Float;False;True;2;Float;ASEMaterialInspector;Standard;FloorShader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;OBJECT;0.0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False
Node;AmplifyShaderEditor.LerpOp;27;1.149117,-472.3942;Float;False;0;FLOAT4;0.0;False;1;FLOAT4;0.0,0,0,0;False;2;COLOR;0.0;False
Node;AmplifyShaderEditor.SamplerNode;6;-884.3011,-789.7011;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;7;-1024.499,-558.801;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-636.4503,-404.7945;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-629.1506,-273.894;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False
Node;AmplifyShaderEditor.ClampOpNode;31;-404.9513,-416.2944;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.ClampOpNode;32;-435.5514,-274.9937;Float;False;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False
Node;AmplifyShaderEditor.SamplerNode;28;-1040.447,-224.9937;Float;True;Property;_Heightmap;Heightmap;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False
Node;AmplifyShaderEditor.BlendOpsNode;33;-231.8502,-353.3938;Float;False;ColorBurn;True;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False
Node;AmplifyShaderEditor.RangedFloatNode;21;-1317.329,-349.7942;Float;False;Property;_GrassAmount;GrassAmount;4;0;1;0;1
Node;AmplifyShaderEditor.OneMinusNode;34;-961.999,-347.2708;Float;False;0;FLOAT;0.0;False
WireConnection;0;2;27;0
WireConnection;27;0;6;0
WireConnection;27;1;7;0
WireConnection;27;2;33;0
WireConnection;30;0;7;4
WireConnection;30;1;34;0
WireConnection;29;0;34;0
WireConnection;29;1;28;2
WireConnection;31;0;30;0
WireConnection;32;0;29;0
WireConnection;33;0;31;0
WireConnection;33;1;32;0
WireConnection;34;0;21;0
ASEEND*/
//CHKSM=E3EA7DD805ED102EA5F1E48164C2D2F48D267816