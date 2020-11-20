using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public bool isGamePaused = false;
	public GameObject pauseMenuUI = default;

	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isGamePaused)
				Resume();
			else
				Pause();
		}
    }

	public void Pause()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0F;
		isGamePaused = true;
	}

	public void Resume()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1F;
		isGamePaused = true;
	}

	public void SettingsMenu()
	{
		Debug.Log("This is the settings menu");
	}
	
	public void ExitGame()
	{
		Application.Quit();
	}
}
