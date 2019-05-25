Shader "Pokemon/Battle Transitions"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
      uvOffset ("UV Offset",Int) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Off

        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int uvOffset;
            float4 _MainTex_TexelSize;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,0);
                float2 offsetVec = float2((float)uvOffset/160.0, (float)uvOffset/144.0);
                float2 newUV = float2(i.uv.x - sign(i.uv.x - 0.5) * offsetVec.x,i.uv.y  - sign(i.uv.y - 0.5) * offsetVec.y);
                bool insideImage = (i.uv.x >= 0.5 + offsetVec.x || i.uv.x <= 0.5 - offsetVec.x) && (i.uv.y >= 0.5 + offsetVec.y || i.uv.y <= 0.5 - offsetVec.y) && (i.uv.x >= 0.0 - offsetVec.x && i.uv.x <= 1.0 + offsetVec.x) && (i.uv.y <= 1.0 + offsetVec.y && i.uv.y >= 0.0 - offsetVec.y);
                if(insideImage) //are we still inside the image after the effect?
               col = tex2D(_MainTex,newUV); 
               else return float4(0,0,0,0);
                
                return col;
            }
            ENDCG
        }
    }
}
