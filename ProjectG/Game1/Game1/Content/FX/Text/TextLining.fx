int width = 0;
int height = 0;
sampler s0;
int liningSize = 3;
float4 liningColor = float4(1.0f, 1.0f, 1.0f, 1.0f);
float r = 1.0f;
float g = 1.0f;
float b = 1.0f;
float a = 1.0f;


float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	if (color.a < .9f)
	{
		return 0;
	}

	if (width == 0 || height == 0 || width < 3 || height < 3 || liningSize < 0)
	{
		return 0;
	}

	liningColor = float4(r, g, b, a);
	//liningSize += 1;
	float pixelWidth = 1 / (float) (width);
	float pixelHeight = 1 / (float) (height);
	float adjustedXLeft = coords.x + ((float) (liningSize) * pixelWidth);
	float adjustedXRight = coords.x - ((float) (liningSize) * pixelWidth);
	float adjustedYLeft = coords.y + ((float) (liningSize) * pixelHeight);
	float adjustedYRight = coords.y - ((float) (liningSize) * pixelHeight);

	color = tex2D(s0, float2(adjustedXLeft, coords.y));
	if (color.a < 0.01f)
	{
		return liningColor;
	}
	color = tex2D(s0, float2(adjustedXRight, coords.y));
	if (color.a < 0.01f)
	{
		return liningColor;
	}
	color = tex2D(s0, float2(coords.x, adjustedYLeft));
	if (color.a < 0.01f)
	{
		return liningColor;
	}
	color = tex2D(s0, float2(coords.x, adjustedYRight));
	if (color.a < 0.01f)
	{
		return liningColor;
	}

	return 0;
	//return liningColor;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}