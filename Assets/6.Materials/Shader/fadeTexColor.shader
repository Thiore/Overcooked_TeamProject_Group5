Shader "Unlit/fadeTexColor"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _UseTexture("Use Texture(0 or 1)", Float) = 1.0
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _AlphaTex("Alpha Texture", 2D) = "white" {}
        _BurnAmount("Burn Amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UseTexture;
            float4 _BaseColor;
            sampler2D _AlphaTex;
            float _BurnAmount;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 기본 텍스쳐 또는 색상
                fixed4 texColor = tex2D(_MainTex, uv);
                fixed4 baseColor = _BaseColor;
                fixed4 col = lerp(baseColor, texColor, _UseTexture);

                // 알파 텍스쳐를 사용하여 마스크값 계산
                float alphaTexCoordX = _BurnAmount; // x좌표에 해당하는 텍스처 좌표
                fixed4 alphaTex = tex2D(_AlphaTex, float2(alphaTexCoordX, 0.1)); // y좌표는 중앙으로 설정
                float mask = alphaTex.a; // 알파 텍스처의 a 채널을 사용

                // 타는 효과를 위해 기본 색상에 마스크값을 곱함
                col.rgb *= mask; // 마스크값을 곱해 검게 변하는 효과 적용

                return col;
            }
            ENDCG
        }
    }
}
