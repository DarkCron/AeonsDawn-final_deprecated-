#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};
sampler s0;
float modifier = 1.2f;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 colorSprites = tex2D(s0, input.TextureCoordinates);
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	if (color.a == 0)
	{
		return colorSprites;
	}

	if (colorSprites.a == 0)
	{
		return color;
	}




	colorSprites.r *= modifier;
	colorSprites.g *= modifier;
	colorSprites.b *= modifier;

	return colorSprites * color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};