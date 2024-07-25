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

                // �⺻ �ؽ��� �Ǵ� ����
                fixed4 texColor = tex2D(_MainTex, uv);
                fixed4 baseColor = _BaseColor;
                fixed4 col = lerp(baseColor, texColor, _UseTexture);

                // ���� �ؽ��ĸ� ����Ͽ� ����ũ�� ���
                float alphaTexCoordX = _BurnAmount; // x��ǥ�� �ش��ϴ� �ؽ�ó ��ǥ
                fixed4 alphaTex = tex2D(_AlphaTex, float2(alphaTexCoordX, 0.1)); // y��ǥ�� �߾����� ����
                float mask = alphaTex.a; // ���� �ؽ�ó�� a ä���� ���

                // Ÿ�� ȿ���� ���� �⺻ ���� ����ũ���� ����
                col.rgb *= mask; // ����ũ���� ���� �˰� ���ϴ� ȿ�� ����

                return col;
            }
            ENDCG
        }
    }
}
