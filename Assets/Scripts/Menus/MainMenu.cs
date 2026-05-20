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
		if (GameProgress.PremierOuverture){
			GameProgress.PremierOuverture = false;
		} else {
			GameProgress.LoadSave();
		}
	}
	
// To do: change scene name to the intro
	public void PlayGame(){
		SceneManager.LoadScene("Level1");
	}
	
	public void ShowOptions(){
		panel_options.SetActive(true);
		foreach (GameObject page in options_pages)
			{ page.SetActive(false); }
		indice = 0;
		Debug.Log(indice);
		options_pages[indice].SetActive(true);
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
		options_pages[indice].SetActive(false);
		if ( indice <= 0){
			indice = options_pages.Count - 1;
		}
		else{
			indice -= 1;
		}
		options_pages[indice].SetActive(true);
	}

	public void NextPage(){
		options_pages[indice].SetActive(false);
		if ( indice >= options_pages.Count - 1){
			indice = 0;
		}
		else{
			indice += 1;
		}
		options_pages[indice].SetActive(true);
	}

	public void SoundMain(){
		//if on, turn off, if off, turn on
	}

	public void SoundVoice(){
		//if voice track is on, turn off, if off, turn on
	}

	public void SoundFX(){
		//if voice track is on, turn off, if off, turn on
	}

	public void SoundMusic(){
		//if voice track is on, turn off, if off, turn on
	}

	public void EtatSalle(string Salle, bool Valide){
		PlayerPrefs.SetInt(Salle, (Valide ? 1 : 0));
	}

	public void Sauvegarde(){
		EtatSalle("SalleDevinette", (GameProgress.SalleDevinetteValidee ? 1 : 0));
		EtatSalle("SalleTuyau", (GameProgress.SalleTuyauValidee ? 1 : 0));
		EtatSalle("SalleParcours", (GameProgress.SalleParcoursValidee ? 1 : 0));
	}
}
