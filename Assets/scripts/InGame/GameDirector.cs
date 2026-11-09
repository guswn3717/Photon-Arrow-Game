using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private GameObject RetryButton;
    
    Image hpGauge = null;
    GameObject GameOverTXT = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpGauge = GameObject.Find("hpGauge").GetComponent<Image>();
        GameOverTXT = GameObject.Find("GameOverTXT");
        
        GameOverTXT.SetActive(false);
        RetryButton.SetActive(false);
    }
    
    // 개별 플레이어 체력에 맞춰 UI 게이지를 갱신해 주는 함수
    public void UpdateHpGauge(float hpRatio)
    {
        if (hpGauge != null)
        {
            hpGauge.fillAmount = hpRatio;
        }
    }

    // 게임 오버 연출 (내 체력이 0이 되었을 때 호출)
    public void TriggerGameOver()
    {
        Time.timeScale = 0f; // 게임 일시정지
        RetryButton.SetActive(true);
        GameOverTXT.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // 체력바가 다 닳았을 때의 예비 안전장치
        if (hpGauge != null && hpGauge.fillAmount <= 0f)
        {
            Time.timeScale = 0f;
            RetryButton.SetActive(true);
            GameOverTXT.SetActive(true);
        }
    }

    public void RetryButtonClicked()
    {
        hpGauge.fillAmount = 1f;
        Time.timeScale = 1f;
        RetryButton.SetActive(false);
        GameOverTXT.SetActive(false);
        
        // 내 캐릭터를 다시 찾아서 활성화하거나 씬을 다시 로드할 수 있습니다.
        GameObject myPlayer = GameObject.FindWithTag("Player");
        if (myPlayer != null)
        {
            myPlayer.SetActive(true);
        }
    }

    public void DecreaseHp()
    {
        if (hpGauge != null)
        {
            hpGauge.fillAmount -= 0.1f;
        }
    }
}