using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTest : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject optionPanel;

    void Start()
    {
        menuPanel.SetActive(true);
        optionPanel.SetActive(false);

    }
    public void StartGame()
    {
        Debug.Log("Démarrer le jeu");
        menuPanel.SetActive(false);
        SceneManager.LoadScene("Level1");
    }
    public void OptionPanel()
    {
        Debug.Log("Ouvrir le panneau d'options");
        //ouvrir le panneau d'options
        menuPanel.SetActive(true);
        optionPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu");
        Application.Quit();
    }

    public void languagePanel()
    {
        Debug.Log("Ouvrir le panneau de langue");
        //ouvrir le panneau de langue
        menuPanel.SetActive(true);
        optionPanel.SetActive(true);
    }

    // Ouvre le menue pause si on appuie sur echap
    void Update()
    {        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
    }
    }

    //ferme le menue pause si on appuie sur le bouton retour
        public void CloseMenu()
        {
            menuPanel.SetActive(false);
        }

        public void ClosePauseMenu()
        {
            menuPanel.SetActive(false);
        }

}
