sampler s0;
int param1 = 5;
float4 otherC;
float texWidth;
float texHeight;
float sourcePosX;
float sourcePosY;
float sourceWidth;
float sourceHeight;

float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
   
    float stepX = sourcePosX / texWidth;
    float stepY = sourcePosY / texHeight;

    float maxX = (sourcePosX+sourceWidth*3/4) / texWidth;

    float4 color = tex2D(s0, coords);
    if (coords.x>maxX)
    {
        return color * otherC;
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