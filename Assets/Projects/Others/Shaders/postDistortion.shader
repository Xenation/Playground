// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:6,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32740,y:33254,varname:node_2865,prsc:2|emission-4676-OUT;n:type:ShaderForge.SFN_TexCoord,id:4219,x:31252,y:32955,cmnt:Default coordinates,varname:node_4219,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Relay,id:4676,x:32523,y:33354,cmnt:Modify color here,varname:node_4676,prsc:2|IN-7542-RGB;n:type:ShaderForge.SFN_Tex2dAsset,id:4430,x:31938,y:33424,ptovrint:False,ptlb:MainTex,ptin:_MainTex,cmnt:MainTex contains the color of the scene,varname:node_9933,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7542,x:32339,y:33354,varname:node_1672,prsc:2,ntxv:0,isnm:False|UVIN-2968-OUT,TEX-4430-TEX;n:type:ShaderForge.SFN_Sin,id:7402,x:31585,y:33205,varname:node_7402,prsc:2|IN-2450-OUT;n:type:ShaderForge.SFN_Multiply,id:2450,x:31418,y:33091,varname:node_2450,prsc:2|A-4219-U,B-1773-OUT;n:type:ShaderForge.SFN_Append,id:2968,x:32121,y:33112,varname:node_2968,prsc:2|A-4219-U,B-2889-OUT;n:type:ShaderForge.SFN_Add,id:2889,x:31938,y:33250,varname:node_2889,prsc:2|A-4219-V,B-2149-OUT;n:type:ShaderForge.SFN_Multiply,id:2149,x:31761,y:33295,varname:node_2149,prsc:2|A-7402-OUT,B-2254-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1773,x:31252,y:33125,ptovrint:False,ptlb:DistortionFrequency,ptin:_DistortionFrequency,varname:node_1773,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:34.5;n:type:ShaderForge.SFN_Slider,id:2254,x:31428,y:33362,ptovrint:False,ptlb:DistortionScale,ptin:_DistortionScale,varname:node_2254,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.1;proporder:4430-1773-2254;pass:END;sub:END;*/

Shader "Shader Forge/postDistortion" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _DistortionFrequency ("DistortionFrequency", Float ) = 34.5
        _DistortionScale ("DistortionScale", Range(0, 0.1)) = 0
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _DistortionFrequency;
            uniform float _DistortionScale;
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
////// Lighting:
////// Emissive:
                float2 node_2968 = float2(i.uv0.r,(i.uv0.g+(sin((i.uv0.r*_DistortionFrequency))*_DistortionScale)));
                float4 node_1672 = tex2D(_MainTex,TRANSFORM_TEX(node_2968, _MainTex));
                float3 emissive = node_1672.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
