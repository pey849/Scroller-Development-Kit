//http://blog.josack.com/2011/07/my-first-2d-pixel-shaders-part-1.html
//http://www.neatware.com/lbstudio/web/hlsl.html
//NOTES: Pixel shaders: the 'Functions' are called for every pixel.
//if(color.r > 0.8 && color.a)
//	color.rgb = 0;

float4 Color;
texture Mask;
sampler s0;
sampler lightSampler = sampler_state{
	Texture = Mask;
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Clamp;
	AddressV = Clamp;
};

float4 MaskFunction(float2 coords: TEXCOORD0) : COLOR0   
{   
	float4 color = tex2D(s0, coords);  
	float4 lightColor = tex2D(lightSampler, coords);
    return color * lightColor;// test;
}   
  
float4 SilhouetteFunction(float2 coords: TEXCOORD0) : COLOR0   
{   
	float4 color = tex2D(s0, coords);  
	color.rgb = 0;
    return color;
} 

float4 ForceColorFunction(float2 coords: TEXCOORD0) : COLOR0   
{   
	float4 color = tex2D(s0, coords);  
	if(color.a != 0)
		color.rgb = Color.rgb;
    return color;
} 
  


//Currently outlines mario
float4 ShowRedOnlyFunction(float2 coords: TEXCOORD0) : COLOR0   
{   
	float4 color = tex2D(s0, coords);
	
	if(color.a != 0 && color.r < 0.9 && color.g < 0.2 && color.b < 0.1){
		color.r = 1;
		color.g = 0;
		color.b = 0;
	}
	else if(color.a != 0)
		color.rgb = 0;

	//negative effect
	/*if(color.a)
		color.rgb = 1 - color.rgb;*/
    return color;
} 

technique Technique1   
{   
	//Pass 0
    pass Pass1   
    {   
        PixelShader = compile ps_2_0 MaskFunction();   
    }   

	//Pass 1
	pass Silhouette
	{
		 PixelShader = compile ps_2_0 SilhouetteFunction();   
	}
	
	// pass 2
	pass ShowRedOnly
	{
		 PixelShader = compile ps_2_0 ShowRedOnlyFunction();   
	}

	// pass 3
	pass ForceColor
	{
		 PixelShader = compile ps_2_0 ForceColorFunction();   
	}
}  
