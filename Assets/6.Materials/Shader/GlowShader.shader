Shader "Unlit/GlowShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UseTexture ("Use Texture(0or1)", Float) = 1.0
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _GlowColor("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity("Glow Intensity", Range(0, 20)) = 1.0
        _AlphaClip ("Alpha Clip Threshold", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }
            ZWrite On
            ZTest LEqual
            ColorMask RGB

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UseTexture;
            float4 _BaseColor;
            float4 _GlowColor;
            float _GlowIntensity;
            float _AlphaClip;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
               half4 texColor = tex2D(_MainTex, i.uv);
               half4 baseColor = _BaseColor;

               clip(texColor.a - _AlphaClip);

               half4 color = lerp(baseColor, texColor, _UseTexture);
               half4 glow = _GlowColor * _GlowIntensity;
               return color + glow;
            }
            ENDCG
        }
    }
            FallBack "Diffuse"
}
