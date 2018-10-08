sampler s0 : register(s0);
texture ground : register(t1);
sampler s1 : register(s1)
{
	Texture = <ground>;
};

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 colorFG = tex2D(s0, coords);
	float4 color = tex2D(s1, coords);
	if (!any(colorFG))
	{
		return 0;
	}
	if (color.a == 0.0f)
	{
		//return 0;
	}

	if (color.r > 0.9f && color.g > 0.9f && color.b > 0.9f && color.a == 1.0f)
	{
		return colorFG;
	}

	return 0;


}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}