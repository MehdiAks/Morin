using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class acceuil : MonoBehaviour
{
	[SerializeField] private List<GameObject> frames = new List<GameObject>();
	[SerializeField] private GameObject btn_suivant;
    [SerializeField] private GameObject intro;
    public int indice = 0;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!GameProgress.PremierOuverture){
            intro.SetActive(false);
        } else {
            intro.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
		    Cursor.visible = true;
		    Time.timeScale = 0f;
        }

        foreach (GameObject frame in frames)
			{ frame.SetActive(false); }
        frames[indice].SetActive(true);
        btn_suivant.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space)){
            suivant();
        }
    }

    public void suivant(){
        frames[indice].SetActive(false);
        indice++;
        if (indice < frames.Count){
            frames[indice].SetActive(true);
        } else {
            btn_suivant.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
		    Cursor.visible = false;
		    Time.timeScale = 1f;
            intro.SetActive(false);
        }
    }
}
