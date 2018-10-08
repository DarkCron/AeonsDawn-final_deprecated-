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
	//float4 color = tex2D(s0, coords);
	//if (!any(color))
	//{
 //   //    return 0;
	//}

	float currentXcoord = (coords.x * texWidth) - sourcePosX;
	float currentYcoord = (coords.y * texHeight) - sourcePosY;
	float height = sourceHeight - currentYcoord;

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

	
	//float4

	if (xPosAfter > sourcePosX && xPosAfter < sourcePosX + sourceWidth)
	{
		cords = float2(adjustedposX, coords.y);
		return tex2D(s0, cords);
	}
	else
	{
		return 0;
		//color.a = 0.0f;
	}

  //  adjustedposX = (float)(( (float)coords.x * (float)texWidth ) / (float)texWidth);
   
     //color = tex2D(s0, float2(adjustedposX,coords.y));
    //adjustedposX = coords.x;
    //color = tex2D(s0, float2(adjustedposX, coords.y));

	//return color;
}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}