sampler s0;
int param1 = 5;
float4 otherC;
float texWidth;
float texHeight;
float sourcePosX;
float sourcePosY;
float sourceWidth;
float sourceHeight;
float horizontalModifier;
float divider = 2;
float multiplier = 0;
float2 cords;


float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float currentYcoord = (coords.y * texHeight) - sourcePosY;
	float flippedY = sourceHeight - currentYcoord + sourcePosY;

	float4 color = tex2D(s0, float2(coords.x, (flippedY / texHeight)));
	if (!any(color))
	{
		return 0;
	}
	else
	{
		//currentYcoord = (coords.y * texHeight) - sourcePosY;
		//color.rgb = float3(1.0f - (float) currentYcoord / (float) sourceHeight, 1.0f - (float) currentYcoord / (float) sourceHeight, 1.0f - (float) currentYcoord / (float) sourceHeight);
		color.a = 0.5f - 0.5f * (1.0f - (float) currentYcoord / (float) sourceHeight);
		return color;
	}
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}