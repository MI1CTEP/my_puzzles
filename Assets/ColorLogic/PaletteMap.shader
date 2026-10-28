Shader "Custom/PaletteMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PaletteSize ("Palette Size", Int) = 10
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            float4 _Palette[10]; // максимум 10 цветов

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 col = tex2D(_MainTex, i.uv).rgb;

                float3 best = _Palette[0].rgb;
                float bestDist = 1000;

                for (int j = 0; j < 10; j++)
                {
                    float3 p = _Palette[j].rgb;

                    float3 diff = col - p;
                    float d = dot(diff, diff); // только RGB расстояние

                    if (d < bestDist)
                    {
                        bestDist = d;
                        best = p;
                    }
                }

                return float4(best, 1.0);
            }
            ENDCG
        }
    }
}