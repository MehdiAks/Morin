using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject panel_pause;
    [SerializeField] private GameObject panel_options;
	[SerializeField] private GameObject save_1;
	[SerializeField] private GameObject save_2;
	[SerializeField] private GameObject save_3;
	[SerializeField] private GameObject main_sound_off;
	[SerializeField] private GameObject voice_sound_off;
	[SerializeField] private GameObject sfx_sound_off;
	[SerializeField] private GameObject music_sound_off;
	[SerializeField] private GameObject compteur;
	[SerializeField] private List<GameObject> options_pages = new List<GameObject>();
	private int indice = 0;
	public int save_compteur = 0;

	public static PauseMenu instance = null;
	
	void Start(){
		UnshowOptions();
		if (instance == null){
			instance = this;
		}
	}

	public void ReturnToGame(){
		panel_pause.SetActive(false);
	}

	public void Pause(){
		panel_pause.SetActive(true);
	}
	
	public void ShowOptions(){
		panel_options.SetActive(true);
		foreach (GameObject page in options_pages)
			{ page.SetActive(false); }
		indice = 0;
		options_pages[indice].SetActive(true);

		save_1.SetActive(false);
		save_2.SetActive(false);
		save_3.SetActive(false);
		// update tracker menu sauvegarde
		if (PlayerPrefs.GetInt("SalleDevinette") == 1) {
			save_1.SetActive(true);
			save_compteur += 1;	}
		if (PlayerPrefs.GetInt("SalleTuyau") == 1) {
			save_2.SetActive(true);
			save_compteur += 1;	}
		if (PlayerPrefs.GetInt("SalleParcours") == 1) {
			save_3.SetActive(true);
			save_compteur += 1;	}
		
		compteur.GetComponent<TMP_Text>().SetText("Enigmes résolues: " + save_compteur.ToString() + "/3");
	}

	public void UnshowOptions(){
		panel_options.SetActive(false);
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
		
	}

	public void SoundVoice(){

	}

	public void SoundFX(){

	}

	public void SoundMusic(){

	}

	public void EtatSalle(string Salle, bool Valide){
		PlayerPrefs.SetInt(Salle, (Valide ? 1 : 0));
	}

	public void Sauvegarde(){
		EtatSalle("SalleDevinette", GameProgress.SalleDevinetteValidee);
		EtatSalle("SalleTuyau", GameProgress.SalleTuyauValidee);
		EtatSalle("SalleParcours", GameProgress.SalleParcoursValidee);
	}
}