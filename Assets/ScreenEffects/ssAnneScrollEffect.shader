Shader "Pokemon/S.S. Anne Scroll Effect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		shipScrollOffset ("Ship Scroll Offset", Float) = 0 //offset in pixels. divide by 160 for offset;
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
			float shipScrollOffset;
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
			if (i.uv.y > 1.0 / 9.0 && i.uv.y < 4.0 / 9.0) { //is the point in the area to be scrolled?
				float scrollX = shipScrollOffset / 160.0; //divide by 160 to convert to uv space
			col = tex2D(_MainTex, i.uv + float2(scrollX,0)); 
			if(i.uv.x + scrollX > 1.0) col = tex2D(_MainTex,  float2((i.uv.x + scrollX)%0.1,i.uv.y)); // mask the repeating effect on the right side with the left side to get a loop effect

			}
                return col;
            }
            ENDCG
        }
    }
}
