Shader "Pokemon/Palette Effect"
{
	Properties
	{
		 _MainTex ("Texture", 2D) = "white" {}
        color1 ("Color 1", Color) = (1,1,1,1)
        color2 ("Color 2", Color) = (.564,.564,.564,1)
        color3 ("Color 3", Color) = (.25,.25,.25,1)
        color4 ("Color 4", Color) = (0,0,0,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
        
		Pass
		{
        Name "MainEffects"

        CGPROGRAM
        #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            
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
            float4 color1, color2, color3, color4;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float4 frag (v2f i) : SV_Target
			{
            float4 color = tex2D(_MainTex,i.uv);
                if(!any(float4(1,1,1,1) - color)){
                color = color1;
                }
                else if(color.r >= .564 && color.g >= .564 && color.b >= .564){
                color = color2;
                }
                else if(color.r >= .25 && color.g >= .25 && color.b >= .25){
                color = color3;
                }
                else if(!any(float4(0,0,0,1) - color)){
                color = color4;
                }
                return color;

			}
            ENDCG

		}
        

        
	}
}
