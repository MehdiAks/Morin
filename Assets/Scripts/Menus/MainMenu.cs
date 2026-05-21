using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenu : MonoBehaviour
{
	[SerializeField] private GameObject panel_credits;
    [SerializeField] private GameObject panel_options;
	[SerializeField] private GameObject save_1;
	[SerializeField] private GameObject save_2;
//	[SerializeField] private GameObject save_3;
	[SerializeField] private GameObject main_sound_off;
//	[SerializeField] private GameObject voice_sound_off;
	[SerializeField] private GameObject sfx_sound_off;
	[SerializeField] private GameObject music_sound_off;
	[SerializeField] private GameObject main_sound_button;
	[SerializeField] private GameObject sfx_sound_button;
	[SerializeField] private GameObject music_sound_button;
	[SerializeField] private GameObject compteur;
	[SerializeField] private List<GameObject> options_pages = new List<GameObject>();
	private int indice = 0;
	public int save_compteur = 0;
	public bool sound_on = true;
	public bool music_on = true;
	public bool sfx_on = true;
	
	void Start(){
		UnshowOptions();
		UnshowCredits();
		if (GameProgress.PremierOuverture){
			GameProgress.PremierOuverture = false;	}
			else {
			GameProgress.LoadSave();	}
	}

	public void PlayGame(){
		SceneManager.LoadScene("ACCEUIL");
	}
	
	public void ShowOptions(){
		panel_options.SetActive(true);
		foreach (GameObject page in options_pages)
			{ page.SetActive(false); }
		indice = 0;
		options_pages[indice].SetActive(true);

		main_sound_off.SetActive(!sound_on);
		sfx_sound_off.SetActive(!sfx_on);
		music_sound_off.SetActive(!music_on);

		save_1.SetActive(false);
		save_2.SetActive(false);
//		save_3.SetActive(false);
		// update tracker menu sauvegarde
		if (PlayerPrefs.GetInt("SalleParcours") == 1) {
			save_1.SetActive(true);
			save_compteur += 1;	}
		if (PlayerPrefs.GetInt("SalleTuyau") == 1) {
			save_2.SetActive(true);
			save_compteur += 1;	}
//		if (PlayerPrefs.GetInt("SalleDevinette") == 1) {
//			save_3.SetActive(true);
//			save_compteur += 1;	}
		
		compteur.GetComponent<TMP_Text>().SetText("Enigmes résolues: " + save_compteur.ToString() + "/2");
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
			AudioManager.instance.SetMain(sound_on);
			if (sound_on) {
				main_sound_button.GetComponent<TMP_Text>().SetText("Activé");
				sfx_sound_button.GetComponent<TMP_Text>().SetText("Activé");
				music_sound_button.GetComponent<TMP_Text>().SetText("Activé");
			} else {
				main_sound_button.GetComponent<TMP_Text>().SetText("Désactivé");
				sfx_sound_button.GetComponent<TMP_Text>().SetText("Désactivé");
				music_sound_button.GetComponent<TMP_Text>().SetText("Désactivé");
			}
			sound_on = !sound_on;
			sfx_on = !sound_on;
			music_on = !sound_on;
			main_sound_off.SetActive(sound_on);
			sfx_sound_off.SetActive(sound_on);
			music_sound_off.SetActive(sound_on);
	}

	//public void SoundVoice(){
	//}

	public void SoundFX(){
		AudioManager.instance.SetSFX(sfx_on);
		if (sfx_on) {
			sfx_sound_button.GetComponent<TMP_Text>().SetText("Activé");
		} else {
			sfx_sound_button.GetComponent<TMP_Text>().SetText("Désactivé");
			}
		sfx_on = !sfx_on;
		sfx_sound_off.SetActive(sfx_on);
	}

	public void SoundMusic(){
		AudioManager.instance.SetMusic(music_on);
		if (music_on) {
			music_sound_button.GetComponent<TMP_Text>().SetText("Activé");
		} else {
			music_sound_button.GetComponent<TMP_Text>().SetText("Désactivé");
			}
		music_on = !music_on;
		music_sound_off.SetActive(music_on);
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
