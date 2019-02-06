Shader "Pokemon/Palette Effect"
{
	Properties
	{
		 _MainTex ("Base (RGB)", 2D) = "white" {}
		 //default colors that pokemon rb uses
        color1 ("Color 1", Color) = (1,1,1,1)
        color2 ("Color 2", Color) = (.564,.564,.564,1)
        color3 ("Color 3", Color) = (.25,.25,.25,1)
        color4 ("Color 4", Color) = (0,0,0,1)
		//value determining the current screen flash level used. e.g. Wild Encounters
		flashLevel ("Screen Flash", Range(-3,3)) = 0
		screenPos ("Screen Position", Vector) = (0,0,0,0)

	}
	SubShader
	{
			Cull Off ZWrite Off ZTest Always
		Tags { "RenderType"="Opaque" }
        
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
			int flashLevel;
			int useRockTunnelColors;
			float2 screenPos;
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

//store colors in an array to make creating the flash effect easier
				float4 colors[4] = {color1,color2,color3,color4};
				i.uv += screenPos;
            float4 color = tex2D(_MainTex,i.uv);
			if(i.uv.x < 0 || i.uv.x > 1 || i.uv.y < 0 || i.uv.y > 1) color = float4(1,1,1,1);
                if(!any(float4(1,1,1,1) - color)){
                color = colors[0 + (flashLevel < 0 ? -flashLevel : 0)];
                }
				//make the value thresholds lower to account for possible deviation in color
                else if(color.r >= .66 && color.g >= .66 && color.b >= .66){ //the color value on avg is 0.68
                color = colors[1 + (flashLevel < 0 ? min(-flashLevel,2) : -min(flashLevel,1))];
                }
                else if(color.r >= .38 && color.g >= .38 && color.b >= .38){ //avg is 0.39
                color = colors[2 + (flashLevel < 0 ? min(-flashLevel,1) : -min(flashLevel,2))];
                }
                else if(!any(float4(0,0,0,1) - color)){
                color = colors[3 + (flashLevel < 0 ? min(-flashLevel,0) : -min(flashLevel,3))];
				
                }

                return color;

			}
            ENDCG

		}
        

        
	}
}
