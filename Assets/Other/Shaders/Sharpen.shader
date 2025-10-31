
Shader"Hidden/Image Effects/Sharpen" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Pass 
		{							
			CGPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag

#include "UnityCG.cginc"	
			
uniform sampler2D _MainTex;
						
uniform float _strength;
uniform float _edgeMult;
			
float4 frag(v2f_img image) : SV_Target
{
    float4 textureColour = tex2D(_MainTex, image.uv);


    float4 sharpenedTextureColour = textureColour * 9.0f;

    float4 offset = 1.0f / (_ScreenParams / _edgeMult);

    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(offset.x, offset.y)); // Top right.
    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(0.0f, offset.y)); // Up.
    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(-offset.x, offset.y)); // Top left.
    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(-offset.x, 0.0f)); // Left.

    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(-offset.x, -offset.y)); // Bottom left.
    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(0.0f, -offset.y)); // Bottom.
    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(offset.x, -offset.y)); // Bottom right.
    sharpenedTextureColour -= tex2D(_MainTex, image.uv + float2(offset.x, 0.0f)); // Right.

    return lerp(textureColour, sharpenedTextureColour, _strength);
}

			ENDCG
		}
	}
}
