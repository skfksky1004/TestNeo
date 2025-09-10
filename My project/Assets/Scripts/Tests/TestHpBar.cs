using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하는 경우

public class TestHpBar : MonoBehaviour
{
    [SerializeField] private Image fillBarImage; // Inspector에서 FillBarImage를 연결
    [SerializeField] private Image[] segmentImages; // Inspector에서 Segment Image 배열을 연결 (선택 사항)

    public float maxHealth;
    public  float currentHealth;

    /// <summary>
    /// 보스 체력 바를 초기화합니다.
    /// </summary>
    /// <param name="_maxHealth">보스의 최대 체력</param>
    /// <param name="_bossName">보스의 이름 (선택 사항)</param>
    public void Initialize(float _maxHealth, string _bossName = null)
    {
        maxHealth = _maxHealth;
        currentHealth = _maxHealth; // 처음에는 최대 체력으로 설정

        // if (bossNameText != null && _bossName != null)
        // {
        //     bossNameText.text = _bossName;
        // }

        UpdateHealthBarUI();
    }

    /// <summary>
    /// 보스의 현재 체력을 업데이트하고 UI에 반영합니다.
    /// </summary>
    /// <param name="_currentHealth">새로운 현재 체력</param>
    public void UpdateHealth(float _currentHealth)
    {
        currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth); // 체력이 0보다 작거나 최대 체력보다 커지지 않도록 제한
        UpdateHealthBarUI();
    }

    /// <summary>
    /// 체력 바 UI 요소들을 실제 체력 값에 맞춰 업데이트합니다.
    /// </summary>
    private void UpdateHealthBarUI()
    {
        if (fillBarImage != null)
        {
            // Fill Amount 계산: 0에서 1 사이의 값
            fillBarImage.fillAmount = currentHealth / maxHealth;
        }

        // if (healthValueText != null)
        // {
        //     // 체력 값 텍스트 업데이트 (예: 10000 / 10000)
        //     healthValueText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        //     // 또는 단순히 현재 체력 퍼센트 표시 등 원하는 형식으로 변경
        //     // healthValueText.text = $"{Mathf.CeilToInt((currentHealth / maxHealth) * 100)}%";
        // }

        // 체력 구간(마디) 이미지 업데이트 로직 (선택 사항)
        // 던파 스타일의 마디는 전체 체력의 일정 % 구간마다 이미지를 활성화/비활성화하거나 색을 변경합니다.
        // 예: 전체 체력이 80% 이하가 되면 첫 번째 마디 이미지가 사라지거나 색이 변함
        // 이 부분은 구현 방식에 따라 복잡도가 달라집니다.
        if (segmentImages != null && segmentImages.Length > 0)
        {
            float healthRatio = currentHealth / maxHealth;
            // 예시: 20% 구간마다 마디가 사라진다고 가정
            for (int i = 0; i < segmentImages.Length; i++)
            {
                // segmentImages[i].gameObject.SetActive(healthRatio > (float)(segmentImages.Length - i -1) / segmentImages.Length * 0.2f); // 20% 구간 예시
                // 실제 구현은 던파의 로직에 맞게 조정해야 합니다.
            }
        }
    }

    // 예시 사용법:
    // 보스 스크립트나 체력 관리 스크립트에서 이 BossHealthBar 스크립트의 UpdateHealth 메서드를 호출합니다.
    // 예를 들어, 데미지를 입었을 때:
    // bossHealthBarScript.UpdateHealth(currentBossHealth - damageAmount);

    // 시작 시 예시 초기화 (테스트용)
    void Start()
    {
        Initialize(1000, "대마법사"); // 최대 체력 1000, 이름 설정
        InvokeRepeating("TakeDamageExample", 1f, 1f); // 1초마다 데미지 입는 예시
    }

    void TakeDamageExample()
    {
        UpdateHealth(currentHealth - Random.Range(50, 150));
        if (currentHealth <= 0)
        {
            CancelInvoke("TakeDamageExample"); // 체력이 0이 되면 데미지 중지
            Debug.Log("보스 처치!");
        }
    }
}
