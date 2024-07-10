Shader "Unlit/LimLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _UseTexture("Use Texture(0or1)", Float) = 1.0
        _Color ("Main Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(1.0, 10.0)) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _UseTexture;
            float4 _Color;
            float4 _RimColor;
            float _RimPower;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                half4 texColor = tex2D(_MainTex, i.uv) * _Color;
                half4 color = lerp(_Color, texColor, _UseTexture);
                // Calculate the rim effect
                float rim = 1.0 - saturate(dot(normalize(i.viewDir), normalize(i.normal)));
                rim = pow(rim, _RimPower);
                half4 rimColor = rim * _RimColor;

                // Combine texture color and rim color
                return color + rimColor;
            }
            ENDCG
        }
    }
            FallBack "Diffuse"
}
