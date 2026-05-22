using UnityEngine;
using UnityEngine.SceneManagement;

public class fin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void Quitter(){
        Application.Quit();
    }

    public void Rejouer(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}