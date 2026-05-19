using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel_options;
    [SerializeField] private GameObject panel_credits;
    [SerializeField] private GameObject options_save;
    [SerializeField] private GameObject options_sound;
    [SerializeField] private GameObject options_controls;
	
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

	public void PreviousPage(){
		if (options_save != null && options_save.activeSelf){
			options_save.SetActive(false);
			if (options_sound != null) options_sound.SetActive(true);
		} else if (options_sound != null && options_sound.activeSelf){
			options_sound.SetActive(false);
			if (options_controls != null) options_controls.SetActive(true);
		} else if (options_controls != null && options_controls.activeSelf){
			options_controls.SetActive(false);
			if (options_save != null) options_save.SetActive(true);
		}
	}

	public void NextPage(){
		if (options_controls != null && options_controls.activeSelf){
			options_controls.SetActive(false);
			if (options_sound != null) options_sound.SetActive(true);
		} else if (options_sound != null && options_sound.activeSelf){
			options_sound.SetActive(false);
			if (options_save != null) options_save.SetActive(true);
		} else if (options_save != null && options_save.activeSelf){
			options_save.SetActive(false);
			if (options_controls != null) options_controls.SetActive(true);
		}
	}
}
