Shader "Custom/RunePulse"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "CanUseSpriteAtlas" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"

            struct Attributes
            {
                COMMON_2D_INPUTS
                half4 color : COLOR;
            };

            struct Varyings
            {
                COMMON_2D_OUTPUTS
                half4 color : COLOR;
            };

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/2DCommon.hlsl"

            Varyings Vert(Attributes input)
            {
                input.positionOS = UnityFlipSprite(input.positionOS, unity_SpriteProps.xy);
                Varyings output = CommonUnlitVertex(input);
                output.color = input.color * unity_SpriteColor;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                half4 color = CommonUnlitFragment(input, input.color);

                // Unity supplies time in seconds. One gentle pulse every three seconds.
                half pulse = 0.5 - 0.5 * cos(_Time.y * 2.0943951);
                // Keep the texture's brightness detail while shifting toward bright cyan-blue.
                half brightness = max(color.r, max(color.g, color.b));
                half3 cyan = brightness * half3(0.4, 1.4, 1.8);
                color.rgb = lerp(color.rgb, cyan, pulse);
                return color; // Original sprite alpha is unchanged.
            }
            ENDHLSL
        }
    }
}
