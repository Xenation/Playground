Shader "Shader Forge/postDepth" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Overlay+1"
            "RenderType"="Overlay"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZTest Always
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
			uniform float4 _CameraDepthTexture_ST;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
            
			struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            
			struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            
			VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            
			float4 frag(VertexOutput i) : COLOR {
                float4 depth = tex2D(_CameraDepthTexture,TRANSFORM_TEX(i.uv0, _CameraDepthTexture)).x;
				float4 mainColor = tex2D(_MainTex, TRANSFORM_TEX(i.uv0, _MainTex));
                float3 emissive = depth;
                float3 finalColor = emissive;
                return fixed4(finalColor, 1);
            }
            ENDCG
        }
    }
}
