// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TextureFlipX" {
	Properties{
		_MainTex("Source Texture", 2D) = "white" { }
	}
	
	SubShader {
		Tags{ "RenderType" = "Opaque" }

		Pass {
			CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

			sampler2D		_MainTex;
			uniform float4	_MainTex_ST;
	
			struct FragInput {
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;
			};
		
			FragInput	vert(appdata_base v) {
				FragInput o;
		
				o.pos = UnityObjectToClipPos(v.vertex);
				//o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv_MainTex = (v.texcoord.xy * float2(-1.0, 1.0) + float2(1.0, 0.0));
		
				return o;
			}
		
			half4 frag(FragInput i) : COLOR {
				half4 tex = tex2D(_MainTex, i.uv_MainTex);
				return half4( tex.rgb, 1.0 );
			}
			ENDCG
		}
	}
}