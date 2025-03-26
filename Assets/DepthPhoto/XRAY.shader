Shader "Unlit/XRayWindow_Unlit_Feather_Modified"
{
    Properties
    {
        _Color("Outline Color", Color) = (1,0,0,1)
        _Feather("Feather Width", Range(0.0, 0.5)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Geometry-1" }
        LOD 100

        // --- First Pass: Write Stencil ---
        Pass
        {
            Name "StencilPass"
            ColorMask 0
            ZWrite Off

            Stencil {
                Ref 1
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vertStencil
            #pragma fragment fragStencil
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                // If your mesh has proper UVs (range 0–1) use them:
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vertStencil(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Uncomment the following line if your mesh UVs are set up correctly:
                o.uv = v.uv;
                // Otherwise, generate UVs from clip space:
                // float2 clipPos = o.vertex.xy / o.vertex.w;
                // o.uv = clipPos * 0.5 + 0.5;
                return o;
            }

            fixed4 fragStencil(v2f i) : SV_Target
            {
                // Write the stencil without drawing any visible color.
                return fixed4(0,0,0,0);
            }
            ENDCG
        }

        // --- Second Pass: Feathered Outline ---
        Pass
        {
            Name "FeatherPass"
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            // Only draw where the stencil value equals 1.
            Stencil {
                Ref 1
                Comp Equal
            }

            CGPROGRAM
            #pragma vertex vertFeather
            #pragma fragment fragFeather
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float _Feather;

            v2f vertFeather(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // Again, if your mesh UVs are good, use:
                o.uv = v.uv;
                // Otherwise, generate UVs from clip space:
                // float2 clipPos = o.vertex.xy / o.vertex.w;
                // o.uv = clipPos * 0.5 + 0.5;
                return o;
            }

            fixed4 fragFeather(v2f i) : SV_Target
            {
                // Calculate the minimum distance from the UV coordinate to any edge.
                // This works best if the UVs map the window in the 0–1 range.
                float edgeDist = min(min(i.uv.x, 1.0 - i.uv.x),
                                    min(i.uv.y, 1.0 - i.uv.y));
                // Feather effect:
                // When edgeDist is 0 (right at the border) smoothstep returns 0,
                // so we get full outline opacity (1 - 0 = 1).
                // When edgeDist reaches _Feather, smoothstep returns 1 and the outline fades out.
                float featherAlpha = 1.0 - smoothstep(0.0, _Feather, edgeDist);

                return fixed4(_Color.rgb, _Color.a * featherAlpha);
            }
            ENDCG
        }
    }
}
