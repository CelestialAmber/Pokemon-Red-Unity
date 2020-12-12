Shader "Pokemon/Psychic Effect" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    }
SubShader {
		Cull Off ZWrite Off ZTest Always
 Tags { "RenderType"="Opaque" }

    Pass {
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            

            v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
            static int waveData[32] = {0,0,0,0,0,1,1,1,2,2,2,2,2,1,1,1,0,0,0,0,0,-1,-1,-1,-2,-2,-2,-2,-2,-1,-1,-1}; //wave effect data in pixel offsets, which approximates a sine wave 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 pos = floor(i.uv * _MainTex_TexelSize.zw);
                float currentFrame = (floor(_Time.y * 120) % 128); //the animation lasts for 128 frames
                if(pos.y >= 141.0) return col; //bug where the top 3 pixels of the screen aren't affected by the wave effect. the pixels are shifted 2 to the left?
                float offsetValue =  (float)waveData[(int)((currentFrame + pos.y*2)%32)];//each scanline is iterated over twice
                float offset = offsetValue/160.0;
                return tex2D(_MainTex, float2(i.uv.x+offset,i.uv.y));
            }
        ENDCG
    }
}

}
