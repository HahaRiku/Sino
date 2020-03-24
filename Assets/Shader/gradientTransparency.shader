// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/gradientTransparency" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue"="Transparent"  "IgnoreProjector"="True"}
		LOD 100
		ZWrite Off
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float bias;
			float _GradientCount = 3;
			fixed4 _GradientColors[4];
			float _GradientPositions[4];

			struct appdata_f
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata_f v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = v.uv;
				return o;
			}
   
			float4 frag (v2f i) : SV_Target {
				fixed4 c;
				if (_GradientCount == 0) {
					c = _GradientColors[0];
				}
				else {
					for (int j = 0; j < _GradientCount; j++) {
						if (i.uv.x <= _GradientPositions[j] - bias) {
							c = _GradientColors[j];
							break;
						}
						else if (i.uv.x <= _GradientPositions[j] + bias) {
							c = lerp(_GradientColors[j + 1], _GradientColors[j], (i.uv.x - _GradientPositions[j] - bias) / (_GradientPositions[j] - bias - _GradientPositions[j] - bias));
							break;
						}
						else if (j == _GradientCount - 1) {
							c = _GradientColors[j + 1];
							break;
						}
					}
				}

				return tex2D(_MainTex, i.uv) * c;
			}

			ENDCG
		}
	}
}