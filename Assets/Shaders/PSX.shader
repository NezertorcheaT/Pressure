Shader "Custom/PSX Lit"
{
    Properties
    {
        [MainTexture] _BaseMap("Albedo", 2D) = "white" {}
        _BaseColor("Base Color", color) = (1,1,1,1)
        _VertexJittering("Vertex Jittering", float) = 0.07
        _Emmiter("Emmiter", Range(0,1)) = 0
        _Cull("Cull", float) = 0
        _AlphaToMask("AlphaToMask", Range(0,1)) = 1
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
            #pragma shader_feature _LIGHT_COOKIES


            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/LightCookie/LightCookie.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float4 vertex_color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                noperspective float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : NORMAL;
                float4 shadowCoord : TEXCOORD3;
                half3 viewDir : TEXCOORD5;
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 4);
                float4 vertex_color : COLOR;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;

            float4 _BaseColor, _OutlineColor;
            half _VertexJittering, _Emmiter, _Outline;

            v2f vert(appdata v)
            {
                v2f o;
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
                half3 diffuse_color = 0;

                for (int j = 0; j < GetAdditionalLightsCount(); ++j)
                {
                    Light light = GetAdditionalLight(j, i.positionWS);

                    #if USE_CLUSTERED_LIGHTING
                         int cookie_light_index = j;
                    #else
                    int cookie_light_index = GetPerObjectLightIndex(j);
                    #endif

                    float3 cookieColor = SampleAdditionalLightCookie(cookie_light_index, i.positionWS);
                    half3 attenuated_light_color = light.color * cookieColor * light.distanceAttenuation;
                    diffuse_color += LightingLambert(attenuated_light_color, light.direction, i.normalWS);
                }
                
                Light main_light = GetMainLight(i.shadowCoord);
                real3 cookieColor = SampleMainLightCookie(i.positionWS);
                half3 attenuated_light_color = main_light.color * cookieColor * (main_light.distanceAttenuation);
                diffuse_color += LightingLambert(attenuated_light_color, main_light.direction, i.normalWS);

                half4 full_color = float4(
                    max(min(pow(abs(diffuse_color * i.vertex_color), Exposure), diffuse_color), _Emmiter),
                    1) * color;
                full_color.a = color.a;

                return full_color;
            }
            ENDHLSL
        }
    }
}