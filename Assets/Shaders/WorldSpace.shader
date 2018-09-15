﻿Shader "Custom/WorldSpace" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTexWall ("Albedo (RGB)", 2D) = "white" {}
		_MainTexTop ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Scale ("Texture Scale", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTexWall;
		sampler2D _MainTexTop;

		struct Input {
			float3 worldNormal;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _Scale;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 UV;
			fixed4 c;

			if(abs(IN.worldNormal.x)>0.5)
			{
				UV = IN.worldPos.xy; //side
				c = tex2D(_MainTexWall, UV * _Scale);
			}
			else if(abs(IN.worldNormal.z)>0.5)
			{
				UV = IN.worldPos.xy; //front
				c = tex2D(_MainTexWall, UV * _Scale);
			}
			else
			{
				UV = IN.worldPos.xz; //top
				c = tex2D(_MainTexTop, UV * _Scale);
			}			

			o.Albedo = c.rgb * _Color;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
