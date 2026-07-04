Shader "Unlit/Chain"{
    Properties{
        _MainTex("Texture", 2D)="white"{}
    }
    SubShader{
        Tags{
            "RenderType"="Transparent"
            "Queue" = "Transparent"
        }
        LOD 100

        Pass{
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //应用在quad上
            //quad在y方向上拉伸一下得到链条的mesh

            //按照我这个搞法最后出来的结果，贴图里面的上方对应的是从钩爪指向玩家的方向

            struct appdata{
                float4 posOS: POSITION;
            };

            struct v2f{
                float4 posCS: SV_POSITION;
                float4 posOS: TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v){
                v2f o;
                o.posCS=UnityObjectToClipPos(v.posOS);
                o.posOS=v.posOS;
                return o;
            }

            fixed4 frag(v2f i): SV_Target{//
                //最后这个东西相当于拉伸后的quad的尺寸是width*height,width<height,
                //拿width*width尺寸的材质平铺
                //因为是quad所以我默认ObjectSpace下xy范围都是[-0.5,0.5]

                //然后我知道效率低下，先不管了
                float4 posOS=i.posOS;
                float width=distance(mul(unity_ObjectToWorld,float4(-0.5,0,0,0)).xyz,mul(unity_ObjectToWorld,float4(0.5,0,0,0)).xyz);
                float w=distance(mul(unity_ObjectToWorld,float4(-0.5,posOS.y,0,0)).xyz,mul(unity_ObjectToWorld,float4(posOS.x,posOS.y,0,0)).xyz);
                //float height=distance(mul(unity_ObjectToWorld,float4(0,-0.5,0,0)).xyz,mul(unity_ObjectToWorld,float4(0,0.5,0,0)).xyz);
                float h=distance(mul(unity_ObjectToWorld,float4(posOS.x,-0.5,0,0)).xyz,mul(unity_ObjectToWorld,float4(posOS.x,posOS.y,0,0)).xyz);
                float2 uv=float2(
                    w/width,
                    frac(h/width)
                );
                return tex2D(_MainTex,uv);
            }
            ENDCG
        }
    }
}
