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
	public bool sound_on = true;
	public bool music_on = true;
	public bool sfx_on = true;

	public static PauseMenu instance = null;
	
	void Start(){
		UnshowOptions();
		if (instance == null){
			instance = this;
		}
		AudioManager.instance.PlayMusic(AudioManager.instance.music_list.music1, AudioManager.instance.volume, true);
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (panel_pause.activeSelf){
				ReturnToGame();
			}
			else{
				Pause();
			}
		}

		if (Input.GetKeyDown(KeyCode.P)){
			if (!panel_pause.activeSelf){
				Pause();
			}
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

		main_sound_off.SetActive(!sound_on);
		sfx_sound_off.SetActive(!sfx_on);
		music_sound_off.SetActive(!music_on);

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
		SceneManager.LoadScene("MainMenu");
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
		sfx_on = !sfx_on;
		sfx_sound_off.SetActive(sfx_on);
	}

	public void SoundMusic(){
		AudioManager.instance.SetMusic(music_on);
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