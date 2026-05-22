using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
	[SerializeField] private List<GameObject> frames = new List<GameObject>();
	[SerializeField] private GameObject btn_suivant;
    [SerializeField] private GameObject btn_lancement;
    public int indice = 0;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frames[indice].SetActive(true);
        btn_suivant.SetActive(true);
        btn_lancement.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space)){
            suivant();
        }
    }

    public void lancer(){
        SceneManager.LoadScene("ACCEUIL");
    }

    public void suivant(){
        frames[indice].SetActive(false);
        indice++;
        if (indice < frames.Count){
            frames[indice].SetActive(true);
        } else {
            btn_suivant.SetActive(false);
            btn_lancement.SetActive(true);
        }
    }
}
