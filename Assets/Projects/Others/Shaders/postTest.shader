// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:False,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:1,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:6,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:1,qpre:4,rntp:5,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32942,y:33244,varname:node_2865,prsc:2|emission-7542-RGB;n:type:ShaderForge.SFN_TexCoord,id:4219,x:30981,y:33248,cmnt:Default coordinates,varname:node_4219,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2dAsset,id:4430,x:32551,y:33520,ptovrint:False,ptlb:MainTex,ptin:_MainTex,cmnt:MainTex contains the color of the scene,varname:node_9933,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7542,x:32747,y:33345,varname:node_1672,prsc:2,ntxv:0,isnm:False|UVIN-2908-OUT,TEX-4430-TEX;n:type:ShaderForge.SFN_Tex2d,id:7237,x:31583,y:33021,ptovrint:False,ptlb:NoiseU,ptin:_NoiseU,varname:node_7237,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-1532-OUT;n:type:ShaderForge.SFN_Add,id:4675,x:32184,y:33363,varname:node_4675,prsc:2|A-1288-OUT,B-4219-V;n:type:ShaderForge.SFN_Append,id:8725,x:31224,y:33039,varname:node_8725,prsc:2|A-4219-U,B-497-OUT;n:type:ShaderForge.SFN_Vector1,id:497,x:31053,y:33134,varname:node_497,prsc:2,v1:0;n:type:ShaderForge.SFN_Slider,id:9799,x:31601,y:32915,ptovrint:False,ptlb:DistortionIntensity,ptin:_DistortionIntensity,varname:node_9799,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.01,max:0.1;n:type:ShaderForge.SFN_Multiply,id:1288,x:31940,y:33004,varname:node_1288,prsc:2|A-9799-OUT,B-210-OUT;n:type:ShaderForge.SFN_Append,id:2908,x:32551,y:33304,varname:node_2908,prsc:2|A-5871-OUT,B-5761-OUT;n:type:ShaderForge.SFN_Append,id:4950,x:31224,y:33189,varname:node_4950,prsc:2|A-497-OUT,B-4219-V;n:type:ShaderForge.SFN_Tex2d,id:7434,x:31583,y:33194,ptovrint:False,ptlb:NoiseV,ptin:_NoiseV,varname:node_7434,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-8032-OUT;n:type:ShaderForge.SFN_Multiply,id:7989,x:31940,y:33146,varname:node_7989,prsc:2|A-9799-OUT,B-6612-OUT;n:type:ShaderForge.SFN_Add,id:310,x:32184,y:33238,varname:node_310,prsc:2|A-7989-OUT,B-4219-U;n:type:ShaderForge.SFN_RemapRange,id:6612,x:31758,y:33146,varname:node_6612,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-7434-R;n:type:ShaderForge.SFN_RemapRange,id:210,x:31758,y:32987,varname:node_210,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-7237-R;n:type:ShaderForge.SFN_Multiply,id:1532,x:31413,y:33021,varname:node_1532,prsc:2|A-8725-OUT,B-9376-OUT;n:type:ShaderForge.SFN_Slider,id:9376,x:31053,y:32921,ptovrint:False,ptlb:DistortionScale,ptin:_DistortionScale,varname:node_9376,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Multiply,id:8032,x:31413,y:33194,varname:node_8032,prsc:2|A-4950-OUT,B-9376-OUT;n:type:ShaderForge.SFN_Clamp01,id:5871,x:32364,y:33238,varname:node_5871,prsc:2|IN-310-OUT;n:type:ShaderForge.SFN_Clamp01,id:5761,x:32364,y:33363,varname:node_5761,prsc:2|IN-4675-OUT;proporder:4430-7237-7434-9799-9376;pass:END;sub:END;*/

Shader "Shader Forge/postTest" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _NoiseU ("NoiseU", 2D) = "white" {}
        _NoiseV ("NoiseV", 2D) = "white" {}
        _DistortionIntensity ("DistortionIntensity", Range(0, 0.1)) = 0.01
        _DistortionScale ("DistortionScale", Range(0, 1)) = 0.5
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
            uniform sampler2D _NoiseU; uniform float4 _NoiseU_ST;
            uniform float _DistortionIntensity;
            uniform sampler2D _NoiseV; uniform float4 _NoiseV_ST;
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
                float node_497 = 0.0;
                float2 node_8032 = (float2(node_497,i.uv0.g)*_DistortionScale);
                float4 _NoiseV_var = tex2D(_NoiseV,TRANSFORM_TEX(node_8032, _NoiseV));
                float2 node_1532 = (float2(i.uv0.r,node_497)*_DistortionScale);
                float4 _NoiseU_var = tex2D(_NoiseU,TRANSFORM_TEX(node_1532, _NoiseU));
                float2 node_2908 = float2(saturate(((_DistortionIntensity*(_NoiseV_var.r*2.0+-1.0))+i.uv0.r)),saturate(((_DistortionIntensity*(_NoiseU_var.r*2.0+-1.0))+i.uv0.g)));
                float4 node_1672 = tex2D(_MainTex,TRANSFORM_TEX(node_2908, _MainTex));
                float3 emissive = node_1672.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
