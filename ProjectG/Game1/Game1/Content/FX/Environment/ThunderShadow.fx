﻿sampler s0;
int param1 = 5;
float4 otherC;
float texWidth;
float texHeight;
float sourcePosX;
float sourcePosY;
float sourceWidth;
float sourceHeight;
float horizontalModifier;
float2 cords;
float alpha;
float divider = 2;
bool bUsesEffect = false;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{

	//float4 color = tex2D(s0, coords);
	//if (any(color) && !bUsesEffect)
	//{
	//	return 0;
	//}
	//if (!any(color))
	//{
	//	 // return 0;
	//}
	float currentXcoord = (coords.x * texWidth) - sourcePosX;
	float currentYcoord = (coords.y * texHeight) - sourcePosY;
	float height = sourceHeight - currentYcoord;
	float multiplier = 0;

	if (divider > height)
	{
		multiplier = 0;
	}
	else
	{
		while (multiplier * divider < height)
		{
			multiplier++;
		}
	}



	float adjustedposX = (coords.x * texWidth + multiplier * horizontalModifier) / texWidth;
	int xPosAfter = adjustedposX * texWidth;

	if (xPosAfter > sourcePosX + texWidth)
	{
		return 0;
	}

	  //  adjustedposX = (float)(( (float)coords.x * (float)texWidth ) / (float)texWidth);

		 //color = tex2D(s0, float2(adjustedposX,coords.y));
		//adjustedposX = coords.x;
		//color = tex2D(s0, float2(adjustedposX, coords.y));
	cords = float2(adjustedposX, coords.y);

	float4 color = tex2D(s0, cords);

	if (xPosAfter > sourcePosX && xPosAfter < sourcePosX + sourceWidth)
	{
		color.rgb = float3(0.0f, 0.0f, 0.0f);

		if (color.a)
		{
			color.a = alpha;
		}

	}
	else
	{
		return 0;
			//color = tex2D(s0, coords);
		color.rgb = float3(0.0f, 0.0f, 0.0f);
		color.a = 0.0f;
	}



	return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}