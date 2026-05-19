using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel_options;
    [SerializeField] private GameObject panel_credits;
	[SerializeField] private List<GameObject> options_pages = new List<GameObject>();
	private int indice = 0;
	
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
		for each (GameObject page in options_pages){
			page.SetActive(false);	}
		options_pages[0].SetActive(true);
		indice = 0;
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
		if ( options_pages[indice] >= options_pages.Count - 1){
			options_pages[indice] = 0;
		}
		else{
			options_pages[indice] += 1;
		}
	}

	public void NextPage(){
		if ( options_pages[indice] <= 0){
			options_pages[indice] = options_pages.Count - 1;
		}
		else{
			options_pages[indice] -= 1;
		}
	}
}
