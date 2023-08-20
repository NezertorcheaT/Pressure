Shader "Custom/PSX Lit"
{
    Properties
    {
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        _BaseColor("Base Color", color) = (1,1,1,1)
        _VertexJittering("Vertex Jittering", float) = 0.07
        _Emmiter("Emmiter", Range(0,1)) = 0
        _Cull("Cull", float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalRenderPipeline"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Cull[_Cull]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_CALCULATE_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _RECEIVE_SHADOWS_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 texcoord1 : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                float3 normal : NORMAL;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 4);
            };

            sampler2D _BaseMap;
            float _VertexJittering;
            float _Emmiter;
            float4 _BaseMap_ST;

            float4 _BaseColor;
            float _Smoothness, _Metallic;

            v2f vert(appdata v)
            {
                v2f o;
                float d = length(mul(UNITY_MATRIX_MV, v.vertex));
                o.positionWS = TransformObjectToWorld(v.vertex.xyz);
                o.normal = TransformObjectToWorldNormal(v.normal.xyz);
                o.normalWS = d + (mul(UNITY_MATRIX_MVP, v.vertex).w * (UNITY_LIGHTMODEL_AMBIENT.a * 8)) / d / 2;
                o.viewDir = normalize(_WorldSpaceCameraPos - o.positionWS);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.uv *= d + (mul(UNITY_MATRIX_MVP, v.vertex).w * (UNITY_LIGHTMODEL_AMBIENT.a * 8)) / d / 2;
                o.vertex = TransformWorldToHClip(o.positionWS);
                o.vertex += float4(0, 0, 0, (o.vertex % _VertexJittering).w);
                OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
                OUTPUT_SH(o.normalWS.xyz, o.vertexSH);
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_BaseMap, i.uv / i.normalWS);
                half4 color = col * _BaseColor;
                /*InputData inputdata = (InputData)0;
                inputdata.positionWS = i.positionWS;
                inputdata.normalWS = normalize(i.normal);
                inputdata.viewDirectionWS = i.viewDir;
                inputdata.bakedGI = SAMPLE_GI(i.lightmapUV, i.vertexSH, inputdata.normalWS);

                SurfaceData surfacedata;
                surfacedata.albedo = (_BaseColor * col).rgb;
                surfacedata.specular = 0;
                surfacedata.metallic = 0;
                surfacedata.smoothness = 0;
                surfacedata.normalTS = 0;
                surfacedata.emission = 0;
                surfacedata.occlusion = 1;
                surfacedata.alpha = _BaseColor.a * col.a;
                surfacedata.clearCoatMask = 0;
                surfacedata.clearCoatSmoothness = 0;

                return UniversalFragmentPBR(inputdata, surfacedata);*/
                half3 diffuseColor = 0;
                //half3 specularColor = 0;

                for (int j = 0; j < GetAdditionalLightsCount(); ++j)
                {
                    Light light = GetAdditionalLight(j, i.positionWS);
                    half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
                    diffuseColor += LightingLambert(attenuatedLightColor, light.direction, i.normal);
                    //specularColor += LightingSpecular(attenuatedLightColor, light.direction, i.normal, i.viewDir, half4((1.0).xxx, 0), 0.5);
                }
                return max(float4(diffuseColor, 1),_Emmiter) * color;
                /*Light l;
                float a = 2;
                float3 dd;
                float3 c = 0;
                for (int ii = 0; ii < GetAdditionalLightsCount(); ii++)
                {
                    Light nl = GetAdditionalLight(ii, i.positionWS);
                    l.color += nl.color;
                    l.direction += nl.direction;
                    dd += cross(nl.direction, i.normal);
                    a += dot(nl.direction, i.normal);
                    l.distanceAttenuation += nl.distanceAttenuation;
                    //l.shadowAttenuation += GetAdditionalLightShadowParams(ii).x;
                    l.shadowAttenuation *= AdditionalLightRealtimeShadow(ii, i.positionWS, l.direction);
                    float NdotL = saturate(dot(normalize(_AdditionalLightsPosition[ii]), i.normal));
                    float3 ambient = SampleSH(i.normalWS);

                    float receiveshadow = AdditionalLightRealtimeShadow(ii, i.positionWS);
                    c += nl.color * receiveshadow * nl.shadowAttenuation + ambient;

                    //c += NdotL * nl.color * nl.shadowAttenuation + ambient;
                }

                //return float4(clamp(length(dd)*a,0,1).xxx, 1);
                //return float4(atten.xyz, 1);
                float intencity = 0.299 * l.color.r + 0.587 * l.color.g + 0.114 * l.color.b;
                return float4(
                    l.color / 8.71 * l.distanceAttenuation / clamp(length(dd), 0, 1),
                    1) * color;*/
            }
            ENDHLSL
        }
    }
}