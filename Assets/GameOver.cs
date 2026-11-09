using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    Button RetryButton = null;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RetryButton = GameObject.Find("RetryButton").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
