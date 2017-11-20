// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:False,qofs:0,qpre:1,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:1,stmr:0,stmw:1,stcp:6,stps:2,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-174-OUT;n:type:ShaderForge.SFN_TexCoord,id:6094,x:31100,y:33079,varname:node_6094,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:6018,x:31266,y:32992,varname:node_6018,prsc:2,spu:1,spv:1|UVIN-6094-UVOUT,DIST-4145-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8782,x:30928,y:32868,ptovrint:False,ptlb:BorderThickness,ptin:_BorderThickness,varname:node_8782,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Divide,id:4145,x:31100,y:32917,varname:node_4145,prsc:2|A-8782-OUT,B-9716-OUT;n:type:ShaderForge.SFN_Vector1,id:9716,x:30928,y:32977,varname:node_9716,prsc:2,v1:2;n:type:ShaderForge.SFN_Frac,id:384,x:31434,y:33058,varname:node_384,prsc:2|IN-6018-UVOUT;n:type:ShaderForge.SFN_Step,id:4033,x:31602,y:32928,varname:node_4033,prsc:2|A-384-OUT,B-8782-OUT;n:type:ShaderForge.SFN_ComponentMask,id:753,x:31772,y:32928,varname:node_753,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-4033-OUT;n:type:ShaderForge.SFN_Add,id:5132,x:31966,y:32928,varname:node_5132,prsc:2|A-753-R,B-753-G;n:type:ShaderForge.SFN_Clamp01,id:7580,x:32135,y:32928,varname:node_7580,prsc:2|IN-5132-OUT;n:type:ShaderForge.SFN_Tex2d,id:9745,x:32159,y:32544,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_9745,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9408-UVOUT;n:type:ShaderForge.SFN_ScreenPos,id:9408,x:31990,y:32544,varname:node_9408,prsc:2,sctp:2;n:type:ShaderForge.SFN_Multiply,id:2003,x:32337,y:32646,varname:node_2003,prsc:2|A-9745-RGB,B-5435-OUT;n:type:ShaderForge.SFN_OneMinus,id:5435,x:32159,y:32707,varname:node_5435,prsc:2|IN-7580-OUT;n:type:ShaderForge.SFN_Add,id:174,x:32525,y:32712,varname:node_174,prsc:2|A-2003-OUT,B-5354-OUT;n:type:ShaderForge.SFN_Color,id:6693,x:32135,y:33091,ptovrint:False,ptlb:BorderColor,ptin:_BorderColor,varname:node_6693,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:5354,x:32327,y:32968,varname:node_5354,prsc:2|A-7580-OUT,B-6693-RGB;proporder:8782-9745-6693;pass:END;sub:END;*/

Shader "Shader Forge/portal" {
    Properties {
        _BorderThickness ("BorderThickness", Float ) = 0.1
        _Texture ("Texture", 2D) = "white" {}
        _BorderColor ("BorderColor", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="TransparentCutout"
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
            uniform float _BorderThickness;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float4 _BorderColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
////// Lighting:
////// Emissive:
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(sceneUVs.rg, _Texture));
                float2 node_753 = step(frac((i.uv0+(_BorderThickness/2.0)*float2(1,1))),_BorderThickness).rg;
                float node_7580 = saturate((node_753.r+node_753.g));
                float3 emissive = ((_Texture_var.rgb*(1.0 - node_7580))+(node_7580*_BorderColor.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
