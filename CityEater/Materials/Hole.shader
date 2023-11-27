Shader "BlackHole/black_hole"
{
    Properties{
    }
    SubShader
    {
        Tags {  "Queue"="Geometry-1"}
        Lighting off
 
        ColorMask 0
 
        Pass{
           ZWrite On
           ZTest LEqual
           ColorMask 0
        }
    }
}