
sampler s0;
texture lightMask;
sampler lightSampler = sampler_state
{
	Texture = <lightMask>;
};
sampler heightSampler = sampler_state
{
	Texture = <heightMask>;
};
sampler truelightSampler = sampler_state
{
	Texture = <truelightMask>;
};
float4 lightColor;
float brightness;
float mod;
float mod2;


float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	
	float4 heightMask = tex2D(heightSampler, coords);
	heightMask.r = 1 - heightMask.r * mod;
	heightMask.g = 1 - heightMask.g * mod;
	heightMask.b = 1 - heightMask.b * mod;

	return tex2D(lightSampler, coords) * color * tex2D(lightSampler, coords) * (heightMask);
	
	if (heightMask.a != 0)
	{
		float heightMod = 1.0f;

		float4 truelightColor = tex2D(truelightSampler, coords);
		float4 lightColor = tex2D(lightSampler, coords);
		heightMod = heightMask.r;
		if (truelightColor.a == 0)
		{

			float4 colorEnd = color * lightColor * lightColor;

			colorEnd.r -= (heightMod * mod);
			colorEnd.g -= (heightMod * mod);
			colorEnd.b -= (heightMod * mod);
			if (colorEnd.r < 0.0f)
			{
				colorEnd.r = 0.0f;
			}
			if (colorEnd.g < 0.0f)
			{
				colorEnd.g = 0.0f;
			}
			if (colorEnd.b < 0.0f)
			{
				colorEnd.b = 0.0f;
			}
			return colorEnd;
			

		}
		
		float4 colorEnd;
		if (brightness < 2.75f)
		{
			lightColor.r = (1.0f - heightMask.r) * mod2;
			lightColor.g = (1.0f - heightMask.r) * mod2;
			lightColor.b = (1.0f - heightMask.r) * mod2;

			colorEnd = color * lightColor * lightColor;
			colorEnd.r -= (brightness * mod);
			colorEnd.g -= (brightness * mod);
			colorEnd.b -= (brightness * mod);
		}
		else
		{
			float temp = 1.0f - brightness;
			temp = temp / 0.25f;
			lightColor.r = 1.0f - ((heightMask.r * temp * 1 + 0.03f)) * mod2;
			lightColor.g = 1.0f - ((heightMask.r * temp * 1 + 0.03f)) * mod2;
			lightColor.b = 1.0f - ((heightMask.r * temp * 1 + 0.03f)) * mod2;
			colorEnd = color * lightColor * lightColor;
			//colorEnd.r -= (brightness * mod) * 1 * brightness;
			//colorEnd.g -= (brightness * mod) * 1 * brightness;
			//colorEnd.b -= (brightness * mod) * 1 * brightness;
		}



		if (colorEnd.r < 0.0f)
		{
			colorEnd.r = 0.0f;
		}
		if (colorEnd.g < 0.0f)
		{
			colorEnd.g = 0.0f;
		}
		if (colorEnd.b < 0.0f)
		{
			colorEnd.b = 0.0f;
		}
		return colorEnd;
	}
	else
	{
		float4 lightColor = tex2D(lightSampler, coords);
		return color * lightColor * lightColor;
	}
	
	
}

technique Light
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}