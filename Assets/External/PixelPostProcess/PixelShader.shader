Shader "Hidden/GGS/Pixel Post Process"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			// Custom post processing effects are written in HLSL blocks,
			// with lots of macros to aid with platform differences.
			// https://github.com/Unity-Technologies/PostProcessing/wiki/Writing-Custom-Effects#shader
			HLSLPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

	// Data pertaining to _MainTex's dimensions.
	// https://docs.unity3d.com/Manual/SL-PropertiesInPrograms.html
	float4 _MainTex_TexelSize;

	float _Scale;
	float _Steps;

	// This matrix is populated in PostProcessOutline.cs.
	float4x4 _ClipToView;

	// Both the Varyings struct and the Vert shader are copied
	// from StdLib.hlsl included above, with some modifications.
	struct Varyings
	{
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoordStereo : TEXCOORD1;
		float3 viewSpaceDir : TEXCOORD2;
	#if STEREO_INSTANCING_ENABLED
		uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
	#endif
	};

	Varyings Vert(AttributesDefault v)
	{
		Varyings o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
		// Transform our point first from clip to view space,
		// taking the xyz to interpret it as a direction.
		o.viewSpaceDir = mul(_ClipToView, o.vertex).xyz;

	#if UNITY_UV_STARTS_AT_TOP
		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
	#endif

		o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);

		return o;
	}

	float4 Frag(Varyings i) : SV_Target
	{
		float2 uv = i.texcoord;
		uv *= _Scale;
		uv.xy = floor(uv.xy);
		uv.xy /= _Scale;

		float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).rgb;
		color *= floor(length(color) * _Steps) / _Steps;
		return float4(color.rgb, 1);
	}
	ENDHLSL
}
	}
}
