Shader "Custom/PSX Lit normal UV"
{
    Properties
    {
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        _BaseColor("Base Color", color) = (1,1,1,1)
        _VertexJittering("Vertex Jittering", float) = 0.07
        _Emmiter("Emmiter", Range(0,1)) = 0
        _Cull("Cull", float) = 0
        _AlphaToMask("AlphaToMask", float) = 1
        _Outline("Outline", Range(0,10)) = 0.1
        _OutlineColor("Outline Color", color) = (1,1,1,1)
    }
    SubShader
    {

        Tags
        {
            "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent" "RenderPipeline" = "UniversalRenderPipeline"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        Lighting Off
        
        Fog
        {
            Mode Off
        }

        AlphaToMask [_AlphaToMask]
        Cull[_Cull]

        Pass
        {
            HLSLPROGRAM
            #define Exposure 1.1

            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _RECEIVE_SHADOWS_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 tangentWS : TANGENT;
                float4 texcoord1 : TEXCOORD1;
                float4 bitangentWS : TEXCOORD5;
                float4 vertex_color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : NORMAL;
                float4 shadowCoord : TEXCOORD3;
                half3 viewDir : TEXCOORD5;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 4);
                float4 vertex_color : COLOR;
            };

            sampler2D _BaseMap;
            float _VertexJittering;
            float _Emmiter;
            half _Outline;
            float4 _BaseMap_ST;

            float4 _BaseColor;
            float4 _OutlineColor;
            float _Smoothness, _Metallic;

            inline half StepFeatherToon(half value, half step, half feather)
            {
                return saturate((value - step + feather) / feather);
            }

            v2f vert(appdata v)
            {
                v2f o;
                float d = length(mul(UNITY_MATRIX_MV, v.vertex));
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.vertex_color = v.vertex_color;
                o.vertex = TransformWorldToHClip(o.positionWS);
                o.vertex -= float4((o.vertex % _VertexJittering).xyz, 0);
                o.shadowCoord = TransformWorldToShadowCoord(o.positionWS);

                OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
                OUTPUT_SH(o.normalWS.xyz, o.vertexSH);
                o.viewDir = GetWorldSpaceViewDir(o.positionWS);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_BaseMap, i.uv);
                half4 color = col * _BaseColor;
                half3 diffuseColor = 0;

                for (int j = 0; j < GetAdditionalLightsCount(); ++j)
                {
                    Light light = GetAdditionalLight(j, i.positionWS);
                    half3 attenuatedLightColor = light.color * (light.distanceAttenuation);
                    diffuseColor += LightingLambert(attenuatedLightColor, light.direction, i.normalWS);
                }
                Light mainLight = GetMainLight(i.shadowCoord);
                half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation);
                diffuseColor += LightingLambert(attenuatedLightColor, mainLight.direction, i.normalWS);

                half4 fullColor = float4(max(min(pow(diffuseColor * i.vertex_color, Exposure), diffuseColor), _Emmiter),
                                         1) * color;
                fullColor = round(fullColor * 64) / 64;

                if (_Outline != 0)
                {
                    float alphaOutline = ceil(color.aaa - (1 - min(1 - color.aaa, 1 / _Outline) / (1 / _Outline)));
                    alphaOutline = 1 - alphaOutline;
                    float edgeNormal = sqrt(dot(i.viewDir, i.normalWS)) > _Outline ? 1 : 0;
                    float outlines = min(alphaOutline, edgeNormal);
                    //outlines = alphaOutline*edgeNormal;
                    fullColor = fullColor * float4(outlines.xxx, 1);
                    fullColor = fullColor + float4(1 - outlines.xxx, 1) * _OutlineColor;
                }
                fullColor.a = color.a;

                return fullColor;
            }
            ENDHLSL
        }
    }
}