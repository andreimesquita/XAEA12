Shader "Unlit/ui/scroll_texture_add" 
{
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_SpeedX ("Speed X", Float) = 1.0
	_SpeedY ("Speed Y", Float) = 1.0
	_Offset ("Offset", Range(0,2)) = 0
	_Alpha ("Alpha Multiplier", Float) = 1
    _Mask ("Screen Mask", 2D) = "white" {}
    [Space(30)][Toggle(TOGGLE_SCREENMASK)] Toggle_ScreenMask ("Toggle ScreenMask", Float) = 0 
    _TransitionColor ("Transition Color", Color) = (1.0, 1.0, 1.0, 1.0)
	
}

Category {
	Tags { "Queue"="Transparent1" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	Cull Off
	Lighting Off
	ZWrite Off
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
            #pragma shader_feature TOGGLE_SCREENMASK

			sampler2D _MainTex;
			fixed4 _TintColor;
			float _SpeedY;
			float _SpeedX;
			fixed _Alpha;
			half _Offset;
			fixed _Brightness;
			sampler2D _Mask;
            fixed4 _TransitionColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
                #if TOGGLE_SCREENMASK
                float4 scrPos : TEXCOORD1;
				#endif
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex.z -= _Offset / o.vertex.z;
				o.color = v.color * _TintColor * 2;
				float2 speed = frac(fixed2(_SpeedX, _SpeedY) *_Time.y);
				o.texcoord.xy = TRANSFORM_TEX(v.texcoord,_MainTex) +speed ;
                #if TOGGLE_SCREENMASK
                o.scrPos = ComputeScreenPos(o.vertex);
				#endif
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = i.color * tex2D(_MainTex, i.texcoord);
				col.a *= _Alpha;
                #if TOGGLE_SCREENMASK
				col.rgb = lerp(col, _TransitionColor, tex2D(_Mask, i.scrPos.xy / i.scrPos.w).a);
                #endif
				return col;
			}
			ENDCG 
		}
	}	
}
}
