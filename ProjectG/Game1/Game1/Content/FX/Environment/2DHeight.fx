sampler s0;
int param1 = 5;
float stdHeight = 128;
float gameHeight = 64;


float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{

	float4 color = tex2D(s0, coords);
	if (!any(color))
	{
		return 0;
	}
	else
	{
		//currentYcoord = (coords.y * texHeight) - sourcePosY;
		//color.rgb = float3(1.0f - (float) currentYcoord / (float) sourceHeight, 1.0f - (float) currentYcoord / (float) sourceHeight, 1.0f - (float) currentYcoord / (float) sourceHeight);
		float4 tempColor = float4(0.0f, 0.0f, 0.0f, 1.0f);
		float multiplier = stdHeight / gameHeight;
		if (multiplier<=1)
		{
			tempColor.r = 1.0f - (coords.y * multiplier);
			tempColor.g = 1.0f - (coords.y * multiplier);
			tempColor.b = 1.0f - (coords.y * multiplier);
		}
		else
		{
			tempColor.r = 1.0f - (coords.y / multiplier);
			tempColor.g = 1.0f - (coords.y / multiplier);
			tempColor.b = 1.0f - (coords.y / multiplier);
		}

		if (tempColor.r < 0)
		{
			tempColor.r = 0.0f;
		}
		if (tempColor.g < 0)
		{
			tempColor.g = 0.0f;
		}
		if (tempColor.b < 0)
		{
			tempColor.b = 0.0f;
		}
		return tempColor;
	}
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}