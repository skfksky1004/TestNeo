Shader "UI/InverseMaskShader"
{
    Properties
    {
        // 표준 UI 셰이더 속성
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        // 마스크 컴포넌트에서 자동으로 설정되는 스텐실 관련 속성
        // Mask 컴포넌트가 설정하는 _StencilComp 값 8은 NotEqual에 해당합니다.
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
            // 사용하는 렌더 파이프라인에 따라 태그를 조정해야 합니다.
            "RenderPipeline" = "BuiltinRenderPipeline" // Built-in 사용 시
            //"RenderPipeline" = "UniversalPipeline" // URP 사용 시
            //"RenderPipeline" = "HDRenderPipeline" // HDRP 사용 시
            "LightMode" = "Universal2D" // 예시: URP 2D. 3D는 "UniversalForward" 등
        }

        // 핵심: 스텐실 설정
        Stencil
        {
            Ref [_Stencil] // 마스크 컴포넌트가 스텐실 버퍼에 쓰는 참조 값 (보통 1)
            Comp [_StencilComp] // 스텐실 버퍼 값과 참조 값 비교 -> NotEqual (8) 사용
            Pass Keep // 스텐실 테스트 통과 시 스텐실 값 유지
            Fail Keep // 스텐실 테스트 실패 시 스텐실 값 유지
            ReadMask [_StencilReadMask] // 스텐실 버퍼 읽기 마스크
            WriteMask [_StencilWriteMask] // 스텐실 버퍼 쓰기 마스크 (이 셰이더는 쓰지 않지만 Mask 컴포넌트가 씀)
        }

        Cull Off // 컬링 사용 안 함
        ZWrite Off // Z 버퍼 쓰기 안 함
        ZTest Always // Z 테스트 항상 통과

        Blend SrcAlpha OneMinusSrcAlpha // 표준 알파 블렌딩

        Pass
        {
            Name "INVERSE_MASK_PASS"
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
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _ColorMask;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                col *= _ColorMask;

                // 스텐실 테스트는 프래그먼트 셰이더 실행 전에 이미 완료됩니다.
                // Comp NotEqual 설정 때문에 마스크 영역 (스텐실 값 == Ref 값)의 픽셀은
                // 이 frag 함수까지 도달하지 못하고 버려집니다.
                // 마스크 영역 밖 (스텐실 값 != Ref 값)의 픽셀만 이곳에서 색상이 계산되어 그려집니다.

                return col;
            }
            ENDCG
        }
    }
    // Fallback "UI/Default" // 셰이더 로드 실패 시 사용될 대체 셰이더 (선택 사항)
}
