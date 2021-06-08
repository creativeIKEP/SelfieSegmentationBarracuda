Shader "Hidden/SelfieSegmentation/VirtualBackgroundVisuallizer"
{
    Properties
    {
        // Segmentation texture
        _MainTex ("Texture", 2D) = "white" {}
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _inputImage;
            sampler2D _backImage;
            float _threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_inputImage, i.uv);
                fixed4 mask = tex2D(_MainTex, i.uv);
                fixed4 back = tex2D(_backImage, i.uv);
                fixed4 p = (mask.r >= _threshold) ? col : back;
                return p;
            }
            ENDCG
        }
    }
}
