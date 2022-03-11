Shader "Unlit/Wireframe_v1"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _lineColor ("Line Color", Color) = (1,1,1,1)
        _surfaceColor ("Surface Color", Color) = (1,1,1,1)
        _lineWidth ("Line Width", Float) = 1

    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Lighting Off
        //LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 Color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : POSITION;
                float4 Color : COLOR;

            };

            // sampler2D _MainTex;
            // float4 _MainTex_ST;
            fixed4 _lineColor;
            fixed4 _surfaceColor;
            fixed _lineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                //UNITY_TRANSFER_FOG(o,o.vertex);
                o.Color = v.Color;
                return o;
            }

            fixed4 frag (v2f i) : Color //SV_Target
            {
                // // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                // // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                // return col;

                float2 d = fwidth(i.uv);

                float lineY = smoothstep(float(0), d.y * _lineWidth, i.uv.y);
                float lineX = smoothstep(float(0), d.x * _lineWidth, 1 - i.uv.x);
                float diagonal = smoothstep(float(0), fwidth(i.uv.x - i.uv.y) * _lineWidth, (i.uv.x - i.uv.y));

                float4 color = lerp(_lineColor, _surfaceColor, diagonal * lineX * lineY);
                return color;
            }
            ENDCG
        }
    }
}
