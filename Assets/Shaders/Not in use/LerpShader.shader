// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/LerpShader"
{
	Properties
	{
		//DISPLACEMENT
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
	    _Magnitude("Magnitude", Range(0,0.1)) = 1

		_MainTex ("Texture", 2D) = "white" {}
	    _altTExt("Texture", 2D) = "black" {}
		_cameraSelect("Camera Select", Range(0, 1)) = 0
		_MaskTex("Mask Texture", 2D) = "white" {}
		_MaskValue("Mask Value", Range(0,1)) = 0.5
		_MaskColor("Mask Color", Color) = (0,0,0,1)
	    _GWColor("Tint Color", Color) = (0,0,0,1)

	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			//DISPLACEMENTSHADER
			sampler2D _DisplaceTex;
			float _Magnitude;
			//lerp
			sampler2D _MainTex;
			sampler2D _altTExt;
		    sampler2D _MaskTex;
			float _MaskValue;
			float4 _MaskColor;
			float4 _GWColor;

			fixed _cameraSelect;

			float4  frag (v2f i) : SV_Target
			{
				float2 disp = tex2D(_DisplaceTex, i.uv).xy;
				disp = ((disp * 2) - 1) * _Magnitude;


				float4 col = tex2D(_MainTex, i.uv);
				float4 MaskTex = tex2D(_MaskTex, i.uv);
				float weight = step(_cameraSelect, MaskTex.a);
			    float4 alt = tex2D(_altTExt, fixed2(i.uv.x, (i.uv.y))+ disp);
				fixed4 altMask = fixed4(col.r,col.g,col.b, MaskTex.a);
				col.rgb = lerp(col.rgb,lerp(_MaskColor.rgb, alt.rgb, weight), _cameraSelect);
				return  lerp(col,alt*_GWColor, _cameraSelect);
			} 
			ENDCG
		}
	}
}
