Shader "Custom/WhiteSpriteWithOutline"
{
    Properties
    {
        _Color ("Glow Color", Color) = (1, 1, 1, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1
        _MainTex ("Main Texture", 2D) = "white" {}
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

            // User-defined properties
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
            float4 _Color;
            float _GlowIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Pobranie koloru tekstury
                fixed4 texColor = tex2D(_MainTex, i.uv);
                
                // Dodanie efektu œwiecenia jako emisji
                fixed4 glowColor = _Color * _GlowIntensity;

                // £¹czymy kolor tekstury z efektem œwiecenia
                fixed4 finalColor = texColor + glowColor;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
