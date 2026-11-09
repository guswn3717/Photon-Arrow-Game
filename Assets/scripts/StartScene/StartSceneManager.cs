using UnityEngine;
using UnityEngine.SceneManagement;


public class StartSceneManager : MonoBehaviour
{ 
    public void OnClickStart()
    {
        SceneManager.LoadScene("InGame");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
