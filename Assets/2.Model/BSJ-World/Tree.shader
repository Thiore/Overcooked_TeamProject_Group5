Shader "Custom/WindAffectedTree"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _BendAmount("Bend Amount", Float) = 1.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows

            sampler2D _MainTex;
            float _BendAmount;

            struct Input
            {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                half4 c = tex2D(_MainTex, IN.uv_MainTex) * _BendAmount;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}