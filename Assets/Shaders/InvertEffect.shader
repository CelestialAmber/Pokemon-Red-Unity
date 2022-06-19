Shader "Pokemon/Invert Effect" {
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
            float4 _MainTex_ST;
            fixed4 frag (v2f i) : SV_Target
            {
            	fixed4 c = tex2D(_MainTex,i.uv);
                return fixed4(1-c.x,1-c.y,1-c.z,c.w);
            }
        ENDCG
    }
}

}
