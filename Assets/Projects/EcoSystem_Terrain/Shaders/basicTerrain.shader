// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32718,y:32611,varname:node_3138,prsc:2|diff-9395-OUT,spec-6979-OUT,gloss-8111-OUT;n:type:ShaderForge.SFN_VertexColor,id:3806,x:31645,y:32472,varname:node_3806,prsc:2;n:type:ShaderForge.SFN_Slider,id:6979,x:32326,y:32760,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:_Specular,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:8111,x:32326,y:32904,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Gloss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_SwitchProperty,id:7191,x:31912,y:32112,ptovrint:False,ptlb:DisplayR,ptin:_DisplayR,varname:_DisplayR,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-8941-OUT,B-3806-R;n:type:ShaderForge.SFN_Vector1,id:8941,x:31645,y:32349,varname:node_8941,prsc:2,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:785,x:31912,y:32327,ptovrint:False,ptlb:DisplayG,ptin:_DisplayG,varname:_DisplayG,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-8941-OUT,B-3806-G;n:type:ShaderForge.SFN_SwitchProperty,id:5068,x:31912,y:32531,ptovrint:False,ptlb:DisplayB,ptin:_DisplayB,varname:_DisplayB,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-8941-OUT,B-3806-B;n:type:ShaderForge.SFN_Append,id:8378,x:32114,y:32337,varname:node_8378,prsc:2|A-7191-OUT,B-785-OUT,C-5068-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:9395,x:32483,y:32496,ptovrint:False,ptlb:DisplayA,ptin:_DisplayA,varname:_DisplayA,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-8378-OUT,B-2753-OUT;n:type:ShaderForge.SFN_Multiply,id:859,x:32133,y:32573,varname:node_859,prsc:2|A-3806-A,B-8378-OUT;n:type:ShaderForge.SFN_Frac,id:5464,x:31146,y:32883,varname:node_5464,prsc:2|IN-8139-OUT;n:type:ShaderForge.SFN_Step,id:8020,x:31320,y:32945,varname:node_8020,prsc:2|A-5464-OUT,B-7681-OUT;n:type:ShaderForge.SFN_Vector1,id:7681,x:31146,y:33019,varname:node_7681,prsc:2,v1:0.5;n:type:ShaderForge.SFN_ComponentMask,id:8549,x:31488,y:32945,varname:node_8549,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8020-OUT;n:type:ShaderForge.SFN_Multiply,id:9863,x:31665,y:32883,varname:node_9863,prsc:2|A-8549-R,B-8549-G,C-6121-OUT;n:type:ShaderForge.SFN_Add,id:2793,x:31709,y:33039,varname:node_2793,prsc:2|A-8549-R,B-8549-G;n:type:ShaderForge.SFN_Subtract,id:2147,x:31893,y:33039,varname:node_2147,prsc:2|A-2793-OUT,B-9863-OUT;n:type:ShaderForge.SFN_Vector1,id:6121,x:31488,y:32820,varname:node_6121,prsc:2,v1:2;n:type:ShaderForge.SFN_OneMinus,id:6808,x:31832,y:32726,varname:node_6808,prsc:2|IN-3806-A;n:type:ShaderForge.SFN_Multiply,id:7138,x:32089,y:32845,varname:node_7138,prsc:2|A-6808-OUT,B-2147-OUT;n:type:ShaderForge.SFN_Add,id:2753,x:32314,y:32573,varname:node_2753,prsc:2|A-859-OUT,B-7138-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:9126,x:30628,y:32680,varname:node_9126,prsc:2;n:type:ShaderForge.SFN_Append,id:3467,x:30820,y:32739,varname:node_3467,prsc:2|A-9126-X,B-9126-Z;n:type:ShaderForge.SFN_Divide,id:8139,x:30974,y:32859,varname:node_8139,prsc:2|A-3467-OUT,B-6736-OUT;n:type:ShaderForge.SFN_Vector1,id:6736,x:30820,y:32936,varname:node_6736,prsc:2,v1:10;proporder:6979-8111-7191-785-5068-9395;pass:END;sub:END;*/

Shader "Shader Forge/basicTerrain" {
    Properties {
        _Specular ("Specular", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0
        [MaterialToggle] _DisplayR ("DisplayR", Float ) = 0
        [MaterialToggle] _DisplayG ("DisplayG", Float ) = 0
        [MaterialToggle] _DisplayB ("DisplayB", Float ) = 0
        [MaterialToggle] _DisplayA ("DisplayA", Float ) = 0
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
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Specular;
            uniform float _Gloss;
            uniform fixed _DisplayR;
            uniform fixed _DisplayG;
            uniform fixed _DisplayB;
            uniform fixed _DisplayA;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(2,3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = float3(_Specular,_Specular,_Specular);
                float specularMonochrome;
                float node_8941 = 0.0;
                float3 node_8378 = float3(lerp( node_8941, i.vertexColor.r, _DisplayR ),lerp( node_8941, i.vertexColor.g, _DisplayG ),lerp( node_8941, i.vertexColor.b, _DisplayB ));
                float2 node_3467 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_8549 = step(frac((node_3467/10.0)),0.5).rg;
                float node_6121 = 2.0;
                float node_9863 = (node_8549.r*node_8549.g*node_6121);
                float3 diffuseColor = lerp( node_8378, ((i.vertexColor.a*node_8378)+((1.0 - i.vertexColor.a)*((node_8549.r+node_8549.g)-node_9863))), _DisplayA ); // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Specular;
            uniform float _Gloss;
            uniform fixed _DisplayR;
            uniform fixed _DisplayG;
            uniform fixed _DisplayB;
            uniform fixed _DisplayA;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(2,3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = float3(_Specular,_Specular,_Specular);
                float specularMonochrome;
                float node_8941 = 0.0;
                float3 node_8378 = float3(lerp( node_8941, i.vertexColor.r, _DisplayR ),lerp( node_8941, i.vertexColor.g, _DisplayG ),lerp( node_8941, i.vertexColor.b, _DisplayB ));
                float2 node_3467 = float2(i.posWorld.r,i.posWorld.b);
                float2 node_8549 = step(frac((node_3467/10.0)),0.5).rg;
                float node_6121 = 2.0;
                float node_9863 = (node_8549.r*node_8549.g*node_6121);
                float3 diffuseColor = lerp( node_8378, ((i.vertexColor.a*node_8378)+((1.0 - i.vertexColor.a)*((node_8549.r+node_8549.g)-node_9863))), _DisplayA ); // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
