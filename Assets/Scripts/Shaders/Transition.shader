Shader "Taiga74164/TransitionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Transition Mask (A)", 2D) = "white" {}
        _Cutoff ("Cutoff", Range(0, 1)) = 0.5
        _FadeColor ("Fade Color", Color) = (0,0,0,1)
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolveBorderWidth ("Dissolve Border Width", Range(0, 1)) = 0.1
        _DissolveBorderColor ("Dissolve Border Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            float4 _FadeColor;
            float _Cutoff;
            float _DissolveAmount;
            float _DissolveBorderWidth;
            float4 _DissolveBorderColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 mask = tex2D(_MaskTex, i.uv);
                
                // Apply cutoff.
                clip(mask.a - _Cutoff);

                // Apply fade color overlay based on cutoff.
                float alpha = smoothstep(_Cutoff - 0.1, _Cutoff, mask.a);
                texColor = lerp(texColor, _FadeColor, alpha);

                // Dissolve effect.
                if(_DissolveAmount > 0)
                {
                    float edge = fwidth(mask.a);
                    float border = smoothstep(_DissolveAmount - _DissolveBorderWidth * edge, _DissolveAmount, mask.a);
                    float dissolve = smoothstep(_DissolveAmount, _DissolveAmount + _DissolveBorderWidth * edge, mask.a);
                    texColor.rgb = lerp(_DissolveBorderColor.rgb, texColor.rgb, dissolve);
                    texColor.a *= border;
                }

                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}