Shader "UI/SoftCircleMask"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {} // 검은색 또는 반투명 검은색 이미지 스프라이트
        _Color ("Tint", Color) = (1,1,1,1) // 이미지 색상 (알파 포함)

        _CenterPos_ScreenXY ("Center Screen Position (px)", Vector) = (0,0,0,0) // 구멍의 중심점 (스크린 좌표 픽셀 단위)
        _HoleRadius ("Hole Radius (px)", Float) = 100 // 완전히 투명해지는 원의 반지름 (픽셀 단위)
        _FadeSmoothness ("Fade Smoothness (px)", Float) = 50 // 투명에서 불투명으로 바뀌는 그라데이션 영역의 폭 (픽셀 단위)

        // 스텐실 관련 속성은 이 셰이더 자체에서 부드러움을 위해 사용하지 않지만,
        // 필요에 따라 하드 클리핑을 위해 UI Mask 컴포넌트와 함께 사용할 수 있습니다.
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
            "RenderPipeline" = "BuiltinRenderPipeline" // Built-in 사용 시
            //"RenderPipeline" = "UniversalPipeline" // URP 사용 시
            //"RenderPipeline" = "HDRenderPipeline" // HDRP 사용 시
            "LightMode" = "Universal2D" // 예시: URP 2D. 3D는 "UniversalForward" 등
        }

        // 스텐실 설정은 여기 있지만, Comp가 NotEqual이 아닌 다른 값으로 기본 설정될 수 있으며,
        // 이 셰이더의 주된 알파 제어 로직은 스텐실 테스트 결과와는 독립적으로 Fragment 셰이더에서 계산됩니다.
        // Stencil { ... }

        Cull Off // 컬링 사용 안 함
        ZWrite Off // Z 버퍼 쓰기 안 함
        ZTest Always // Z 테스트 항상 통과

        Blend SrcAlpha OneMinusSrcAlpha // 표준 알파 블렌딩 (투명도 적용)

        Pass
        {
            Name "SOFT_MASK_PASS"
             // 사용하는 렌더 파이프라인에 맞는 LightMode 태그 필요
             Tags { "LightMode" = "Universal2D" } // 예시: URP 2D

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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // 클립 공간 위치 (렌더링 파이프라인에서 사용)
                float2 texcoord : TEXCOORD0; // 메인 텍스처 UV
                float4 color : COLOR; // 정점 색상 (UI 요소 색상/알파 포함)
                float4 screenPos : TEXCOORD1; // 스크린 공간 위치 (픽셀 단위 거리 계산용)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            // 스크립트에서 넘겨받을 속성
            float4 _CenterPos_ScreenXY; // .xy에 구멍 중심의 스크린 좌표(픽셀) 저장
            float _HoleRadius; // 완전히 투명해지는 내부 원 반지름 (픽셀)
            float _FadeSmoothness; // 페이드 영역 폭 (픽셀)

            v2f vert (appdata_t v)
            {
                v2f o;
                // 정점 위치를 클립 공간으로 변환
                o.vertex = UnityObjectToClipPos(v.vertex);
                // 텍스처 UV 변환 (Tiling, Offset 등)
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                // 정점 색상과 속성 색상 곱하기
                o.color = v.color * _Color;

                // 현재 정점의 스크린 공간 위치 계산 및 전달
                // ComputeScreenPos 결과는 float4이며, xy는 화면 좌표(투영 전), w는 깊이 관련 값입니다.
                o.screenPos = ComputeScreenPos(o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 메인 텍스처(검은 화면 이미지) 샘플링 및 색상 적용
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;

                // 현재 픽셀의 실제 스크린 공간 위치 (픽셀 단위) 계산
                // screenPos.w로 나누어 원근 보정 후, 화면 해상도 및 0.5 offset 적용
                float2 pixelScreenPos = i.screenPos.xy / i.screenPos.w;
                pixelScreenPos = pixelScreenPos * _ScreenParams.xy / 2.0 + _ScreenParams.xy / 2.0;

                // 픽셀 위치와 구멍 중심점(_CenterPos_ScreenXY.xy) 간의 거리 계산
                float dist = distance(pixelScreenPos, _CenterPos_ScreenXY.xy);

                // 거리를 기준으로 알파 값을 계산하여 그라데이션 생성
                // smoothstep(edge0, edge1, x)는 x가 edge0보다 작으면 0, edge1보다 크면 1, 그 사이는 부드럽게 보간
                // 우리는 dist가 _HoleRadius 이하일 때 알파 0, _HoleRadius + _FadeSmoothness 이상일 때 알파 1을 원합니다.
                // 즉, 구멍 내부(radius 이하)는 투명, 외부(radius + fade 이상)는 불투명
                float gradientAlpha = smoothstep(_HoleRadius, _HoleRadius + _FadeSmoothness, dist);

                // 원래 이미지 색상(col.a)과 계산된 그라데이션 알파 값을 곱하여 최종 알파 결정
                col.a *= gradientAlpha;

                return col;
            }
            ENDCG
        }
    }
}
