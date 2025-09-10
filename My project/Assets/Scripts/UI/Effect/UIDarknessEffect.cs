using System;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIDarknessEffect : MonoBehaviour
{
    [Header("Soft Hole Settings (Pixels)")]
    [SerializeField] private float holeRadius = 100f; // 완전히 투명해지는 원의 반지름 (스크린 픽셀 단위)
    [SerializeField] private float fadeSmoothness = 50f; // 투명에서 불투명으로 바뀌는 그라데이션 영역 폭 (스크린 픽셀 단위)

    public Vector2 addCenterPos = Vector2.zero;
    
    private Transform _mainPlayerTransform;
    private Camera _uiCamera;
    private Image _overlayImage;

    private Material _overlayMaterial; // Image 컴포넌트의 Material 인스턴스

    private float InversionDistanceValue = 0f;

    private Vector2 _saveTempResolution = Vector2.zero;

    private Transform MainPlayerTf = null;//> PlayerManager.instance.GetMainPlayer().transform;
    private Transform MainCameraTf = null;//> GameCamera.instance.MainCameraTransform;
    
    private void Awake()
    {
        if (_overlayImage == null)
            _overlayImage = GetComponent<Image>();

        // if (_uiCamera == null)
        //     _uiCamera = UIManager.instance.UICamera;

        if (_overlayImage == null || _uiCamera == null)
        {
            enabled = false;
            return;
        }
        
        _overlayMaterial = _overlayImage.material;
    }

    private void OnEnable()
    {
        RefreshResolution();
    }

    private void LateUpdate()
    {
        if (_saveTempResolution != GetResolution())
        {
            RefreshResolution();
            _saveTempResolution = GetResolution();
        }
        
        if (_overlayMaterial != null)
        {
            //  플레이어와 카메라의 거리에 따라서
            //  블라인드 원 사이즈를 변경
            var charPos = MainPlayerTf.position;
            var camPos = MainCameraTf.position;
            var value = -((charPos - camPos).sqrMagnitude * 0.1f);

            if (InversionDistanceValue != value)
            {
                _overlayMaterial.SetFloat("_HoleRadius", holeRadius + value);
                InversionDistanceValue = value;
            }
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

    private void RefreshResolution()
    {
        // 필요한 요소들이 모두 유효한 경우에만 업데이트
        if (_overlayImage != null && _uiCamera != null && _overlayMaterial != null)
        {
            var mainPlayer = new GameObject();//PlayerManager.instance.GetMainPlayer();
            if (mainPlayer == null)
                return;
            
            // 계산된 스크린 좌표, 구멍 반지름, 페이드 영역 폭을 셰이더 마테리얼 속성으로 전달합니다.
            // 셰이더는 _CenterPos_ScreenXY의 .xy 값만 사용합니다.
            _overlayMaterial?.SetVector("_CenterPos_ScreenXY", GetResolution());
            //  최초 원사이즈
            _overlayMaterial?.SetFloat("_HoleRadius", holeRadius + InversionDistanceValue);
            //  원 알파 테두리 처리
            _overlayMaterial?.SetFloat("_FadeSmoothness", fadeSmoothness);
        }
    }
    
    //  해상도별 센터값
    private Vector2 GetResolution()
    {
        float x = 0, y = 0, addY = 0;

        // if (PlatformUtil.IsMobile()) 
        // {
        //     var resolution = Screen.currentResolution;
        //     x = resolution.width * 0.25f; // / 4;
        //     y = resolution.height * 0.25f; // / 4;
        //     addY = -(resolution.height * 0.02f); // / 10; 
        // }
        // else if (PlatformUtil.IsPC() || PlatformUtil.IsSteam())
        // {
        //     var screenIndex = Option.OptionDatabase.instance.OptionData.localGraphicOption?.screenSizeIndex.Value ?? 0;
        //     var resolution = Util.PcScreenSizeList[screenIndex]; 
        //     x = resolution.Item1 * 0.25f; // / 4;
        //     y = resolution.Item2 * 0.25f; // / 4;
        //     addY = -(resolution.Item2 * 0.02f); // / 10; 
        // }

        return new Vector2(x, y + addY) + addCenterPos;
    }
}
