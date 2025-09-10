using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 사용하기 위해 필요

public class SoftCircleMaskController : MonoBehaviour
{
    // Inspector에서 연결할 요소
    [SerializeField] private Transform characterTransform; // 마스크 중심을 따라갈 캐릭터의 Transform
    [SerializeField] private Camera mainCamera; // 캐릭터를 렌더링하는 메인 카메라
    [SerializeField] private Image overlayImage; // SoftCircleMask 셰이더 마테리얼이 적용된 UI Image

    [Header("Soft Hole Settings (Pixels)")]
    [SerializeField] private float holeRadius = 100f; // 완전히 투명해지는 원의 반지름 (스크린 픽셀 단위)
    [SerializeField] private float fadeSmoothness = 50f; // 투명에서 불투명으로 바뀌는 그라데이션 영역 폭 (스크린 픽셀 단위)

    private Material overlayMaterial; // Image 컴포넌트의 Material 인스턴스

    void Awake()
    {
        // overlayImage가 Inspector에서 연결되지 않았다면, 같은 GameObject에서 Image 컴포넌트를 가져옵니다.
        if (overlayImage == null)
        {
            overlayImage = GetComponent<Image>();
            if (overlayImage == null)
            {
                Debug.LogError("Overlay Image component not found on this GameObject.");
                enabled = false; // Image 컴포넌트가 없으면 스크립트 비활성화
                return;
            }
        }

        // Image 컴포넌트의 Material 속성에 접근하여 마테리얼 인스턴스를 가져옵니다.
        // .material 속성을 사용하면 해당 Image 컴포넌트만을 위한 새로운 마테리얼 인스턴스가 생성되거나 기존 인스턴스가 반환됩니다.
        // .sharedMaterial을 사용하면 프로젝트의 원본 마테리얼을 직접 수정하게 되어 다른 오브젝트에 영향을 줄 수 있으므로 주의!
        overlayMaterial = overlayImage.material;

        // 메인 카메라가 연결되지 않았다면 Camera.main을 찾아서 사용
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("메인 카메라를 찾을 수 없습니다. CharacterFollowMask 스크립트의 Camera 필드를 연결해 주세요.");
                enabled = false;
                return;
            }
        }

        // 캐릭터 Transform이 연결되지 않았다면 경고
        if (characterTransform == null)
        {
             Debug.LogError("Character Transform not assigned. Please assign the character.");
             enabled = false;
             return;
        }

         if (overlayMaterial == null)
        {
             Debug.LogError("Overlay Material not found on the Image component. Make sure the SoftCircleMask shader is applied to the Image's Material field.");
             enabled = false;
             return;
        }
    }

    void Update()
    {
        // 필요한 요소들이 모두 유효한 경우에만 업데이트
        if (characterTransform != null && mainCamera != null && overlayMaterial != null)
        {
            // 캐릭터의 월드 위치를 가져옵니다.
            Vector3 characterWorldPosition = characterTransform.position;

            // 캐릭터의 월드 위치를 화면상의 스크린 좌표(픽셀 단위)로 변환합니다.
            Vector3 characterScreenPosition = mainCamera.WorldToScreenPoint(characterWorldPosition);

            // 계산된 스크린 좌표, 구멍 반지름, 페이드 영역 폭을 셰이더 마테리얼 속성으로 전달합니다.
            // 셰이더는 _CenterPos_ScreenXY의 .xy 값만 사용합니다.
            overlayMaterial.SetVector("_CenterPos_ScreenXY", characterScreenPosition);
            overlayMaterial.SetFloat("_HoleRadius", holeRadius);
            overlayMaterial.SetFloat("_FadeSmoothness", fadeSmoothness);
        }
    }

    // 선택 사항: 에디터에서 구멍 반지름을 시각적으로 디버깅
    // void OnDrawGizmosSelected()
    // {
    //     if (characterTransform != null && mainCamera != null)
    //     {
    //         // 캐릭터의 스크린 위치 계산
    //         Vector3 screenPos = mainCamera.WorldToScreenPoint(characterTransform.position);
    //
    //         // 스크린 좌표를 다시 월드 좌표로 변환하여 Gizmos로 표시
    //         // 주의: Z 값에 따라 크기가 달라질 수 있습니다.
    //         Vector3 worldCenter = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, mainCamera.nearClipPlane + 1f)); // 카메라 앞 적절한 Z
    //         float worldRadius = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x + holeRadius, screenPos.y, mainCamera.nearClipPlane + 1f)).x - worldCenter.x;
    //
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawWireSphere(worldCenter, worldRadius);
    //
    //          Gizmos.color = Color.red;
    //          float worldOuterRadius = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x + holeRadius + fadeSmoothness, screenPos.y, mainCamera.nearClipPlane + 1f)).x - worldCenter.x;
    //          Gizmos.DrawWireSphere(worldCenter, worldOuterRadius);
    //     }
    // }
}
