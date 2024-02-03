Shader "SoFunny/TNT/TNTTEST"
{
    Properties
    {
        _Test ("Test Value", Range(0.0, 1.0)) = 0.5
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 300
        Pass
        {
            Name "TNTTest"
            //Tags { "LightMode" = "FunnyLandMobileForward" }
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            #pragma multi_compile _APPLE _BANANA
            #pragma multi_compile _CAR _DAD
            #pragma multi_compile _EGG _FUCK
            #pragma multi_compile _GIRL _HI
            #pragma multi_compile _ILL _JACK

            #ifndef HAVE_VFX_MODIFICATION
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #if UNITY_PLATFORM_ANDROID || UNITY_PLATFORM_WEBGL || UNITY_PLATFORM_UWP
                    #pragma target 3.5 DOTS_INSTANCING_ON
                #else
                    #pragma target 4.5 DOTS_INSTANCING_ON
                #endif
            #endif // HAVE_VFX_MODIFICATION

            #pragma vertex vert
            #pragma fragment frag


            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes v)
            {
                Varyings o = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

                VertexPositionInputs vpi = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = vpi.positionCS;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {

                UNITY_SETUP_INSTANCE_ID(i);
                half4 color = half4(1, 1, 0, 0.1);
                return color;
                #ifdef _APPLE
                    color = half4(1, 0, 0, 0);
                #endif

                #ifdef _BANANA
                    color = half4(0, 1, 0, 0);
                #endif

                #ifdef _CAR
                    color = half4(0, 0, 1, 0);
                #endif

                #ifdef _DAD
                    color = half4(0, 0, 0, 0);
                #endif


                return color;
            }
            ENDHLSL
        }
    }
}