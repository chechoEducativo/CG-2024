#ifndef LIGHTING_INCLUDED
#define LIGHTING_INCLUDED

 //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
// #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

void GetMainLight_float(float3 positionWS, out float3 Direction, out float3 Color, out float ShadowAttenuation)
{
    Direction = float3(1,1,-1);
    Color = 1;
    ShadowAttenuation = 1.0;
    
    #ifdef UNIVERSAL_LIGHTING_INCLUDED
    float4 shadowCoords = TransformWorldToShadowCoord(positionWS);
    Light light = GetMainLight(shadowCoords);
    Direction = light.direction;
    Color = light.color;
    ShadowAttenuation = light.shadowAttenuation;
    #endif
}

void GetMainLight_half(float3 positionWS, out float3 Direction, out float3 Color, out float ShadowAttenuation)
{
    Direction = float3(1,1,-1);
    Color = 1;
    ShadowAttenuation = 1.0;

    #ifdef UNIVERSAL_LIGHTING_INCLUDED
    float4 shadowCoords = TransformWorldToShadowCoord(positionWS);
    Light light = GetMainLight(shadowCoords);
    Direction = light.direction;
    Color = light.color;
    ShadowAttenuation = light.shadowAttenuation;
    #endif
}

#endif

