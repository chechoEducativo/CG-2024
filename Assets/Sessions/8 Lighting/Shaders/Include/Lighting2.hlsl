#ifndef LINGHTING_2_INCLUDED
#define LINGHTING_2_INCLUDED

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

void GetMainLight_float(out float3 Direction, out float3 Color)
{
    Direction = float3(1,1,-1);
    Color = 1;
    #ifdef UNIVERSAL_LIGHTING_INCLUDED
    const Light light = GetMainLight();
    Direction = light.direction;
    Color = light.color;
    #endif
}

void Add_float(float A, float B, out float Result)
{
    Result = A + B;
}

#endif