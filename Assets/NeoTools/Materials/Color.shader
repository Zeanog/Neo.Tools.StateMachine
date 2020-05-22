// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent/Color"{
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
	}

	CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _MainTex_ST;
		half4 _Color;

		struct FragmentInput {
			float4	mvp_pos : SV_POSITION;
			float2  uv : TEXCOORD0;
		};
	ENDCG
 
	SubShader {
		Tags{ "Queue" = "Transparent" }

 		Pass {

		CGPROGRAM
			//#pragma surface surf BlinnPhong addshadow vertex:vert
			#pragma vertex vert
			#pragma fragment frag
		
			FragmentInput vert( appdata_base v ) {
				FragmentInput o;
		
				o.mvp_pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;//TRANSFORM_TEX(_MainTex, v.texcoord);
		
				return o;
			}
		
			half4 frag( FragmentInput i ) : COLOR {
				half4 	texColor = tex2D(_MainTex, i.uv);
				return half4(texColor.rgb * (1.0 -_Color.a) + _Color.rgb * _Color.a, 1.0);
			}
		
		ENDCG
		}
	}
}