// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/gradientTransparency" {
	Properties {
		 [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		 _Color ("Left Color", Color) = (1,1,1,1)
		 _Color2 ("Right Color", Color) = (1,1,1,1)
		 _Size ("Size", Range(1, 10)) = 1
		 _Rotation("Rotation", Range(0, 359)) = 0	//0:right
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
			fixed4 _Color;
			fixed4 _Color2;
			sampler2D _MainTex;
			float _Size;
			float _Rotation;

			struct appdata_f
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 col : COLOR;
				float2 uv : TEXCOORD0;
			};

			v2f vert (appdata_f v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				float sinX = sin (radians(_Rotation));
				float cosX = cos (radians(_Rotation));
				float2x2 rotationMatrix = float2x2( cosX, -sinX, sinX, cosX);
				//o.col = lerp(_Color, _Color2, (mul (v.uv - fixed2 (0.5, 0.5), rotationMatrix) + fixed2 (0.5, 0.5)).x * _Size) ;
				o.col = lerp(_Color, _Color2, (mul (v.uv, rotationMatrix)).x * _Size) ;
				//o.col = lerp(_Color, _Color2, v.uv.x * _Size) ;

				o.uv = v.uv;
				return o;
			}
   
			float4 frag (v2f i) : SV_Target {
				float4 c = tex2D(_MainTex, i.uv) * i.col;
				return c;
			}
			ENDCG
		}
	}
}