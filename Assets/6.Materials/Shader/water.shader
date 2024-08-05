Shader "Unlit/water"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _WaveSpeed("Wave Speed", Range(0, 1)) = 0.1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
            LOD 200

            Pass
            {
                Name "ForwardLit"
                Tags { "LightMode" = "UniversalForward" }

                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                sampler2D _MainTex;
                float _WaveSpeed;

                struct Attributes
                {
                    float4 positionOS : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct Varyings
                {
                    float4 positionHCS : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                    OUT.uv = IN.uv;
                    return OUT;
                }

                half4 frag(Varyings IN) : SV_Target
                {
                    float2 uv = IN.uv;
                    uv.y += sin(_Time.y * _WaveSpeed) * 0.1;
                    half4 c = tex2D(_MainTex, uv);
                    return c;
                }
                ENDHLSL
            }
        }
            FallBack "Diffuse"
}
