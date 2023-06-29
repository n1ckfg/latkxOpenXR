Shader "QPassthrough/ColorPicker/SolidColor" { 
    Properties {
        _Color ("Color", Color) = (1,1,1)
    }
 
    SubShader {
        Color [_Color]
		Tags {"Queue" = "Transparent+1" "IgnoreProjector" = "True" "RenderType" = "Opaque"}
		LOD 100

		Pass {}
    } 
}