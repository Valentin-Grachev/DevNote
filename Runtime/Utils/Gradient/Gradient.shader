Shader "UI/LinearGradient5Keys_MultiplyWithVertexColor"
{
    Properties
    {
        [PerRendererData]_MainTex("Sprite Texture", 2D) = "white" {}

        _Color0("Color 0", Color) = (1,0,0,1)
        _Pos0("Pos 0", Range(0,1)) = 0.0

        _Color1("Color 1", Color) = (0,1,0,1)
        _Pos1("Pos 1", Range(0,1)) = 0.25

        _Color2("Color 2", Color) = (0,0,1,1)
        _Pos2("Pos 2", Range(0,1)) = 0.5

        _Color3("Color 3", Color) = (1,1,0,1)
        _Pos3("Pos 3", Range(0,1)) = 0.75

        _Color4("Color 4", Color) = (1,0,1,1)
        _Pos4("Pos 4", Range(0,1)) = 1.0

        _Angle("Gradient Angle", Range(0,360)) = 0
        _ColorCount("Number of Colors", Range(1,5)) = 5
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriteAtlas"="True"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
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
                float4 color : COLOR; // цвет элемента
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // передаем в фрагмент
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color0,_Color1,_Color2,_Color3,_Color4;
            float _Pos0,_Pos1,_Pos2,_Pos3,_Pos4;
            float _Angle;
            float _ColorCount;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color; // передаем цвет элемента
                return o;
            }

            float4 SampleGradient(float t)
            {
                float positions[5] = {_Pos0,_Pos1,_Pos2,_Pos3,_Pos4};
                float4 colors[5] = {_Color0,_Color1,_Color2,_Color3,_Color4};

                int count = max(1, min(5, (int)_ColorCount));

                for (int i=0; i<count-1; i++)
                {
                    if (t >= positions[i] && t <= positions[i+1])
                    {
                        float u = (t - positions[i]) / max(1e-5, (positions[i+1]-positions[i]));
                        return lerp(colors[i], colors[i+1], saturate(u));
                    }
                }

                if (t < positions[0]) return colors[0];
                return colors[count-1];
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float rad = radians(_Angle);
                float2 dir = float2(cos(rad), sin(rad));
                float2 centeredUV = i.uv - 0.5;
                float t = dot(centeredUV, dir) + 0.5;

                fixed4 baseCol = tex2D(_MainTex, i.uv);
                fixed4 gradCol = SampleGradient(saturate(t));

                // умножаем на цвет пикселя и цвет самого UI/TMP
                gradCol.rgb *= baseCol.rgb * i.color.rgb;

                // сохраняем альфу
                gradCol.a *= baseCol.a * i.color.a;

                return gradCol;
            }
            ENDCG
        }
    }

    CustomEditor "LinearGradient5KeysGUI"
}






