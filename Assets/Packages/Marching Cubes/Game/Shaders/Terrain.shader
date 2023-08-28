Shader "Custom/Terrain"
{
	Properties
	{
		_RockInnerShallow ("Rock Inner Shallow", Color) = (1,1,1,1)
		_RockInnerDeep ("Rock Inner Deep", Color) = (1,1,1,1)
		_RockLight ("Rock Light", Color) = (1,1,1,1)
		_RockDark ("Rock Dark", Color) = (1,1,1,1)
		_GrassLight ("Grass Light", Color) = (1,1,1,1)
		_GrassDark ("Grass Dark", Color) = (1,1,1,1)


		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Test ("Test", Float) = 0.0

		_NoiseTex("Noise Texture", 2D) = "White" {}
	}
	SubShader
	{
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "ShaderModel"="4.5"}
		LOD 300
		
        Pass
        {
            Name "Terrain"
            Tags{"LightMode" = "UniversalForward"}

            Blend[_SrcBlend][_DstBlend]
            ZWrite[_ZWrite]
            Cull[_Cull]
			
			HLSLPROGRAM
			#pragma fragment fragmentProgramm

			struct fragmentInput
			{
				float4 positionCS : SV_POSITION;
				float3 normalWS : TEXTCOORD0;
			};
			struct fragmentOutput
			{
				float4 color : SV_Target;
			};

			fragmentOutput fragmentProgramm(fragmentInput input)
			{
				fragmentOutput output;
				output.color=_Color;
				return output;
			}
			ENDHLSL
		}
	}
	FallBack "Universal Render Pipeline/Lit"
}
