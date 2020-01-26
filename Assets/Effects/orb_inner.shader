// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:33159,y:32656,varname:node_4013,prsc:2|emission-1304-RGB,alpha-3853-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32403,y:32752,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_TexCoord,id:9470,x:32124,y:33058,varname:node_9470,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:9844,x:32503,y:32953,ptovrint:False,ptlb:node_9844,ptin:_node_9844,varname:node_9844,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-5514-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:9158,x:32676,y:32881,varname:node_9158,prsc:2,cc1:0,cc2:-1,cc3:-1,cc4:-1|IN-9844-RGB;n:type:ShaderForge.SFN_Panner,id:5514,x:32331,y:32953,varname:node_5514,prsc:2,spu:0.01,spv:0.01|UVIN-9470-UVOUT;n:type:ShaderForge.SFN_Time,id:1505,x:31977,y:32898,varname:node_1505,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7546,x:32491,y:33232,ptovrint:False,ptlb:node_7546,ptin:_node_7546,varname:node_7546,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Fresnel,id:1855,x:32750,y:33077,varname:node_1855,prsc:2|EXP-7546-OUT;n:type:ShaderForge.SFN_Multiply,id:3853,x:32940,y:32915,varname:node_3853,prsc:2|A-9158-OUT,B-1855-OUT;proporder:1304-9844-7546;pass:END;sub:END;*/

Shader "Shader Forge/orb_inner" {
    Properties {
        _Color ("Color", Color) = (0,1,1,1)
        _node_9844 ("node_9844", 2D) = "white" {}
        _node_7546 ("node_7546", Float ) = 2
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _node_9844; uniform float4 _node_9844_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP( float, _node_7546)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float3 emissive = _Color_var.rgb;
                float3 finalColor = emissive;
                float4 node_2336 = _Time;
                float2 node_5514 = (i.uv0+node_2336.g*float2(0.01,0.01));
                float4 _node_9844_var = tex2D(_node_9844,TRANSFORM_TEX(node_5514, _node_9844));
                float _node_7546_var = UNITY_ACCESS_INSTANCED_PROP( Props, _node_7546 );
                fixed4 finalRGBA = fixed4(finalColor,(_node_9844_var.rgb.r*pow(1.0-max(0,dot(normalDirection, viewDirection)),_node_7546_var)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
