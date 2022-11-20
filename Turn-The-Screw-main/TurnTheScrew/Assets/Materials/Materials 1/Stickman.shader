Shader "Gota/Stickman"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _TiredTex("Tired Texture", 2D) = "white" {}
        _TiredCol("Tired Color",Color) = (1,1,1,1)
        _TiredRange("Tired Range", Range(0,1)) = 0
        _TiredSmooth("Tired Smooth", Range(0,1)) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _TiredTex;
        fixed4 _TiredCol;
        half _TiredRange;
        half _TiredSmooth;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Texturelerin kullanýlabilir hale getiren standart fonksiyonlar.
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            fixed4 tired = tex2D(_TiredTex, IN.uv_MainTex);

            // Tired Smooth Deðerini ayarlamak için kullandýðým fonsiyon. Eðer Range deðeri Smooth deðerinden yüksekse herhangi bir deðiþiklik yok, fakat eðer düþükse bu sefer Smooth deðerini Range deðerine göre arttýrýp azaltýyor.
            half smooth = _TiredRange > _TiredSmooth ? _TiredSmooth : lerp(0,_TiredSmooth,_TiredRange * (1/_TiredSmooth));

            // Son rengi ayarlayan if statementler. Ýlk kýsýmda Range yeterince büyükse direkt renk veriyor. Fakat eðer Smooth deðeri kadar bir fark varsa bu sefer lerp ile o deðer aralýðýný yumuþatýyor. Daha küçükse bu sefer orjinal renk olacak þekilde render alýyor.
            o.Albedo = _TiredRange > 1 - tired.r ? _TiredCol :  _TiredRange > 1 - tired.r - smooth ? lerp(c, _TiredCol, ((1/ smooth * tired.r) - ((1 - (_TiredRange+ smooth))* (1/ smooth)))) : c;
            

            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
