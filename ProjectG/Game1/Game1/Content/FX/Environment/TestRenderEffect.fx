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
int hMod = 5;
float zoom = 1.0f;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	return tex2D(s0,coords);


}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}