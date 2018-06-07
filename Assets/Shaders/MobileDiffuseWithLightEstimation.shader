Shader "Portal/DiffuseWithLightEstimation"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Stencil("StencilNum",int)=3
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        LOD 150
        Stencil{
            Ref 1
            Comp [_Stencil]
        }
        CGPROGRAM
        #pragma surface surf Lambert noforwardadd finalcolor:lightEstimation

        sampler2D _MainTex;
        fixed3 _GlobalColorCorrection;

        struct Input
        {
            float2 uv_MainTex;
        };

        void lightEstimation(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            color.rgb *= _GlobalColorCorrection;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Mobile/VertexLit"
}
