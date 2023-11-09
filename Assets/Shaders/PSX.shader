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
            #define Exposure 1.1

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
                float4 vertex_color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                noperspective float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : NORMAL;
                float4 shadowCoord : TEXCOORD3;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 4);
                float4 vertex_color : COLOR;
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
                o.normalWS = TransformObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                o.vertex_color = v.vertex_color;
                /*
                v.vertex = mul(unity_ObjectToWorld, v.vertex);
                v.vertex /= _VertexJittering;
                v.vertex = round(v.vertex);
                v.vertex *= _VertexJittering;
                v.vertex = mul(unity_WorldToObject, v.vertex);
                */
                o.vertex = TransformWorldToHClip(o.positionWS);
                o.shadowCoord = TransformWorldToShadowCoord(o.positionWS);
                //o.vertex += float4(0, 0, 0, (o.vertex % _VertexJittering).w);
                o.vertex -= float4((o.vertex % _VertexJittering).xyz, 0);

                OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
                OUTPUT_SH(o.normalWS.xyz, o.vertexSH);
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
                
                half4 fullColor=float4(max(min(pow(diffuseColor * i.vertex_color, Exposure), diffuseColor), _Emmiter), 1) * color;
                return round(fullColor*64)/64;
                //return max(float4(diffuseColor-(diffuseColor%(0.125/2)), 1), _Emmiter) * color; 
            }
            ENDHLSL
        }
    }
}