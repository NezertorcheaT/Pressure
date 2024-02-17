Shader "Custom/Billboard"
{
    Properties
    {
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        _BaseColor("Base Color", color) = (1,1,1,1)
        _Emmiter("Emmiter", Range(0,1)) = 0
        _AlphaToMask("AlphaToMask", Range(0,1)) = 1
        _Cull("Cull", float) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True"
        }

        LOD 100
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Lighting Off

        Fog
        {
            Mode Off
        }

        AlphaToMask [_AlphaToMask]
        Cull [_Cull]
        Pass
        {
            HLSLPROGRAM
            #define Exposure 1.1

            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 vertex_color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 vertex_color : COLOR;
            };

            sampler2D _BaseMap;
            float _VertexJittering;
            float _Emmiter;
            float4 _BaseMap_ST;

            float4 _BaseColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.vertex_color = v.vertex_color;
                o.vertex = TransformWViewToHClip(
                    TransformWorldToView(GetObjectToWorldMatrix()._m03_m13_m23) + TransformObjectToWorldDir(
                        v.vertex, false));
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_BaseMap, i.uv);
                return col * _BaseColor;
            }
            ENDHLSL
        }
    }
}