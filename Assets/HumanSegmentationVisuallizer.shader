Shader "Hidden/SelfieSegmentation/HumanSegmentationVisuallizer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _inputImage ("Texture", 2D) = "white" {}
    }
    SubShader
    {
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
            sampler2D _inputImage;
            float4 _inputImage_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_inputImage, i.uv);
                fixed4 mask = tex2D(_MainTex, i.uv);
                mask = (mask.r > 0.95) ? mask : fixed4(0, 0, 0, 1);
                return col * mask;
            }
            ENDCG
        }
    }
}
