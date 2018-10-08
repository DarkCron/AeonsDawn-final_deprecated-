sampler s0 : register(s0);
texture objects : register(t1);
sampler s1 : register(s1)
{
	Texture = <objects>;
};

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
	float4 objectColor = tex2D(s1, coords);

	//if (any(groundColor))
	//{
	//	return 0;
	//}
	if (objectColor.a != 0.0f)
	{
		return objectColor*color;
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