Shader "Custom/Blend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        [Space(10)]
        _DisValue("Distortion Value", Range(2, 10)) = 3
        _DisSpeed("Distortion Speed", Range(-0.4, 0.4)) = 0.1
        [Space(10)]
        _WaveSpeed("Wave Speed", Range(0, 5)) = 1
        _WaveFrequency("Wave Frequency", Range(0, 5)) = 1
        _WaveAmplitude("Wave Amplitude", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque"
            "Queue"= "Transparent"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        BlendOp Add

        Cull Off

        Pass
        {
            ZTest LEqual
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
            float4 _MainTex_ST;
            float4 _Color;

            float _DisValue;
            float _DisSpeed;

            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);



                //o.vertex.x += sin((-worldPos.z + (_Time.x * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;
                //o.vertex.x += cos((-worldPos.y + (_Time.x * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;
                //o.vertex.z += sin((-worldPos.x + (_Time.y * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;
                //o.vertex.z += cos((-worldPos.y + (_Time.y * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;
                //o.vertex.x += sin((-worldPos.x + (_Time.y * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;
                //o.vertex.x += cos((-worldPos.x + (_Time.y * _WaveSpeed)) * _WaveFrequency) * _WaveAmplitude;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                /*half distortion = tex2D(_DisTex, i.uv + (_Time * _DisSpeed)).r;
                i.uv.x += distortion / _DisValue;
                i.uv.y += distortion / _DisValue;*/
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                return col;
            }
            ENDCG
        }
    }
}
