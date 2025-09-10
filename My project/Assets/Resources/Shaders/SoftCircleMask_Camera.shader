Shader "UI/SoftCircleMask_Camera"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {} // 검은색 또는 반투명 검은색 이미지 스프라이트
        _Color ("Tint", Color) = (1,1,1,1) // 이미지 색상 (알파 포함)

        // 스크립트에서 넘겨받을 속성 (이제 '실제 스크린 픽셀' 또는 이에 비례하는 값)
        _CenterPos_ScreenXY ("Center Screen Position (px)", Vector) = (0,0,0,0) // 구멍의 중심점 (스크린 픽셀 단위)
        _HoleRadius ("Hole Radius (px)", Float) = 100 // 완전히 투명해지는 원의 반지름 (스크린 픽셀 단위)
        _FadeSmoothness ("Fade Smoothness (px)", Float) = 50 // 투명에서 불투명으로 바뀌는 그라데이션 영역의 폭 (스크린 픽셀 단위)

        // 스텐실 관련 속성은 부드러움을 위해 사용하지 않지만, 필요시 남겨둘 수 있습니다.
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            // 사용 중인 렌더 파이프라인에 따라 태그를 조정해야 합니다.
//            "RenderPipeline" = "BuiltinRenderPipeline" // Built-in 사용 시
            //"RenderPipeline" = "UniversalPipeline" // URP 사용 시
            //"RenderPipeline" = "HDRenderPipeline" // HDRP 사용 시
//            "LightMode" = "Universal2D" // 예시: URP 2D.
            
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        // Stencil { ... } // 스텐실 사용 시 주석 해제

        Cull Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "SOFT_MASK_PASS_CAMERA"
//            Tags
//            {
//                "LightMode" = "Universal2D"
//            } // 예시: URP 2D

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // 사용 중인 렌더 파이프라인에 맞는 include 파일 사용
             #include "UnityCG.cginc" // Built-in 렌더 파이프라인
             // #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" // URP

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // 클립 공간 위치
                float2 uv : TEXCOORD0; // RectTransform UV (0-1)
                float4 color : COLOR; // 정점 색상 (UI 요소 색상/알파 포함)
                float4 screenPos : TEXCOORD1; // 스크린 공간 위치 (픽셀 단위 거리 계산용)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            // 스크립트에서 넘겨받을 속성 (실제 스크린 픽셀 단위)
            float4 _CenterPos_ScreenXY; // .xy에 구멍 중심의 스크린 픽셀 좌표 저장
            float _HoleRadius; // 완전히 투명해지는 원의 반지름 (스크린 픽셀 단위)
            float _FadeSmoothness; // 투명에서 불투명으로 바뀌는 그라데이션 영역의 폭 (스크린 픽셀 단위)

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color * _Color;

                // 현재 정점의 스크린 공간 위치 계산 및 전달
                // ComputeScreenPos 결과는 float4이며, xy는 화면 좌표(투영 전), w는 깊이 관련 값입니다.
                // Screen Space - Camera 모드에서 정확한 픽셀 좌표를 얻기 위해 필요합니다.
                o.screenPos = ComputeScreenPos(o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                // 현재 픽셀의 실제 스크린 공간 위치 (픽셀 단위) 계산
                // i.screenPos를 사용하여 원근 보정 및 화면 해상도 적용
                float2 pixelScreenPos = i.screenPos.xy / i.screenPos.w;
                pixelScreenPos = pixelScreenPos * _ScreenParams.xy / 2.0 + _ScreenParams.xy / 2.0;

                // 픽셀 위치와 스크립트에서 전달받은 구멍 중심점 (_CenterPos_ScreenXY.xy) 간의 거리 계산 (픽셀 단위)
                float dist = distance(pixelScreenPos, _CenterPos_ScreenXY.xy);

                // 거리를 기준으로 알파 값을 계산하여 그라데이션 생성
                // smoothstep(_HoleRadius, _HoleRadius + _FadeSmoothness, dist)
                // 거리가 _HoleRadius 이하일 때 0 (투명), _HoleRadius + _FadeSmoothness 이상일 때 1 (불투명)
                float gradientAlpha = smoothstep(_HoleRadius, _HoleRadius + _FadeSmoothness, dist);

                // 원래 이미지 색상(col.a)과 계산된 그라데이션 알파 값을 곱하여 최종 알파 결정
                col.a *= gradientAlpha;

                return col;
            }
            ENDCG
        }
    }
    // Fallback "UI/Default"
}
