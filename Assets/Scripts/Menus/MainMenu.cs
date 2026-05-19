using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel_options;
    [SerializeField] private GameObject panel_credits;
	
	void Start(){
		UnshowOptions();
		UnshowCredits();
	}
	
// To do: change scene name to the intro
	public void PlayGame(){
		SceneManager.LoadScene("Level1");
	}
	
	public void ShowOptions(){
		panel_options.SetActive(true);
	}

	public void ShowCredits(){
		panel_credits.SetActive(true);
	}
	
	public void UnshowOptions(){
		panel_options.SetActive(false);
	}

	public void UnshowCredits(){
		panel_credits.SetActive(false);
	}

	public void QuitGame(){
		Application.Quit();
	}
}
