#include "./A.hlsl"
float HelloWorld()
{
    return 1;
}
#include "./B.hlsl"
float HelloWorld()
{
    return 1;
}
float x = HelloWorld();



