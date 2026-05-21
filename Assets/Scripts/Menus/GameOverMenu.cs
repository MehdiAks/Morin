using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
	void Start(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

    public void spawnBack(){
		SceneManager.LoadScene("SOUS TERRAIN");
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
