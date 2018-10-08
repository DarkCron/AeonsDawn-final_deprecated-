sampler s0 : register(s0);
texture objects : register(t1);
texture noiseMap : register(t2);
sampler s1 : register(s1)
{
	Texture = <objects>;
};
sampler s2 : register(s2)
{
	Texture = <noiseMap>;
};
float distanceHorMod = 0.0f;
float distanceMod = 6.0f;
float width = 100;
float height = 100;
float noiseWidth = 2048;
float noiseHeight = 2048;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{



	float4 color = tex2D(s0, coords);
	//if (!any(color))
	//{
	//	return 0;
	//}
	if (color.a == 0.0f)
	{
		return 0;
	}

	
	float2 tempCoords = float2((coords.x * width) / noiseWidth, (coords.y * height + (float) (distanceHorMod)) / noiseHeight);
	float distort = tex2D(s2, tempCoords).r;
	float distance = 0.0f;
	if (distort >= 0.4f)
	{
		distance = -distanceMod / 2 * (distort - 0.4f);
	}
	else
	{
		distance = distanceMod / 2 * (0.4f - distort);
	}

	tempCoords = float2(coords.x + (float) (distance), -coords.y + ((float) 30 / (float) 768));

	float4 objectColor = tex2D(s1, tempCoords);

	//if (any(groundColor))
	//{
	//	return 0;
	//}
	if (objectColor.a != 0.0f)
	{
		objectColor.r *= float(1.2f);
		objectColor.g *= float(1.2f);
		objectColor.b *= float(1.2f);
		return objectColor * color;
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