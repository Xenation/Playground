Shader "Shader Forge/screenDebug" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
				float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
				float4 screenPos2 : TEXCOORD2;
				float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.screenPos = o.pos;
				o.screenPos2 = float4(o.pos.xy / o.pos.w, 0, 0);
				o.screenPos2.y *= _ProjectionParams.x;
				o.screenPos2 = float4((o.screenPos2.rg + 1.0)*0.5, 0, 0);
				o.uv0 = v.texcoord0;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
////// Lighting:
////// Emissive:
                float2 node_367 = ((i.screenPos2.rg+1.0)*0.5);
                float4 _Texture_var = tex2D(_Texture, i.uv0);
				//float4 _Texture_var = tex2D(_Texture, i.screenPos2.xy);
                float3 emissive = _Texture_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor.rgb, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
