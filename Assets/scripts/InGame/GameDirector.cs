using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameDirector : MonoBehaviour
{
    [SerializeField] private GameObject RetryButton;
    
    Image hpGauge = null;
    GameObject player = null;
    GameObject GameOverTXT = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpGauge = GameObject.Find("hpGauge").GetComponent<Image>(); //.GetComponent<Image>()를 start로 빼내와서 연산량 줄임
        player = GameObject.Find("player");
        GameOverTXT = GameObject.Find("GameOverTXT");
        
        GameOverTXT.SetActive(false);
        RetryButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hpGauge.fillAmount <= 0f)
        {
            player.SetActive(false);
            Time.timeScale = 0f; // 시간이 흘러가지 않음 (0배속해서 멈춤)
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
        player.SetActive(true);
    }
    

    public void DecreaseHp()
    {
        hpGauge.fillAmount -= 0.1f;
    }
}
